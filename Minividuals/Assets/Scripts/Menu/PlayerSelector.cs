using Assets.Scripts.Board;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class PlayerSelector : MonoBehaviour
    {
        [Tooltip("The colour of the given image is changed to fit the player colour")]
        public Image colourImage;

        public GameObject aButton;
        public GameObject figure;

        public Player Owner { get; private set; }
        public bool IsFree => Owner == null;

        public void APressed(Player player)
        {
            Owner = player;
            colourImage.color = player.Colour;
            aButton.SetActive(false);
            figure.SetActive(true);
        }

        public void BPressed()
        {
            Owner = null;
            aButton.SetActive(true);
            figure.SetActive(false);
        }
    }
}
