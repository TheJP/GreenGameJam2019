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

        private IList<Player> players;
        
        private void Start()
        {
            map = Instantiate(mapPrefab);
            map.Width = width;
            map.Height = height;

            players = new[]
            {
                new Player(Color.green, "Joystick1_")
            };

            var player = Instantiate(playerPrefab, map.TopLeft, Quaternion.identity);
            player.Player = players[0];
            player.NetworkMap = map;
            player.Direction = Vector3.right;
        }
    }
}