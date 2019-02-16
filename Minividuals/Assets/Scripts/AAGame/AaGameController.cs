using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Board;
using UnityEngine;

namespace AAGame
{
    public class AaGameController
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        [Tooltip("The plane prefab")]
        private PlaneControl planePrefab;

        [SerializeField]
        [Tooltip("The AA gun prefab")]
        private GunControl gunPrefab;

#pragma warning restore 649

        private PlaneControl plane;
        private GunControl[] guns;

        private void Start()
        {
            var boardController = FindObjectOfType<BoardController>();
            IList<Player> players;
            if(boardController != null)
            {
                players = boardController.players.Players;
            }
            else
            {
                players = new List<Player>
                {
                    new Player(Color.green, "Joystick1_"),
                    new Player(Color.red, "Joystick2_"),
                    new Player(Color.cyan, "Player1_"),
                    new Player(Color.magenta, "Player2_")
                };
            }

            var planePlayer = players[0];
            plane = Instantiate(planePrefab, new Vector3(0, 50, 0), Quaternion.identity);
            plane.Player = planePlayer;
            plane.FlySpeed = 10;

            guns = new GunControl[players.Count - 1];
            for(var i = 0; i < players.Count - 1; ++i)
            {
                guns[i] = Instantiate(gunPrefab, FindGunPosition(), Quaternion.identity);
                guns[i].Player = players[i + 1];
            }
        }

        private Vector3 FindGunPosition()
        {
            for(;;)
            {
                var x = Random.value * 100 - 50;
                var z = Random.value * 100 - 50;
                var gunPosition = new Vector3(x, 0, z);

                if(!guns.Any(g => g != null && (g.transform.position - gunPosition).magnitude < 50))
                {
                    return gunPosition;
                }
            }
        }
    }
}