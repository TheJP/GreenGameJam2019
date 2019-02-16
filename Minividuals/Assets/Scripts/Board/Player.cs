using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Player
    {
        /// <summary>
        /// Colour associated with this player.
        /// </summary>
        public Color Colour { get; }

        /// <summary>
        /// Prefix used to get buttons and axis inputs of this player.
        /// </summary>
        public string InputPrefix { get; }

        /// <summary>
        /// Current location of the player on the board.
        /// </summary>
        public Tile Location { get; set; }

        public Player(Color colour, string inputPrefix)
        {
            Colour = colour;
            InputPrefix = inputPrefix;
        }
    }
}
