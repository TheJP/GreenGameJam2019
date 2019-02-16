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

        [SerializeField]
        [Tooltip("The possible landscape clusters that should be used")]
        private LandscapeCluster[] clusterPrefabs;

        [SerializeField]
        [Tooltip("The maximum of landscape clusters that will be generated")]
        private int maxLandscapeClusters;

        [SerializeField]
        [Tooltip("The minimum of landscape clusters that will be generated")]
        private int minLandscapeClusters;

#pragma warning restore 649

        private PlaneControl plane;
        private GunControl[] guns;
        private LandscapeCluster[] clusters;

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
            plane = Instantiate(planePrefab, new Vector3(50, 50, -100), Quaternion.identity);
            plane.Player = planePlayer;
            plane.FlySpeed = 10;

            guns = new GunControl[players.Count - 1];
            for(var i = 0; i < players.Count - 1; ++i)
            {
                guns[i] = Instantiate(gunPrefab, FindGunPosition(), Quaternion.identity);
                guns[i].Player = players[i + 1];
            }
            
            clusters = new LandscapeCluster[Random.Range(minLandscapeClusters, maxLandscapeClusters + 1)];
            for(var i = 0; i < clusters.Length; ++i)
            {
                var landscapeClusterPrefab = clusterPrefabs[Random.Range(0, clusterPrefabs.Length)];
                clusters[i] = Instantiate(landscapeClusterPrefab, FindClusterPosition(),
                    Quaternion.identity);
            }
        }

        private Vector3 FindGunPosition()
        {
            for(var i = 0; i < 1000; ++i)
            {
                var x = Random.value * 120 - 20;
                var z = Random.value * 120 - 100;
                var gunPosition = new Vector3(x, 0, z);

                if(!guns.Any(g => g != null && (g.transform.position - gunPosition).magnitude < 50))
                {
                    return gunPosition;
                }
            }

            Debug.Log("Couldn't find valid gun position!");
            return Vector3.zero;
        }
        
        private Vector3 FindClusterPosition()
        {
            for(var i = 0; i < 1000; ++i)
            {
                var x = Random.value * 200 - 100;
                var z = Random.value * 200 - 100;
                var clusterPosition = new Vector3(x, 0, z);

                if(!guns.Any(g => g != null && (g.transform.position - clusterPosition).magnitude < 50)
                    && !clusters.Any(c => c != null && (c.transform.position - clusterPosition).magnitude < 50))
                {
                    return clusterPosition;
                }
            }
            
            Debug.Log("Couldn't find valid cluster position!");
            return Vector3.zero;
        }
    }
}