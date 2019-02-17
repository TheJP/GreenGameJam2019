using Assets.Scripts.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        private const string SceneName = "MainScene";
        private const string MenuSceneName = "MenuScene";

        public Tiles tiles;
        public PlayerController players;
        public MiniGamesController miniGames;
        public Scoreboard scoreboard;
        public GameOver gameOver;
        public Die die;

        [Tooltip("Board GameObject that shall not die")]
        public GameObject board;

        private bool isBackInMainScene = false;

        private void Awake()
        {
            // Destroy this board if there already exsits one
            if (FindObjectsOfType<BoardController>().Length > 1)
            {
                Destroy(board);
                return;
            }
        }

        private void Start()
        {
            // Keep board for all scenes
            // TODO: Destroy when going back to main menu
            DontDestroyOnLoad(board);

            SceneManager.sceneLoaded += SceneLoaded;

            var gameStart = FindObjectOfType<GameStart>();
            var playersData = gameStart?.Players ?? new[] {
                new Player(Color.green, "Player1_"),
                new Player(Color.blue, "Player2_")
            };
            Destroy(gameStart.gameObject);

            players.Setup(playersData);
            tiles.Setup(players.Players, miniGames.miniGames);
            StartCoroutine(GameLoop());
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneName) { isBackInMainScene = true; }
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                var player = players.ActivePlayer;

                // Roll die
                die.PrepareRoll(player);
                yield return new WaitUntil(() => Input.GetButtonDown($"{player.InputPrefix}{InputSuffix.A}"));
                yield return die.RollCoroutine();

                // Move player
                yield return new WaitForSeconds(1f);
                yield return MovePlayerCoroutine(player, die.DieResult);

                // Reveal tile
                die.HideDie();
                yield return player.Location.HideCloudsCoroutine();

                //Play minigame
                if (player.Location.MiniGame != null)
                {
                    isBackInMainScene = false;
                    SceneManager.LoadScene(player.Location.MiniGame.sceneName);
                    yield return new WaitUntil(() => isBackInMainScene);
                }

                // Check win condition
                if (players.Players.Any(p => p.StepsProgress >= tiles.StepsToWin))
                {
                    yield return GameOver();
                    yield break;
                }

                players.NextPlayer();
            }
        }

        /// <summary>
        /// Game over logic.
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameOver()
        {
            var winner = players.Players.OrderByDescending(p => p.StepsProgress).First();
            gameOver.colourImage.color = winner.Colour;
            gameOver.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(MenuSceneName);
            Destroy(board);
        }

        /// <summary>
        /// Call this method to move the player fluently for the given number of steps.
        /// </summary>
        /// <param name="player">Player to be moved.</param>
        /// <param name="steps">Amount of steps the player should move in positive or negative direction.</param>
        /// <returns></returns>
        public IEnumerator MovePlayerCoroutine(Player player, int steps)
        {
            player.StepsProgress += steps;
            while (steps != 0)
            {
                var target = steps < 0 ? tiles.TileBefore(player.Location) : tiles.TileAfter(player.Location);
                player.Location = target;
                yield return player.MoveToPlayerLocation();
                yield return new WaitForSeconds(0.1f);
                steps -= System.Math.Sign(steps);
            }

            // Move other player on same tile 1 step back
            var playerOnSameTile = players.Players.FirstOrDefault(p => p.Location == player.Location && p != player);
            if (playerOnSameTile != null) { yield return MovePlayerCoroutine(playerOnSameTile, -1); }
        }

        /// <summary>
        /// Should be called by the mini game if the game is done.
        /// Player scores contains pairs of players with their score. If no player scored an empty collection can be passed.
        /// </summary>
        /// <remarks>Example: FinishedMiniGame(new[]{ (player1, 4), (player2, -1) });</remarks>
        /// <param name="playerScores">Scores of the players.</param>
        public void FinishedMiniGame(IEnumerable<(Player player, int steps)> playerScores)
        {
            Debug.Assert(playerScores != null);
            StartCoroutine(ShowScoreBoard(playerScores.OrderByDescending(score => score.steps)));
        }

        private IEnumerator ShowScoreBoard(IEnumerable<(Player player, int steps)> playerScores)
        {
            // Display achieved scores
            scoreboard.Clear();
            scoreboard.title.text = players.ActivePlayer.Location.MiniGame.name;
            scoreboard.gameObject.SetActive(true);
            foreach (var (player, steps) in playerScores)
            {
                yield return scoreboard.AddScore(player, steps);
            }
            yield return new WaitUntil(AnyPlayerClicksA);
            scoreboard.gameObject.SetActive(false);

            // Move tiles according to achieved scores
            foreach (var (player, steps) in playerScores.Where(score => score.steps != 0))
            {
                yield return MovePlayerCoroutine(player, steps);
                yield return player.Location.HideCloudsCoroutine();
            }

            SceneManager.LoadScene(SceneName);
        }

        /// <summary>
        /// Checks if any player pressed A during the current frame.
        /// </summary>
        /// <returns></returns>
        private bool AnyPlayerClicksA()
        {
            foreach (var player in players.Players)
            {
                if (Input.GetButtonDown($"{player.InputPrefix}{InputSuffix.A}")) { return true; }
            }
            return false;
        }
    }
}
