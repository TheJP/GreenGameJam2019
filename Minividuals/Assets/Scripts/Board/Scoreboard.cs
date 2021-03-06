﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Board
{
    public class Scoreboard : MonoBehaviour
    {
        [Tooltip("Prefab that is used to display the player scores")]
        public FigureScore figureScorePrefab;

        [Tooltip("Parent GameObject that contains all scores")]
        public Transform scoreParent;

        [Tooltip("Title displayed at the top of the scoreboard")]
        public Text title;

        public IEnumerator AddScores(IEnumerable<(Player player, int steps)> playerScores)
        {
            List<(FigureScore display, int steps)> scores = new List<(FigureScore display, int steps)>();
            foreach(var (player, steps) in playerScores)
            {
                var scoreEntry = Instantiate(figureScorePrefab, scoreParent);
                scoreEntry.colourRenderer.color = player.Colour;
                scoreEntry.score.color = player.Colour;
                scores.Add((scoreEntry, steps));
            }

            foreach (var (display, steps) in scores) { yield return AddScore(display, steps); }
        }

        private IEnumerator AddScore(FigureScore scoreEntry, int score)
        {
            scoreEntry.Show();
            for (int i = 0; i != score; i += System.Math.Sign(score))
            {
                scoreEntry.score.text = i.ToString();
                yield return new WaitForSeconds(0.2f);
            }
            scoreEntry.score.text = score.ToString();
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
        }

    }
}
