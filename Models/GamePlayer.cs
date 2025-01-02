using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.dgl.win98labyrinth.Models
{
    internal class GamePlayer
    {
        /// <summary>
        /// nome del giocatore
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// colore della pedina
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// lista degli obiettivi
        /// </summary>
        public List<string> Prizes { get; set; }
        /// <summary>
        /// posizione X nella plancia
        /// </summary>
        public int XPosition { get; set; }
        /// <summary>
        /// posizione Y nella plancia
        /// </summary>
        public int YPosition { get; set; }
        /// <summary>
        /// flag che indica che il giocatore ha spostato la tessera
        /// </summary>
        public bool HasMovedTile { get; set; }
        /// <summary>
        /// flag che indica che il giocatore ha raccolto un obiettivo (max uno per turno)
        /// </summary>
        public bool HasCollectedPrize { get; set; }
        /// <summary>
        /// posizione di partenza/arrivo del giocatore
        /// </summary>
        public int XStartPosition { get; set; }
        /// <summary>
        /// posizione di partenza/arrivo del giocatore
        /// </summary>
        public int YStartPosition { get; set; }
        /// <summary>
        /// flag che indica se il giocatore è nella casella di partenza
        /// </summary>
        public bool IsInStartPosition
        {
            get
            {
                return XPosition == XStartPosition && YPosition == YStartPosition;
            }
        }

        /// <summary>
        /// inizializza l'oggetto
        /// </summary>
        public GamePlayer()
        {
            Prizes = new List<string>();
        }

        /// <summary>
        /// aggiorna entrambe le coordinate in una chiamata
        /// </summary>
        /// <param name="p_y">nuova coordinata Y</param>
        /// <param name="p_x">nuova coordinata X</param>
        public void MoveTo(int p_y, int p_x)
        {
            XPosition = p_x;
            YPosition = p_y;
        }

        /// <summary>
        /// riposiziona un giocatore nel caso in cui sia uscito dal tabellone
        /// </summary>
        public void Reposition()
        {
            if (XPosition < 1) XPosition = 7;
            else if (XPosition > 7) XPosition = 1;
            if (YPosition < 1) YPosition = 7;
            else if (YPosition > 7) YPosition = 1;
        }

        /// <summary>
        /// ripristina le variabili al nuovo turno
        /// </summary>
        public void Reset()
        {
            HasMovedTile = false;
            HasCollectedPrize = false;
        }
    }
}
