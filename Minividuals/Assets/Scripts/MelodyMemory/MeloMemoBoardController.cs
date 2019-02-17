using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Board;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MelodyMemory
{
    public class MeloMemoBoardController : MonoBehaviour
    {
        
#pragma warning disable 649
        [Tooltip("The maximum time a game should take before it is aborted in seconds")]
        [SerializeField] private int maxGameTime;
        
        [Tooltip("The minimum length for a melody")]
        [SerializeField] private int minRiddleLength;

        [SerializeField] private Tiles tiles;
        [SerializeField] private StartButtonScript startButton;
        
        [SerializeField]
        private Countdown countdownPrefab;
        
        [SerializeField]
        private Transform countdownLocation;

#pragma warning restore 649
       
        private BoardController boardController;
        private bool gameFinished;
        private float gameTime;
        private IEnumerator countdownCoroutine;
        
        private int numRiddlesSolved;
        private int riddleLength;
        
        private void Start()
        {
            gameFinished = false;
            Debug.Log($"Game Time is {maxGameTime}");
            
            tiles.Setup();
            tiles.RiddleSolved += OnRiddleSolved;
            
            startButton.setActive(false);    // is inactive by default   
            startButton.ClickEvent += StartButtonOnClickEvent;

            riddleLength = minRiddleLength;
            numRiddlesSolved = 0;

            boardController = FindObjectOfType<BoardController>();    
            // if we have no board controller, then the minigame was started standalone

            StartRiddle();
        }
        
        private void StartButtonOnClickEvent()
        {
            StartRiddle();
        }

        private void Update()
        {
            if(gameFinished)
            {
                Debug.Log("Game finished");
                return;
            }

            if(countdownCoroutine == null)
            {
                gameTime += Time.deltaTime;
                if(maxGameTime - gameTime <= 10)
                {
                    Debug.Log("Starting countdown");
                    StartCoroutine(countdownCoroutine = StartCountdown());
                }
            }

        }


        private void StartRiddle()
        {
            startButton.setActive(false);
            Riddle riddle = new Riddle(riddleLength, Tiles.tileCount);
            Debug.Log($"have new melody with length {riddleLength}");
            
            tiles.AddAndPlayRiddle(riddle);
            startButton.setActive(true);    // so the player can start a new game if he/she cannot solve it

        }

        private void OnRiddleSolved()
        {
            // restart a game with a longer melody
            riddleLength++;
            numRiddlesSolved++;
            StartCoroutine("NextRiddle");
        }
        
        IEnumerator NextRiddle() 
        {
            yield return new WaitForSeconds(3.0f);
            StartRiddle();
        }

        private IEnumerator StartCountdown()
        {
            for(var i = 10; i > 0; --i)
            {
                var countdown = Instantiate(countdownPrefab, countdownLocation);
                countdown.TextMesh.text = $"{i:00}";
                yield return new WaitForSeconds(1);
            }

            // finish game
            
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
                scores.Add((boardController.players.ActivePlayer, numRiddlesSolved));
                boardController.FinishedMiniGame(scores);
                gameFinished = true;

            }
        }
        
    }    
}

