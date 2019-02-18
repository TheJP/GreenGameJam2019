using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Board
{
    public class GameInfoScreen : MonoBehaviour
    {
        [Tooltip("Parent GameObject that contains game info")]
        public Transform gameInfoParent;

        [Tooltip("Title displayed at the top of the screen")]
        public Text title;

        [Tooltip("Icon that us displayed at the top of the screen")]
        public Image gameIcon;

        /// <summary>
        /// Set GameInfo to display the given <see cref="MiniGame"/>.
        /// </summary>
        /// <param name="miniGame">MiniGame that should be shown on the info screen.</param>
        public void SetMiniGame(MiniGame miniGame)
        {
            title.text = miniGame.name;
            gameIcon.sprite = miniGame.icon;
        }

        /// <summary>
        /// Remove all scores from scoreboard.
        /// </summary>
        public void Clear()
        {
            for (int i = gameInfoParent.childCount - 1; i >= 0; --i)
            {
                Destroy(gameInfoParent.GetChild(i).gameObject);
            }
        }
    }
}
