using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Board
{
    public class Scoreboard : MonoBehaviour
    {
        [Tooltip("Prefab that is used to display the player scores")]
        public FigureScore figureScorePrefab;

        [Tooltip("Vertical space between the figure scores")]
        public float verticalSpacing = 100f;

        [Tooltip("Parent GameObject that contains all scores")]
        public Transform scoreParent;

        [Tooltip("Title displayed at the top of the scoreboard")]
        public Text title;

        private int numberOfScores = 0;

        public IEnumerator AddScore(Player player, int score)
        {
            var scoreEntry = Instantiate(figureScorePrefab, scoreParent);
            scoreEntry.GetComponent<RectTransform>().position += Vector3.down * (numberOfScores * verticalSpacing);
            scoreEntry.colourRenderer.color = player.Colour;
            scoreEntry.score.color = player.Colour;
            for (int i = 0; i != score; i += System.Math.Sign(score))
            {
                scoreEntry.score.text = i.ToString();
                yield return new WaitForSeconds(0.2f);
            }
            scoreEntry.score.text = score.ToString();
            numberOfScores += 1;
        }

        /// <summary>
        /// Remove all scores from scoreboard.
        /// </summary>
        public void Clear()
        {
            for(int i = scoreParent.childCount - 1; i >= 0; --i)
            {
                Destroy(scoreParent.GetChild(i).gameObject);
            }
            numberOfScores = 0;
        }

    }
}
