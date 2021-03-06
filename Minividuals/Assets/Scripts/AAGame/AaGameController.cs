using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Board;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

        [SerializeField]
        [Tooltip("The maximum time a game should take before it is aborted in seconds")]
        private int maxGameTime;

        [SerializeField]
        private Countdown countdownPrefab;
        
        [SerializeField]
        private Transform countdownLocation;

#pragma warning restore 649

        private BoardController boardController;
        
        private PlaneControl plane;
        private GunControl[] guns;
        private LandscapeCluster[] clusters;

        private bool gameFinished;
        private float gameTime;
        private IEnumerator countdownCoroutine;

        private void Start()
        {
            Player activePlayer;
            IList<Player> players;
            
            boardController = FindObjectOfType<BoardController>();
            if(!ReferenceEquals(boardController, null))
            {
                players = boardController.players.Players;
                activePlayer = boardController.players.ActivePlayer;
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

                activePlayer = players[0];
            }

            var g = 0;
            guns = new GunControl[players.Count - 1];
            foreach(var player in players)
            {
                if(player == activePlayer)
                {
                    plane = Instantiate(planePrefab, new Vector3(50, 50, -100), Quaternion.identity);
                    plane.Player = player;
                    plane.FlySpeed = 10;
                    plane.MinFence = new Vector3(-50, 0, -130);
                    plane.MaxFence = new Vector3(130, 50, 20);
                }
                else
                {
                    guns[g] = Instantiate(gunPrefab, FindGunPosition(), Quaternion.identity);
                    guns[g].Player = player;
                    ++g;
                }
            }
            
            clusters = new LandscapeCluster[Random.Range(minLandscapeClusters, maxLandscapeClusters + 1)];
            for(var i = 0; i < clusters.Length; ++i)
            {
                var landscapeClusterPrefab = clusterPrefabs[Random.Range(0, clusterPrefabs.Length)];
                clusters[i] = Instantiate(landscapeClusterPrefab, FindClusterPosition(),
                    Quaternion.identity);
            }
        }

        private void Update()
        {
            if(gameFinished)
            {
                return;
            }

            if(countdownCoroutine == null)
            {
                gameTime += Time.deltaTime;

                if(maxGameTime - gameTime <= 10)
                {
                    StartCoroutine(countdownCoroutine = StartCountdown());
                }
            }

            if(plane.IsDead || guns.All(g => g.IsTargetDestroyed))
            {
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
                    var scores = new List<(Player player, int steps)>();

                    int planeScore;
                    if(plane.IsDead)
                    {
                        planeScore = -guns.Length + guns.Count(g => g.IsTargetDestroyed);
                    }
                    else
                    {
                        planeScore = guns.Count(g => g.IsTargetDestroyed);
                        if(planeScore == guns.Length)
                        {
                            ++planeScore;
                        }
                    }
                    
                    scores.Add((plane.Player, planeScore));
                    scores.AddRange(guns.Select(gun =>
                        (gun.Player, (gun.HasHitThePlane ? 5 : 0) + (gun.IsTargetDestroyed ? -1 : 0))));

                    boardController.FinishedMiniGame(scores);
                    gameFinished = true;

                    if(countdownCoroutine != null)
                    {
                        StopCoroutine(countdownCoroutine);
                    }
                }
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

        private IEnumerator StartCountdown()
        {
            for(var i = 10; i > 0; --i)
            {
                var countdown = Instantiate(countdownPrefab, countdownLocation);
                countdown.TextMesh.text = $"{i:00}";
                yield return new WaitForSeconds(1);
            }
            
            plane.Hit();
        }
    }
}