using Assets.Scripts.Board;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ColourJumper
{
    public class ColourJumperController : MonoBehaviour
    {
        [Tooltip("GameObject that contains all spawns")]
        public Transform spawnParent;

        [Tooltip("Prefab of the colour jumper player")]
        public ColourJumperPlayer blobPrefab;

        [Tooltip("Player parent GameObject")]
        public Transform playerParent;

        [Tooltip("Textbox that allows information to be displayed")]
        public Text displayText;

        [Tooltip("Time during which colours are shuffled")]
        public float colourShuffleTime = 3f;

        [Tooltip("Time that the shuffler waits between multiple shuffles")]
        public float shuffleWait = 0.2f;

        [Tooltip("Time the players get to reach their platform at the start")]
        public float initialRoundTime = 20f;

        [Tooltip("Time by which the round time is reduced each round")]
        public float roundTimeCutoff = 2f;

        [Tooltip("Amount of platforms that get deleted per round")]
        public int platformCancelPerTurn = 3;

        [Tooltip("Descending list of scores that the players get")]
        public int[] playerScores = new[] { 5, 3, -1, -3 };

        /// <summary>Board controller used to interface with the rest of the games. May be null!</summary>
        private BoardController boardController;

        private List<ColourJumperPlayer> blobs;
        private readonly List<(Player player, int diedInRound)> lostOrder = new List<(Player, int)>();

        private Platform[] platforms;
        private int cancelledPlatforms = 0;

        private void Awake() => boardController = FindObjectOfType<BoardController>();

        private IEnumerator Start()
        {
            platforms = FindObjectsOfType<Platform>();

            var players = boardController?.players?.Players ??
                new[] { new Player(Color.green, "Player1_"), new Player(Color.blue, "Player2_") };

            blobs = new List<ColourJumperPlayer>();
            foreach (var player in players)
            {
                // Spawn a blob for each player
                var spawn = spawnParent.GetChild(Random.Range(0, spawnParent.childCount));
                var blob = Instantiate(blobPrefab, spawn.position, Quaternion.identity, playerParent);
                blobs.Add(blob);
                blob.Setup(player);
            }

            yield return GameLoop();
        }

        private bool MissingAColour() =>
            blobs.Any(blob => !platforms.Any(platform => blob.Player.Colour == platform.Colour));

        private void ShufflePlatformColours()
        {
            Debug.Assert(blobs.Count > 0);
            Debug.Assert(cancelledPlatforms < platforms.Length);
            do
            {
                // Choose which platforms are disabled
                bool[] cancelled = new bool[platforms.Length];
                for (int i = 0; i < cancelledPlatforms; ++i)
                {
                    int cancel;
                    do { cancel = Random.Range(0, platforms.Length); } while (cancelled[cancel]);
                    cancelled[cancel] = true;
                }

                // Set colour and disabled state to each platform
                for (int i = 0; i < platforms.Length; ++i)
                {
                    platforms[i].SetPlatformActive(!cancelled[i]);
                    if (!cancelled[i]) { platforms[i].SetColour(blobs[Random.Range(0, blobs.Count)].Player.Colour); }
                }
            } while (MissingAColour());
        }

        private IEnumerator Shuffle()
        {
            var startShuffle = Time.time;
            displayText.text = "Shuffle..";
            while (Time.time - startShuffle < colourShuffleTime)
            {
                ShufflePlatformColours();
                yield return new WaitForSeconds(shuffleWait);
            }
            displayText.text = "GO!";
        }

        private void RemoveLoosers(int round)
        {
            var wrongColour = blobs.Where(blob => blob?.CurrentPlatform?.Colour != blob.Player.Colour || !blob.CurrentPlatform.PlatformActive).ToArray();
            foreach (var blob in wrongColour)
            {
                blobs.Remove(blob);
                lostOrder.Add((blob.Player, round));
                blob.KillPlayer();
            }
        }

        private IEnumerator LetPlayersPlay(float roundTime)
        {
            var start = Time.time;
            while (Time.time - start < roundTime)
            {
                var remaining = roundTime - (Time.time - start);
                if (Time.time - start > 1f)
                {
                    if (remaining < 6f) { displayText.text = ((int)remaining).ToString(); }
                    else { displayText.text = ""; }
                }
                yield return null;
            }
            displayText.text = "";
        }

        private IEnumerable<(Player player, int score)> ScorePlayers() => lostOrder.Reverse<(Player player, int diedInRound)>()
                .Zip(playerScores.Take(lostOrder.Count), (lost, score) => (lost.player, lost.diedInRound, score))
                .GroupBy(lost => lost.diedInRound)
                .SelectMany(group => group.Select(lost => (lost.player, (int)group.Average(g => g.score))));

        private IEnumerator GameLoop()
        {
            var round = 0;
            var roundTime = initialRoundTime;
            while (blobs.Count > 1) // Play until only one blob is left blobbing
            {
                // Shuffle platform colours
                yield return Shuffle();
                cancelledPlatforms = Mathf.Min(platforms.Length - blobs.Count, cancelledPlatforms + platformCancelPerTurn);

                // Let players search colours
                yield return LetPlayersPlay(roundTime);
                roundTime = Mathf.Max(1f, roundTime - roundTimeCutoff);

                // Remove players that lost
                RemoveLoosers(round);
                round += 1;

                yield return new WaitForSeconds(1f);
            }
            var winner = blobs.FirstOrDefault();
            if (winner != null) { lostOrder.Add((winner.Player, int.MaxValue)); }

            boardController?.FinishedMiniGame(ScorePlayers());
        }
    }
}
