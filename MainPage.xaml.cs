using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using org.dgl.win98labyrinth.Models;

namespace org.dgl.win98labyrinth
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// riferimento all'oggetto game
        /// </summary>
        private Game _game;
        /// <summary>
        /// variabile di appoggio durante il drag & drop
        /// </summary>
        private GameBoardTile _draggedTile;

        public MainPage()
        {
            InitializeComponent();
            //forza le dimensioni del tabellone affinchè sia un quadrato
            GameBoardGrid.SizeChanged += (s, e) =>
            {
                if (GameBoardGrid.Width > GameBoardGrid.Height)
                {
                    GameBoardGrid.WidthRequest = GameBoardGrid.Height;
                }
                else if (GameBoardGrid.Width < GameBoardGrid.Height)
                {
                    GameBoardGrid.HeightRequest = GameBoardGrid.Width;
                }
            };
            NewGame();
        }

        /// <summary>
        /// inizializza una nuova partita
        /// </summary>
        public void NewGame()
        {
            _game = new Game();
            //aggiorna il tabellone
            RefreshHeader();
            RefreshGrid();
            RefreshFooter();
        }

        /// <summary>
        /// aggiorna l'intestazione
        /// mostra il nome del giocatore. lo sfondo equivale al colore del giocatore
        /// abilita il tasto fine turno se il giocatore ha mosso la tessera
        /// </summary>
        private void RefreshHeader()
        {
            GameHeaderLabel.Text = _game.CurrentPlayer.Name;
            GameHeaderLabel.BackgroundColor = _game.CurrentPlayer.Color;
            if (_game.CurrentPlayer.Prizes.Count == 0 && _game.CurrentPlayer.IsInStartPosition)
            {
                _game.CurrentPlayer.HasCollectedPrize = true;
                GameHeaderButton.Text = "VITTORIA";
                GameHeaderButton.IsEnabled = false;
            }
            else
                GameHeaderButton.IsEnabled = _game.CurrentPlayer.HasMovedTile;
        }

        /// <summary>
        /// aggiorna il fondo pagina
        /// mostra l'obiettivo corrente e aggiorna il numero di obiettivi rimanenti
        /// </summary>
        private void RefreshFooter()
        {
            GameFooterActual.Text = $"{(_game.CurrentPlayer.Prizes.Count > 0 ? _game.CurrentPlayer.Prizes[0] : "FINE")}";
            GameFooterRemaining.Text = $"{_game.CurrentPlayer.Prizes.Count}";
        }

        /// <summary>
        /// aggiorna il tabellone delle tessere
        /// prima elimina tutti i componenti grafici 
        /// li inserisce tutti (TODO ottimizzare)
        /// aggiunge gli eventi di drag drop e tap
        /// </summary>
        private void RefreshGrid()
        {
            Rectangle playerRectangle = null;
            Rectangle homeRectangle = null;
            GameBoardGrid.Children.Clear();
            //inizializza il tabellone
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    GameBoardTile tile = _game.Board.Tiles[y][x];
                    Grid container = new Grid();
                    //tessera
                    Image image = new Image()
                    {
                        Source = tile.GetRotatedImage()
                    };
                    //obiettivo
                    Label label = new Label()
                    {
                        Text = tile.Prize,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 32,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.Black,
                        BackgroundColor = Colors.Transparent
                    };
                    //pedina giocatore
                    playerRectangle = null;
                    if (_game.GetPlayerByPosition(y, x) is GamePlayer p)
                    {
                        playerRectangle = CreatePlayerRectangle(p);
                    }
                    //tessera di partenza giocatore
                    homeRectangle = null;
                    if (_game.GetPlayerByStartPosition(y, x) is GamePlayer pp)
                    {
                        homeRectangle = CreatePlayerHomeRectangle(pp);
                    }
                    //disegna gli elementi
                    tile.GridReference = container;
                    tile.ImageReference = image;
                    tile.LabelReference = label;
                    container.Children.Add(image);
                    container.Children.Add(label);
                    if (homeRectangle != null) container.Children.Add(homeRectangle);
                    if (playerRectangle != null) container.Children.Add(playerRectangle);
                    GameBoardGrid.Add(container, x, y);
                    container.GestureRecognizers.Add(new TapGestureRecognizer());
                    ((TapGestureRecognizer)tile.GridReference.GestureRecognizers[0]).Tapped += (s, e) =>
                    {
                        OnTapToMoveEvent(tile);
                    };
                }
            }
            //riconosce gli eventi TAP e DragDrop degli elementi
            RefreshGridAddGestures();
        }

        /// <summary>
        /// genera l'elemento grafico della pedina del giocatore
        /// </summary>
        /// <param name="player">l'istanza del giocatore</param>
        /// <returns>l'elemento grafico della pedina del giocatore</returns>
        private Rectangle CreatePlayerRectangle(GamePlayer player)
        {
            Rectangle output;
            if (player == null)
                return null;
            output = new Rectangle()
            {
                Fill = new SolidColorBrush(player.Color),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                RadiusX = 100,
                RadiusY = 100
            };
            output.SizeChanged += (s, e) =>
            {
                if (s == null) return;
                ((Rectangle)s).WidthRequest = GameBoardGrid.Width / 20;
                ((Rectangle)s).HeightRequest = GameBoardGrid.Height / 20;
            };
            return output;
        }

        /// <summary>
        /// genera l'elemento grafico della pedina del giocatore
        /// </summary>
        /// <param name="player">l'istanza del giocatore</param>
        /// <returns>l'elemento grafico della pedina del giocatore</returns>
        private Rectangle CreatePlayerHomeRectangle(GamePlayer player)
        {
            Rectangle output;
            if (player == null)
                return null;
            output = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(player.Color),
                StrokeThickness = 3,
                RadiusX = 100,
                RadiusY = 100
            };
            output.SizeChanged += (s, e) =>
            {
                if (s == null) return;
                ((Rectangle)s).WidthRequest = GameBoardGrid.Width / 20;
                ((Rectangle)s).HeightRequest = GameBoardGrid.Height / 20;
            };
            return output;
        }



        /// <summary>
        /// assegna gli eventi TAP e DRAGDROP agli elementi grafici
        /// </summary>
        private void RefreshGridAddGestures()
        {
            _game.Board.Tiles[0][0].GridReference.GestureRecognizers.Clear();
            _game.Board.Tiles[0][0].GridReference.GestureRecognizers.Add(new DragGestureRecognizer());
            _game.Board.Tiles[0][0].GridReference.GestureRecognizers.Add(new TapGestureRecognizer());
            ((DragGestureRecognizer)_game.Board.Tiles[0][0].GridReference.GestureRecognizers[0]).DragStarting += (s, e) =>
            {
                if (_game.CurrentPlayer.HasMovedTile)
                {
                    e.Cancel = true;
                    return;
                }
                if (!_game.Board.Tiles[0][0].CanGoNorth
                    && !_game.Board.Tiles[0][0].CanGoSouth
                    && !_game.Board.Tiles[0][0].CanGoEast
                    && !_game.Board.Tiles[0][0].CanGoWest)
                {
                    e.Cancel = true;
                    return;
                }
                TryVibrate(100);
                _draggedTile = (_game.Board.Tiles[0][0]);
            };
            ((TapGestureRecognizer)_game.Board.Tiles[0][0].GridReference.GestureRecognizers[1]).Tapped += (s, e) =>
            {
                OnTapToRotateEvent(_game.Board.Tiles[0][0]);
            };
            foreach (GameBoardTile tile in new GameBoardTile[] { _game.Board.Tiles[0][2], _game.Board.Tiles[0][4], _game.Board.Tiles[0][6],
                                                                _game.Board.Tiles[2][0], _game.Board.Tiles[4][0], _game.Board.Tiles[6][0],
                                                                _game.Board.Tiles[8][2], _game.Board.Tiles[8][4], _game.Board.Tiles[8][6],
                                                                _game.Board.Tiles[2][8], _game.Board.Tiles[4][8], _game.Board.Tiles[6][8],})
            {
                tile.GridReference.GestureRecognizers.Clear();
                tile.GridReference.GestureRecognizers.Add(new DragGestureRecognizer());
                tile.GridReference.GestureRecognizers.Add(new TapGestureRecognizer());
                tile.GridReference.GestureRecognizers.Add(new DropGestureRecognizer());
                ((DragGestureRecognizer)tile.GridReference.GestureRecognizers[0]).DragStarting += (s, e) =>
                {
                    if (_game.CurrentPlayer.HasMovedTile)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (!tile.CanGoNorth
                        && !tile.CanGoSouth
                        && !tile.CanGoEast
                        && !tile.CanGoWest)
                    {
                        e.Cancel = true;
                        return;
                    }
                    TryVibrate(100);
                    _draggedTile = (tile);
                };
                ((TapGestureRecognizer)tile.GridReference.GestureRecognizers[1]).Tapped += (s, e) =>
                {
                    OnTapToRotateEvent(tile);
                };
                ((DropGestureRecognizer)tile.GridReference.GestureRecognizers[2]).Drop += (s, e) =>
                {
                    if (_game.CurrentPlayer.HasMovedTile)
                        return;
                    if (_draggedTile == tile)
                        return;
                    _game.PushTilesFrom(_draggedTile, tile);
                    _game.CurrentPlayer.HasMovedTile = true;
                    RefreshHeader();
                    RefreshGrid();
                    RefreshFooter();
                };
            }
        }


        /// <summary>
        /// quando viene premuta la tessera viene mosso il giocatore su questa
        /// solo se il giocatore ha già mosso la tessera
        /// solo se il giocatore è adiacente ed è il giocatore corrente
        /// solo se le tessere sono collegate
        /// </summary>
        /// <param name="toTile">casella di destinazione</param>
        private void OnTapToMoveEvent(GameBoardTile toTile)
        {
            GameBoardTile fromTile;
            int toX, toY;
            _game.Board.GetTilePosition(toTile, out toY, out toX);
            if (toX < 1 || toY > 7)
                return; //casella non trovata o fuori tabellone
            if (!_game.CurrentPlayer.HasMovedTile
                || _game.CurrentPlayer.HasCollectedPrize)
                return; //il giocatore non ha ancora mosso la tessera o ha già raccolto obiettivo
            if ((_game.CurrentPlayer.XPosition != toX - 1 && _game.CurrentPlayer.XPosition != toX + 1)
                && (_game.CurrentPlayer.YPosition != toY - 1 && _game.CurrentPlayer.YPosition != toY + 1))
                return; //il giocatore non è adiacente
            fromTile = _game.Board.Tiles[_game.CurrentPlayer.YPosition][_game.CurrentPlayer.XPosition];
            //anti-diagonale
            if (_game.CurrentPlayer.XPosition != toX
                && _game.CurrentPlayer.YPosition != toY)
                return;
            //verticale
            if (_game.CurrentPlayer.XPosition == toX)
            {
                //da alto
                if ((_game.CurrentPlayer.YPosition < toY)
                    && (!fromTile.CanGoSouth || !toTile.CanGoNorth))
                    return;
                //da basso
                if ((_game.CurrentPlayer.YPosition > toY)
                    && (!fromTile.CanGoNorth || !toTile.CanGoSouth))
                    return;
            }
            //orizzontale
            if (_game.CurrentPlayer.YPosition == toY)
            {
                //da destra
                if ((_game.CurrentPlayer.XPosition < toX)
                    && (!fromTile.CanGoEast || !toTile.CanGoWest))
                    return;
                //da sinistra
                if ((_game.CurrentPlayer.XPosition > toX)
                    && (!fromTile.CanGoWest || !toTile.CanGoEast))
                    return;
            }
            //raccoglie il premio
            if ((!string.IsNullOrEmpty(toTile.Prize)
                && !_game.CurrentPlayer.HasCollectedPrize)
                && _game.CurrentPlayer.Prizes.Count > 0
                && toTile.Prize == _game.CurrentPlayer.Prizes[0])
            {
                _game.CurrentPlayer.Prizes.RemoveAt(0);
                _game.CurrentPlayer.HasCollectedPrize = true;
            }
            //aggiorna la posizione
            _game.CurrentPlayer.XPosition = toX;
            _game.CurrentPlayer.YPosition = toY;
            //verifica vittoria
            RefreshHeader();
            RefreshGrid();
            RefreshFooter();
        }

        /// <summary>
        /// quando viene premuta la tessera viene ruotata
        /// non si può fare se il giocatore ha già mosso
        /// non si può fare sulle tessere a freccia
        /// </summary>
        /// <param name="p_tile">riferimento alla tessera</param>
        private void OnTapToRotateEvent(GameBoardTile p_tile)
        {
            if (_game.CurrentPlayer.HasMovedTile) return;
            if (!(p_tile.CanGoNorth || p_tile.CanGoSouth || p_tile.CanGoWest || p_tile.CanGoEast)) return;
            p_tile.RotateClockwise();
            p_tile.ImageReference.Source = p_tile.GetRotatedImage();
        }

        /// <summary>
        /// quando finisce il turno si resetta quello corrente e si passa al giocatore successivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameHeaderButton_Clicked(object sender, EventArgs e)
        {
            _game.NextTurn();
            RefreshHeader();
            RefreshFooter();
        }

        /// <summary>
        /// se il dispositivo lo supporta vibra per la durata indicata
        /// in iOS/MAC il default è sempre 500ms
        /// </summary>
        /// <param name="howLong">durata in ms</param>
        private void TryVibrate(double howLong)
        {
            try
            {
                Vibration.Default.Vibrate(howLong);
            }
            catch (Exception ignored) { }
        }
    }

}
