using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace org.dgl.win98labyrinth.Models
{
    internal class GameBoard
    {
        public GameBoardTile[][] Tiles { get; set; }

        public GameBoard()
        {
            Tiles = new GameBoardTile[9][];
            //inizializza tutte le tessere come bloccate
            for (int y = 0; y < 9; y++)
            {
                Tiles[y] = new GameBoardTile[9];
                for (int x = 0; x < 9; x++)
                {
                    Tiles[y][x] = new GameBoardTile();
                }
            }
            //imposta le tessere con la freccia
            Tiles[0][2].ImageFilename = "down.png";
            Tiles[0][4].ImageFilename = "down.png";
            Tiles[0][6].ImageFilename = "down.png";
            Tiles[2][0].ImageFilename = "right.png";
            Tiles[4][0].ImageFilename = "right.png";
            Tiles[6][0].ImageFilename = "right.png";
            Tiles[8][2].ImageFilename = "up.png";
            Tiles[8][4].ImageFilename = "up.png";
            Tiles[8][6].ImageFilename = "up.png";
            Tiles[2][8].ImageFilename = "left.png";
            Tiles[4][8].ImageFilename = "left.png";
            Tiles[6][8].ImageFilename = "left.png";
            //imposta le tessere fisse
            //angoli fissi
            Tiles[1][1] = GameBoardTile.NewCornerTile().RotateClockwise();
            Tiles[7][1] = GameBoardTile.NewCornerTile();
            Tiles[1][7] = GameBoardTile.NewCornerTile().RotateClockwise().RotateClockwise();
            Tiles[7][7] = GameBoardTile.NewCornerTile().RotateCounterClockwise();
            Tiles[1][3] = GameBoardTile.NewCrossTile("A").RotateClockwise();
            //incroci fissi
            Tiles[1][5] = GameBoardTile.NewCrossTile("B").RotateClockwise();
            Tiles[3][1] = GameBoardTile.NewCrossTile("C");
            Tiles[3][3] = GameBoardTile.NewCrossTile("D");
            Tiles[3][5] = GameBoardTile.NewCrossTile("E").RotateClockwise();
            Tiles[3][7] = GameBoardTile.NewCrossTile("F").RotateClockwise().RotateClockwise();
            Tiles[5][1] = GameBoardTile.NewCrossTile("G");
            Tiles[5][3] = GameBoardTile.NewCrossTile("I").RotateCounterClockwise();
            Tiles[5][5] = GameBoardTile.NewCrossTile("H").RotateCounterClockwise().RotateCounterClockwise();
            Tiles[5][7] = GameBoardTile.NewCrossTile("J").RotateCounterClockwise().RotateCounterClockwise();
            Tiles[7][3] = GameBoardTile.NewCrossTile("K").RotateCounterClockwise();
            Tiles[7][5] = GameBoardTile.NewCrossTile("L").RotateCounterClockwise();
            //tessere casuali
            InitializeRandomTiles();
        }

        /// <summary>
        /// inizializza le tessere casuali
        /// </summary>
        private void InitializeRandomTiles()
        {
            int[] yPositions = new int[] { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7 };
            int[] xPositions = new int[] { 2, 4, 6, 1, 2, 3, 4, 5, 6, 7, 2, 4, 6, 1, 2, 3, 4, 5, 6, 7, 2, 4, 6, 1, 2, 3, 4, 5, 6, 7, 2, 4, 6 };
            string[] prizes = new string[] { "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X" };
            int y, x;
            int positionIndex = 0;
            int prizeIndex = -1;
            int remainingStraightTiles = 12;
            int remainingCornerTiles = 10;
            int remainingCornerTilesWithPrize = 6;
            int remainingCrossTilesWithPrize = 6;
            Random random = new Random();
            while (remainingStraightTiles > 0
                || remainingCornerTiles > 0
                || remainingCornerTilesWithPrize > 0
                || remainingCrossTilesWithPrize > 0)
            {
                //coordinate della tessera
                y = positionIndex < yPositions.Length ? yPositions[positionIndex] : 0;
                x = positionIndex < xPositions.Length ? xPositions[positionIndex] : 0;
                //genera una tessera casuale se ancora a disposizione
                switch (random.Next(4))
                {
                    case 0: //dritto
                        if (remainingStraightTiles <= 0) continue;
                        remainingStraightTiles--;
                        Tiles[y][x] = GameBoardTile.NewStraightTile();
                        break;
                    case 1: //angolo vuoto
                        if (remainingCornerTiles <= 0) continue;
                        remainingCornerTiles--;
                        Tiles[y][x] = GameBoardTile.NewCornerTile();
                        break;
                    case 2: //angolo con premio
                        if (remainingCornerTilesWithPrize <= 0) continue;
                        remainingCornerTilesWithPrize--;
                        Tiles[y][x] = GameBoardTile.NewCornerTile(prizes[++prizeIndex]);
                        break;
                    case 3: //incrocio con premio
                        if (remainingCrossTilesWithPrize <= 0) continue;
                        remainingCrossTilesWithPrize--;
                        Tiles[y][x] = GameBoardTile.NewCrossTile(prizes[++prizeIndex]);
                        break;
                }
                //aggiunge una rotazione casuale
                for (int i = 0; i < random.Next(4); i++)
                    Tiles[y][x].RotateClockwise();
                //prossima tessera
                positionIndex++;
            }
        }

        /// <summary>
        /// restituisce le coordinate x e y di una tessera
        /// </summary>
        /// <param name="p_where">tessera da trovare</param>
        /// <param name="out_x">assegna in uscita il valore X</param>
        /// <param name="out_y">assegna in uscita il valore Y</param>
        public void GetTilePosition(GameBoardTile p_where, out int out_y, out int out_x)
        {
            int x, y;
            out_x = -1;
            out_y = -1;
            for (y = 0; y < 9; y++)
                for (x = 0; x < 9; x++)
                    if (Tiles[y][x] == p_where)
                    {
                        out_x = x;
                        out_y = y;
                        return;
                    }
        }

        /// <summary>
        /// ripristina la casella con la freccia
        /// </summary>
        /// <param name="p_tile">riferimento alla casella</param>
        public void ResetArrowTile(GameBoardTile p_tile)
        {
            int x, y;
            for (y = 0; y < 9; y++)
                for (x = 0; x < 9; x++)
                    if (Tiles[y][x] == p_tile)
                    {
                        Tiles[y][x] = new GameBoardTile();
                        if (y == 0 && x == 2) Tiles[y][x].ImageFilename = "down.png";
                        if (y == 0 && x == 4) Tiles[y][x].ImageFilename = "down.png";
                        if (y == 0 && x == 6) Tiles[y][x].ImageFilename = "down.png";
                        if (y == 2 && x == 0) Tiles[y][x].ImageFilename = "right.png";
                        if (y == 4 && x == 0) Tiles[y][x].ImageFilename = "right.png";
                        if (y == 6 && x == 0) Tiles[y][x].ImageFilename = "right.png";
                        if (y == 8 && x == 2) Tiles[y][x].ImageFilename = "up.png";
                        if (y == 8 && x == 4) Tiles[y][x].ImageFilename = "up.png";
                        if (y == 8 && x == 6) Tiles[y][x].ImageFilename = "up.png";
                        if (y == 2 && x == 8) Tiles[y][x].ImageFilename = "left.png";
                        if (y == 4 && x == 8) Tiles[y][x].ImageFilename = "left.png";
                        if (y == 6 && x == 8) Tiles[y][x].ImageFilename = "left.png";
                    }
        }

        /// <summary>
        /// restituisce l'istanza della tessera che ha il riferimento all'oggetto grafico passato alla funzione
        /// </summary>
        /// <param name="in_reference">riferimento all'oggetto grafico (Grid, Image o Label)</param>
        /// <returns>istanza della tessera</returns>
        public GameBoardTile GetTileByReference(object in_reference)
        {
            for (int y = 0; y < Tiles.Length; y++)
                for (int x = 0; x < Tiles.Length; x++)
                {
                    if (in_reference.GetType() == typeof(DragGestureRecognizer)
                        && Tiles[y][x].GridReference == ((DragGestureRecognizer)in_reference).Parent)
                        return Tiles[y][x];
                    if (in_reference.GetType() == typeof(Grid) && Tiles[y][x].GridReference == in_reference)
                        return Tiles[y][x];
                    if (in_reference.GetType() == typeof(Image) && Tiles[y][x].ImageReference == in_reference)
                        return Tiles[y][x];
                    if (in_reference.GetType() == typeof(Label) && Tiles[y][x].LabelReference == in_reference)
                        return Tiles[y][x];
                }
            return null;
        }

    }
}
