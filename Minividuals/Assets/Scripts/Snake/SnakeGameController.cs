using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Board;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snake
{
    public class SnakeGameController
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        [Tooltip("The starting positions for the snake game. Make sure there are enough!")]
        private Transform[] startingPositions;

        [SerializeField]
        private SnakeHead snakeHeadPrefab;
        
#pragma warning restore 649
        
        private BoardController boardController;
        private List<SnakePlayer> snakePlayers;

        private bool gameFinished;
        
        private void Start()
        {
            snakePlayers = new List<SnakePlayer>();
            boardController = FindObjectOfType<BoardController>();

            IList<Player> players;
            if(ReferenceEquals(boardController, null))
            {
                players = new[]
                {
                    new Player(Color.green, "Joystick1_"),
                    new Player(Color.red, "Joystick2_"),
                    new Player(Color.cyan, "Player1_"),
                    new Player(Color.magenta, "Player2_")
                };
            }
            else
            {
                players = boardController.players.Players;
            }

            for(var i = 0; i < players.Count; ++i)
            {
                var snakeHead = Instantiate(snakeHeadPrefab, startingPositions[i].position, Quaternion.identity);
                snakePlayers.Add(snakeHead.Player = new SnakePlayer(players[i]));
            }
        }

        private void Update()
        {
            if(gameFinished || snakePlayers.Count(p => !p.IsDead) > 1)
            {
                return;
            }
            
            var scoreGain = 0;
            foreach(var snakePlayer in snakePlayers.OrderBy(p => p.TimeOfDeath))
            {
                var gain = scoreGain++;
                snakePlayer.Score += gain;
            }

            if(!ReferenceEquals(boardController, null))
            {
                boardController.FinishedMiniGame(snakePlayers.Select(s => (s.Player, s.Score)));
            }
            else
            {
#if UNITY_EDITOR
                foreach(var snakePlayer in snakePlayers)
                {
                    Debug.Log($"P: {snakePlayer.InputPrefix} / S: {snakePlayer.Score}");
                }
                
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            gameFinished = true;
        }
    }
}