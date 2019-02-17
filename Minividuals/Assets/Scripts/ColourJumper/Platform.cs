using UnityEngine;

namespace Assets.Scripts.ColourJumper
{
    public class Platform : MonoBehaviour
    {
        [Tooltip("Renderer used to give the platform a colour tint")]
        public SpriteRenderer colourRenderer;

        [Tooltip("Sprite that is shown, if the platform is deactivated")]
        public SpriteRenderer xSprite;

        public Color Colour { get; private set; }

        public bool PlatformActive { get; private set; } = true;

        private Color startColor;
        private Color targetColour;
        private float animationStart;
        private float animationDuration;

        public void SetPlatformActive(bool active)
        {
            PlatformActive = active;
            xSprite.gameObject.SetActive(!active);
            startColor = Color.white;
        }

        public void SetColour(Color colour)
        {
            Colour = colour;
            colourRenderer.color = Color.Lerp(colour, Color.black, 0.3f);
            SetPlatformActive(true);
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

        private void Update()
        {
            if (PlatformActive) { return; }
            if (Time.time - animationStart > animationDuration)
            {
                targetColour = Random.ColorHSV(0f, 1f, 0f, 0.5f, 0f, 0.5f);
                animationDuration = Random.Range(4f, 5f);
                animationStart = Time.time;
                startColor = colourRenderer.color;
            }
            colourRenderer.color = Color.Lerp(startColor, targetColour, (Time.time - animationStart) / animationDuration);
        }
    }
}
