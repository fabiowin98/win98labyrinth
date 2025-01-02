using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Modifica senza merge dal progetto 'Win98Labirinto (net9.0-ios)'
Aggiunto:
using Win98Labirinto;
using Win98Labirinto.Models;
using Win98Labirinto.Models;
using Win98Labirinto.Models.GameBoard;
*/

namespace org.dgl.win98labyrinth.Models
{
    /// <summary>
    /// tessera di gioco generica
    /// </summary>
    internal class GameBoardTile
    {
        /// <summary>
        /// posizione X all'interno del tabellone
        /// </summary>
        public int XPosition { get; set; }
        /// <summary>
        /// posizione Y all'interno del tabellone
        /// </summary>
        public int YPosition { get; set; }
        /// <summary>
        /// flag che abilita il movimento verso l'alto
        /// </summary>
        public bool CanGoNorth { get; set; }
        /// <summary>
        /// flag che abilita il movimento verso il basso
        /// </summary>
        public bool CanGoSouth { get; set; }
        /// <summary>
        /// flag che abilita il movimento verso sinistra
        /// </summary>
        public bool CanGoEast { get; set; }
        /// <summary>
        /// flag che abilita il movimento verso destra
        /// </summary>
        public bool CanGoWest { get; set; }
        /// <summary>
        /// Oggetto della tessera
        /// NULL o stringa vuota se la tessera non ha un oggetto
        /// </summary>
        public string? Prize { get; set; }
        /// <summary>
        /// Nome del file di risorsa immagine
        /// </summary>
        public string ImageFilename { get; set; }
        /// <summary>
        /// valore di rotazione, serve per disegnare l'immagine
        /// </summary>
        private int _rotation;
        /// <summary>
        /// riferimento all'oggetto grafico tassello
        /// </summary>
        public Grid GridReference { get; set; }
        /// <summary>
        /// riferimento all'oggetto grafico image
        /// </summary>
        public Image ImageReference { get; set; }
        /// <summary>
        /// riferimento all'oggetto grafico label
        /// </summary>
        public Label LabelReference { get; set; }

        /// <summary>
        /// ruota la tessera in senso orario
        /// aggiorna la proprietà rotation
        /// </summary>
        /// <returns>istanza dell'oggetto</returns>
        public GameBoardTile RotateClockwise()
        {
            bool aux;
            _rotation += 90;
            if (_rotation > 270) _rotation = 0;
            aux = CanGoNorth;
            CanGoNorth = CanGoWest;
            CanGoWest = CanGoSouth;
            CanGoSouth = CanGoEast;
            CanGoEast = aux;
            return this;
        }

        /// <summary>
        /// ruota la tessera in senso antiorario
        /// aggiorna la proprietà rotation
        /// </summary>
        /// <returns>istanza dell'oggetto</returns>
        public GameBoardTile RotateCounterClockwise()
        {
            bool aux;
            _rotation -= 90;
            if (_rotation < 0) _rotation = 270;
            aux = CanGoNorth;
            CanGoNorth = CanGoEast;
            CanGoEast = CanGoSouth;
            CanGoSouth = CanGoWest;
            CanGoWest = aux;
            return this; 
        }

        /// <summary>
        /// restituisce il valore di rotazione attuale, per disegnare l'immagine
        /// </summary>
        /// <returns>gradi di rotazione</returns>
        public double GetRotation()
        {
            return _rotation;
        }

        /// <summary>
        /// restituisce il nome del file già ruotato
        /// necessario perchè il drag e drop non gestisce la rotazione in anteprima
        /// </summary>
        /// <returns>il nome della risorsa immagine</returns>
        public string GetRotatedImage()
        {
            return string.IsNullOrEmpty(ImageFilename) ? "" : $"{ImageFilename.Split('.')[0]}{GetRotation()}.{ImageFilename.Split('.')[1]}";
        }

        /// <summary>
        /// restituisce una nuova istanza di una tessera dritta
        /// </summary>
        /// <param name="in_prize">se specificato, imposta il premio</param>
        /// <returns>istanza della tessera</returns>
        public static GameBoardTile NewStraightTile(string? in_prize = null)
        {
            return new GameBoardTile()
            {
                CanGoNorth = true,
                CanGoSouth = true,
                ImageFilename = "straight.png",
                Prize = in_prize
            };
        }

        /// <summary>
        /// restituisce una nuova istanza di una tessera ad incrocio
        /// </summary>
        /// <param name="in_prize">se specificato, imposta il premio</param>
        /// <returns>istanza della tessera</returns>
        public static GameBoardTile NewCrossTile(string? in_prize = null)
        {
            return new GameBoardTile()
            {
                CanGoNorth = true,
                CanGoEast = true,
                CanGoSouth = true,
                ImageFilename = "cross.png",
                Prize = in_prize
            };
        }

        /// <summary>
        /// restituisce una nuova istanza di una tessera ad angolo
        /// </summary>
        /// <param name="in_prize">se specificato, imposta il premio</param>
        /// <returns>istanza della tessera</returns>
        public static GameBoardTile NewCornerTile(string? in_prize = null)
        {
            return new GameBoardTile()
            {
                CanGoNorth = true,
                CanGoEast = true,
                ImageFilename = "corner.png",
                Prize = in_prize
            };
        }

    }
}
