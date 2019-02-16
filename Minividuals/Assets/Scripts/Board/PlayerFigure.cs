using UnityEngine;

namespace Assets.Scripts.Board
{
    public class PlayerFigure : MonoBehaviour
    {
        private const float DarkenBy = 0.2f;

        [Tooltip("Renderer which allows to set the colour of the figure")]
        public SpriteRenderer colourRenderer;

        [Tooltip("Time that the figure needs to walk from one tile to the next")]
        public float figureWalkDuration = 2f;

        public Player Owner { get; private set; }

        public void SetOwner(Player owner)
        {
            Owner = owner;
            colourRenderer.color = Color.Lerp(owner.Colour, Color.black, DarkenBy);
        }
    }
}
