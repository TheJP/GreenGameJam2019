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
                new Player(Color.green, "Player1"),
                new Player(Color.blue, "Player2")
            };

            players.Setup(playersData);
            tiles.Setup(players.Players); // TODO: Pass minigame information to tiles.Setup
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
                die.PrepareRoll(players.ActivePlayer);
                do
                {
                    yield return null;
                } while (!Input.GetMouseButtonUp(0)); // TODO: Player Input
                yield return die.RollCoroutine();

                yield return new WaitForSeconds(1f);
                yield return MovePlayerCoroutine(players.ActivePlayer, die.DieResult);

                die.HideDie();

                yield return players.ActivePlayer.Location.HideCloudsCoroutine();

                // TODO: Start minigame
                isBackInMainScene = false;
                // SceneManager.LoadScene("TestScene");
                yield return new WaitUntil(() => isBackInMainScene);
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
