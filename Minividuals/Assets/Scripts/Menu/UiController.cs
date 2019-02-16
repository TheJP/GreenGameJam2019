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

        private void Update()
        {
            foreach (var prefix in playerPrefixes)
            {
                if (Input.GetButtonDown($"{prefix}{AButtonSuffix}"))
                {
                    var selector = playerSelectors.FirstOrDefault(s => s.IsFree);
                    var hasNoSpotYet = playerSelectors.FirstOrDefault(s => s?.Owner?.InputPrefix == prefix) == null;
                    if (selector != null && hasNoSpotYet)
                    {
                        selector.APressed(new Board.Player(Color.green, prefix)); // TODO: Give colour
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
