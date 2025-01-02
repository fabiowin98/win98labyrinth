using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.dgl.win98labyrinth.Models
{
    internal class Game
    {
        /// <summary>
        /// riferimento alla plancia di gioco
        /// </summary>
        public GameBoard Board { get; set; }
        /// <summary>
        /// riferimento giocatore 1
        /// </summary>
        public GamePlayer Player1 { get; set; }
        /// <summary>
        /// riferimento giocatore 2
        /// </summary>
        public GamePlayer Player2 { get; set; }
        /// <summary>
        /// riferimento giocatore 3
        /// </summary>
        public GamePlayer Player3 { get; set; }
        /// <summary>
        /// riferimento giocatore 4
        /// </summary>
        public GamePlayer Player4 { get; set; }
        /// <summary>
        /// indica il turno attuale da 1 a 4
        /// </summary>
        public int Turn { get; set; }
        /// <summary>
        /// restituisce il giocatore attuale in base al turno
        /// </summary>
        public GamePlayer CurrentPlayer
        {
            get
            {
                switch (Turn)
                {
                    case 0:
                        return Player1;
                        break;
                    case 1:
                        return Player2;
                        break;
                    case 2:
                        return Player3;
                        break;
                    case 3:
                        return Player4;
                        break;
                }
                return null;
            }
        }

        /// <summary>
        /// nuovo gioco
        /// </summary>
        public Game()
        {
            //inizializza oggetti
            Board = new GameBoard();
            Player1 = new GamePlayer() { Name = "GIOCATORE 1", Color = Color.FromRgba(255,0,0,190), XPosition = 1, YPosition = 1, XStartPosition = 1, YStartPosition = 1 };
            Player2 = new GamePlayer() { Name = "GIOCATORE 2", Color = Color.FromRgba(0, 255, 0, 190), XPosition = 7, YPosition = 1, XStartPosition = 7, YStartPosition = 1 };
            Player3 = new GamePlayer() { Name = "GIOCATORE 3", Color = Color.FromRgba(0, 0, 255, 190), XPosition = 7, YPosition = 7, XStartPosition = 7, YStartPosition = 7 };
            Player4 = new GamePlayer() { Name = "GIOCATORE 4", Color = Color.FromRgba(255, 255, 0, 190), XPosition = 1, YPosition = 7, XStartPosition = 1, YStartPosition = 7 };
            //distribuisce gli obiettivi
            List<string> prizes = "ABCDEFGHIJKLMNOPQRSTUVWX".ToCharArray().Select(c => c.ToString()).ToList();
            Random random = new Random();
            int prizeIndex;
            while (prizes.Count > 0)
            {
                prizeIndex = random.Next(prizes.Count);
                Player1.Prizes.Add(prizes[prizeIndex]);
                prizes.RemoveAt(prizeIndex);
                prizeIndex = random.Next(prizes.Count);
                Player2.Prizes.Add(prizes[prizeIndex]);
                prizes.RemoveAt(prizeIndex);
                prizeIndex = random.Next(prizes.Count);
                Player3.Prizes.Add(prizes[prizeIndex]);
                prizes.RemoveAt(prizeIndex);
                prizeIndex = random.Next(prizes.Count);
                Player4.Prizes.Add(prizes[prizeIndex]);
                prizes.RemoveAt(prizeIndex);
            }
        }

        /// <summary>
        /// ripristina il giocatore corrente
        /// cambia il valore del turno
        /// </summary>
        public void NextTurn()
        {
            CurrentPlayer.Reset();
            if (Turn == 3)
                Turn = -1;
            Turn++;
        }

        /// <summary>
        /// sposta le tessere orizzontalmente o verticalmente a seconda di dove viene trascinata una tessera
        /// </summary>
        /// <param name="p_what">tessera trascinata</param>
        /// <param name="p_where">tessera freccia destinazione</param>
        public void PushTilesFrom(GameBoardTile p_what, GameBoardTile p_where)
        {
            int x, y;
            Board.GetTilePosition(p_where, out y, out x);
            PushTilesFrom(p_what, y, x);
            Player1.Reposition();
            Player2.Reposition();
            Player3.Reposition();
            Player4.Reposition();
        }

        /// <summary>
        /// sposta le tessere orizzontalmente o verticalmente a seconda di dove viene trascinata una tessera
        /// </summary>
        /// <param name="p_tile">tessera trascinata</param>
        /// <param name="p_y">coordinate della tessera dove è stata trascinata</param>
        /// <param name="p_x">coordinate della tessera dove è stata trascinata</param>
        private void PushTilesFrom(GameBoardTile p_tile, int p_y, int p_x)
        {
            int x = p_x;
            int y = p_y;
            int d = 0;
            int start, stop;
            x = p_x;
            y = p_y;
            if (p_x == p_y) return;
            Board.ResetArrowTile(p_tile);
            if (p_y == 0 || p_y == 8)   //verticale 
            {
                start = p_y == 0 ? 7 : 1;
                stop = p_y == 0 ? 1 : 7;
                d = p_y == 0 ? -1 : +1;
                Board.Tiles[start - d][x] = Board.Tiles[start][x];
                GetPlayerByPosition(start, x)?.MoveTo(start - d, x);
                for (y = start; p_y == 0 ? y > stop : y < stop; y += d)
                {
                    Board.Tiles[y][x] = Board.Tiles[y + d][x];
                    GetPlayerByPosition(y + d, x)?.MoveTo(y, x);
                }
                Board.Tiles[stop][x] = p_tile;
            }
            if (p_x == 0 || p_x == 8)   //orizzontale 
            {
                start = p_x == 0 ? 7 : 1;
                stop = p_x == 0 ? 1 : 7;
                d = p_x == 0 ? -1 : +1;
                Board.Tiles[y][start - d] = Board.Tiles[y][start];
                GetPlayerByPosition(y, start)?.MoveTo(y, start - d);
                for (x = start; p_x == 0 ? x > stop : x < stop; x += d)
                {
                    Board.Tiles[y][x] = Board.Tiles[y][x + d];
                    GetPlayerByPosition(y, x + d)?.MoveTo(y, x);
                }
                Board.Tiles[y][stop] = p_tile;
            }
        }

        /// <summary>
        /// restituisce il giocatore che si trova alle coordinate specificate
        /// </summary>
        /// <param name="p_x">coordinata x</param>
        /// <param name="p_y">coordinata y</param>
        /// <returns>NULL se non trova il giocatore</returns>
        public GamePlayer GetPlayerByPosition(int p_y, int p_x)
        {
            foreach (GamePlayer player in new GamePlayer[] { Player1, Player2, Player3, Player4 })
            {
                if (player.XPosition == p_x && player.YPosition == p_y)
                    return player;
            }
            return null;
        }

        /// <summary>
        /// restituisce il giocatore se corrisponde alla coordinata di partenza
        /// </summary>
        /// <param name="p_x">coordinata x</param>
        /// <param name="p_y">coordinata y</param>
        /// <returns>NULL se non trova il giocatore</returns>
        public GamePlayer GetPlayerByStartPosition(int p_y, int p_x)
        {
            foreach (GamePlayer player in new GamePlayer[] { Player1, Player2, Player3, Player4 })
            {
                if (player.XStartPosition == p_x && player.YStartPosition == p_y)
                    return player;
            }
            return null;
        }

    }
}
