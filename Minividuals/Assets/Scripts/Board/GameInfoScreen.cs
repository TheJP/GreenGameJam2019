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

        private Sprite defaultIcon;

        private void Start() => defaultIcon = gameIcon.sprite;

        /// <summary>
        /// Set GameInfo to display the given <see cref="MiniGame"/>.
        /// </summary>
        /// <param name="miniGame">MiniGame that should be shown on the info screen.</param>
        public void SetMiniGame(MiniGame miniGame)
        {
            Clear();
            title.text = miniGame.name;
            gameIcon.sprite = miniGame.icon ?? defaultIcon;
            if(miniGame.gameInfoPrefab != null)
            {
                Instantiate(miniGame.gameInfoPrefab, gameInfoParent.transform);
            }
        }

        /// <summary>
        /// Remove all scores from scoreboard.
        /// </summary>
        private void Clear()
        {
            for (int i = gameInfoParent.childCount - 1; i >= 0; --i)
            {
                Destroy(gameInfoParent.GetChild(i).gameObject);
            }
        }
    }
}
