using System.Collections.Generic;
using Assets.Scripts.Board;
using UnityEngine;

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
        
        #pragma warning restore 649

        private NetworkMap map;

        private IList<NetworkPlayer> players;

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
        }
    }
}