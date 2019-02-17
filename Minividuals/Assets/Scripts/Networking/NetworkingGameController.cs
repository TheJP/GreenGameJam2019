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
                    new Player(Color.green, "Joystick1_")
                };
            }
            else
            {
                boardPlayers = boardController.players.Players;
            }

            var startPositions = new (Vector3 pos, Vector3 dir)[]
            {
                (map.TopLeft, Vector3.right),
                (map.BottomRight, Vector3.left),
                (map.BottomLeft, Vector3.right),
                (map.TopRight, Vector3.left)
            };

            players = new NetworkPlayer[4];
            
            for(var i = 0; i < boardPlayers.Count; ++i)
            {
                var player = Instantiate(playerPrefab, startPositions[i].pos, Quaternion.identity);
                player.Player = boardPlayers[i];
                player.NetworkMap = map;
                player.Direction = startPositions[i].dir;
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
            
            if(ReferenceEquals(boardController, null))
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                // TODO Add scores
                boardController.FinishedMiniGame(players.Select(p => (p.Player, 0)));
            }
        }
    }
}