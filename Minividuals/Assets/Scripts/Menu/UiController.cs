using System.Linq;
using UnityEngine;

// Steam controller button mappings: https://answers.unity.com/questions/1097208/is-it-possible-to-implement-the-steam-controller-i.html
namespace Assets.Scripts.Menu
{
    public class UiController : MonoBehaviour
    {
        private const string AButtonSuffix = "A";
        private const string BButtonSuffix = "B";

        public PlayerSelector[] playerSelectors;

        [Tooltip("Available player input prefixes (for each controller, keyboard, ... player one of those has to be added)")]
        public string[] playerPrefixes;

        [Tooltip("Colours that can be selected by the players")]
        public Color[] playerColours;

        private Color NextFreeColour(int index)
        {
            int i = 0;
            while (playerSelectors.FirstOrDefault(s => s?.Owner?.Colour == playerColours[index]) != null)
            {
                index = (index + 1) % playerColours.Length;
                ++i;
                if (i > 1000)
                {
                    Debug.LogError("Too few colours");
                    return playerColours[0];
                }
            }
            return playerColours[index];
        }

        private void Update()
        {
            foreach (var prefix in playerPrefixes)
            {
                if (Input.GetButtonDown($"{prefix}{AButtonSuffix}"))
                {
                    Debug.Log($"{prefix}{AButtonSuffix}");
                    var selector = playerSelectors.FirstOrDefault(s => s.IsFree);
                    var spot = playerSelectors.FirstOrDefault(s => s?.Owner?.InputPrefix == prefix);
                    if (selector != null && spot == null)
                    {
                        selector.APressed(new Board.Player(NextFreeColour(0), prefix)); // TODO: Give colour
                    }
                    else if (spot != null)
                    {
                        var index = System.Array.IndexOf(playerColours, spot.Owner.Colour);
                        var colour = NextFreeColour(index);
                        spot.BPressed();
                        spot.APressed(new Board.Player(colour, prefix));
                    }

                }
                else if (Input.GetButtonDown($"{prefix}{BButtonSuffix}"))
                {
                    var spot = playerSelectors.FirstOrDefault(s => s?.Owner?.InputPrefix == prefix);
                    if (spot != null) { spot.BPressed(); }
                }
            }
        }
    }
}
