using System.Collections;
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

        /// <summary>
        /// Figure associated with the player.
        /// </summary>
        public PlayerFigure Figure { get; set; }

        private Vector3 GetFigureTarget()
        {
            var tileMiddle = Vector3.right * (Location.transform.localScale.x / 2f);
            return Location.transform.position + tileMiddle;
        }

        public Player(Color colour, string inputPrefix)
        {
            Colour = colour;
            InputPrefix = inputPrefix;
        }

        public void TeleportToPlayerLocation() => Figure.transform.position = GetFigureTarget();

        public IEnumerator MoveToPlayerLocation()
        {
            var start = Time.time;
            var startPosition = Figure.transform.position;
            while (Time.time - start < Figure.figureWalkDuration)
            {
                Figure.transform.position = Vector3.Lerp(startPosition, GetFigureTarget(), (Time.time - start) / Figure.figureWalkDuration);
                yield return null;
            }
        }
    }
}
