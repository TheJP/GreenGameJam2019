using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Steam controller button mappings: https://answers.unity.com/questions/1097208/is-it-possible-to-implement-the-steam-controller-i.html
namespace Assets.Scripts.Menu
{
    public class UiController : MonoBehaviour
    {
        private const string AButtonSuffix = "A";
        private const string BButtonSuffix = "B";
        private const string YButtonSuffix = "Y";
        private const string MainSceneName = "MainScene";

        public PlayerSelector[] playerSelectors;

        [Tooltip("Available player input prefixes (for each controller, keyboard, ... player one of those has to be added)")]
        public string[] playerPrefixes;

        [Tooltip("Colours that can be selected by the players")]
        public Color[] playerColours;

        [Tooltip("Data carrier that is used get data from menu to main scene")]
        public GameStart gameStart;

        public GameObject bHint;
        public GameObject yHint;

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

        private PlayerSelector GetSpot(string playerPrefix) => playerSelectors.FirstOrDefault(s => s?.Owner?.InputPrefix == playerPrefix);

        private void Update()
        {
            foreach (var prefix in playerPrefixes)
            {
                if (Input.GetButtonDown($"{prefix}{InputSuffix.A}"))
                {
                    // First a press to reserve a spot
                    var selector = playerSelectors.FirstOrDefault(s => s.IsFree);
                    var spot = GetSpot(prefix);
                    if (selector != null && spot == null)
                    {
                        selector.APressed(new Board.Player(NextFreeColour(0), prefix));
                    }
                    else if (spot != null)
                    {
                        // Select colour by pressing a after getting a spot
                        var index = System.Array.IndexOf(playerColours, spot.Owner.Colour);
                        var colour = NextFreeColour(index);
                        spot.BPressed();
                        spot.APressed(new Board.Player(colour, prefix));
                    }

                }
                else if (Input.GetButtonDown($"{prefix}{InputSuffix.B}"))
                {
                    // Release the reserved spot of this player
                    var spot = GetSpot(prefix);
                    if (spot != null) { spot.BPressed(); }
                }
                else if (Input.GetButtonDown($"{prefix}{InputSuffix.Y}"))
                {
                    // Start the game if the player has a reserved spot
                    if (GetSpot(prefix) != null)
                    {
                        SceneManager.LoadScene(MainSceneName);
                        gameStart.Players = playerSelectors
                            .Select(s => s.Owner)
                            .Where(player => player != null)
                            .ToArray();
                    }
                }
            }

            // Show or hide hints
            var visible = playerSelectors.Any(s => s.Owner != null);
            bHint.SetActive(visible);
            yHint.SetActive(visible);
        }

        // Find button mappings
        // private void OnGUI()
        // {
        //     Event e = Event.current;
        //     if (e.isKey){ Debug.Log("Detected a keyboard event!" + e.modifiers + " + " + e.keyCode + " "); }
        //     if (e.isMouse){ Debug.Log("Detected a Mouse event!" + e.type); }
        // }
    }
}
