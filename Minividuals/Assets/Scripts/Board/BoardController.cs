using Assets.Scripts.Menu;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        private const string SceneName = "MainScene";

        public Tiles tiles;
        public PlayerController players;
        public MiniGamesController miniGames;
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

                die.PrepareRoll(player);
                yield return new WaitUntil(() => Input.GetButtonDown($"{player.InputPrefix}{InputSuffix.A}"));
                yield return die.RollCoroutine();

                yield return new WaitForSeconds(1f);
                yield return MovePlayerCoroutine(player, die.DieResult);

                die.HideDie();

                yield return player.Location.HideCloudsCoroutine();

                if (player.Location.MiniGame != null)
                {
                    isBackInMainScene = false;
                    SceneManager.LoadScene(player.Location.MiniGame.sceneName);
                    yield return new WaitUntil(() => isBackInMainScene);
                }
                yield return new WaitForSeconds(1f);

                players.NextPlayer();
            }
        }

        /// <summary>
        /// Call this method to move the player fluently for the given number of steps.
        /// </summary>
        /// <param name="player">Player to be moved.</param>
        /// <param name="steps">Amount of steps the player should move in positive or negative direction.</param>
        /// <returns></returns>
        public IEnumerator MovePlayerCoroutine(Player player, int steps)
        {
            while (steps != 0)
            {
                var target = steps < 0 ? tiles.TileBefore(player.Location) : tiles.TileAfter(player.Location);
                player.Location = target;
                yield return player.MoveToPlayerLocation();
                yield return new WaitForSeconds(0.1f);
                steps -= System.Math.Sign(steps);
            }
        }
    }
}
