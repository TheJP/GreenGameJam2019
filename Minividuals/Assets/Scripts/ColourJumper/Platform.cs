using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ColourJumper
{
    public class Platform : MonoBehaviour
    {
        [Tooltip("Renderer used to give the platform a colour tint")]
        public SpriteRenderer colourRenderer;

        public Color Colour { get; private set; }

        public void SetColour(Color colour)
        {
            Colour = colour;
            colourRenderer.color = Color.Lerp(colour, Color.black, 0.3f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<ColourJumperPlayer>();
            if (player != null) { player.CurrentPlatform = this; }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var player = collision.GetComponent<ColourJumperPlayer>();
            if (player != null) { player.CurrentPlatform = null; }
        }
    }
}
