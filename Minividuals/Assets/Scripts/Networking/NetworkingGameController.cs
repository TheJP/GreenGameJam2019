using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Board;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Networking
{
    public class NetworkingGameController
        : MonoBehaviour
    {
        #pragma warning disable 649

        [SerializeField]
        private NetworkMap mapPrefab;

        [SerializeField]
        private NetworkPlayer playerPrefab;

        [SerializeField]
        private int width = 10;

        [SerializeField]
        private int height = 7;

        [SerializeField]
        [Tooltip("The maximum time a game should take in seconds")]
        private float maxGameTime = 120;
        
        #pragma warning restore 649

        private NetworkMap map;

        private IList<NetworkPlayer> players;

        private float elapsedGameTime;

        private BoardController boardController;
        
        private void Start()
        {
            map = Instantiate(mapPrefab);
            map.Width = width;
            map.Height = height;
            
            boardController = FindObjectOfType<BoardController>();

            IList<Player> boardPlayers;
            if(ReferenceEquals(boardController, null))
            {
                boardPlayers = new[]
                {
                    new Player(Color.green, "Joystick1_"),
                    new Player(Color.red, "Joystick2_"),
                    new Player(Color.cyan, "Player1_"),
                    new Player(Color.blue, "Player2_")
                };
            }
            else
            {
                boardPlayers = boardController.players.Players;
            }

            var startPositions = new (Vector3 pos, Vector3 dir, int x, int y)[]
            {
                (map.TopLeft, Vector3.right, 0, 0),
                (map.BottomRight, Vector3.left, map.Width, map.Height),
                (map.BottomLeft, Vector3.right, 0, map.Height),
                (map.TopRight, Vector3.left, map.Width, 0)
            };

            players = new NetworkPlayer[4];
            
            for(var i = 0; i < boardPlayers.Count; ++i)
            {
                var (pos, dir, x, y) = startPositions[i];
                var player = Instantiate(playerPrefab, pos, Quaternion.identity);
                player.Player = boardPlayers[i];
                player.NetworkMap = map;
                player.Direction = dir;
                player.Location = (x, y);

                players[i] = player;
            }

            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(maxGameTime - 10);

            for(var i = 10; i > 0; --i)
            {
                yield return new WaitForSeconds(1);
            }

            foreach(var player in players)
            {
                player.Stop = true;
            }
            
            var scores = players
                .Select(p => (p.Player, Score: map.CountTilesOwnedBy(p.Player)))
                .OrderByDescending(s => s.Score);

            var maxSteps = 7;
            var steps = scores.Select(s => (s.Player, maxSteps -= 2));
            
            if(ReferenceEquals(boardController, null))
            {
#if UNITY_EDITOR
                foreach(var step in steps)
                {
                    Debug.Log($"Player: {step.Player.InputPrefix} / Score: {step.Item2}");
                }
                
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                boardController.FinishedMiniGame(steps);
            }
        }
    }
}