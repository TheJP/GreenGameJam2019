using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Board;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private RestartButtonScript startButton;
        
        [SerializeField]
        private Countdown countdownPrefab;
        
        [SerializeField]
        private Transform countdownLocation;

        [SerializeField] private Cursor cursor;
        
        [Tooltip("The delay between the start of RoundStarting and RoundPlaying phases")]
        [SerializeField] private float startDelay = .5f; 
        [Tooltip("The delay between the end of RoundPlaying and RoundEnding phases")]
        [SerializeField] private float endDelay = 1f;

#pragma warning restore 649
       
        private BoardController boardController;
        private bool gameFinished;
        private bool restartPressed;      // player wishes to restart the a riddle (of same length)
        private bool riddleSolved;        // player has solved the current riddle 
        private float gameTime;
        private IEnumerator countdownCoroutine;
        
        private int numRiddlesSolved;
        private int riddleLength;
        private WaitForSeconds startWait;         // delay whilst the round starts
        private WaitForSeconds endWait;           // delay whilst the round or game ends
        private WaitForSeconds countdownWait;     // delay during the countdown
        
        
        private void Start()
        {
            // Create the delays so they only have to be made once
            startWait = new WaitForSeconds (startDelay);
            endWait = new WaitForSeconds (endDelay);
            countdownWait = new WaitForSeconds (1);
            
            gameFinished = false;
            Debug.Log($"Game Time is {maxGameTime}");
            
            tiles.Setup();
            tiles.RiddleSolved += OnRiddleSolved;
            // startButton.setActive(false);    // is inactive by default   
            startButton.ClickEvent += RestartButtonOnClickEvent;

            riddleLength = minRiddleLength;
            numRiddlesSolved = 0;

            // assign the player control
            boardController = FindObjectOfType<BoardController>();    
            // if we have no board controller, then the minigame was started standalone
            if (boardController != null)
            {
                cursor.SetPlayer(boardController.players.ActivePlayer);
            }
            
            // when everything has been set up, start the game.
            StartCoroutine (GameLoop ());
        }
        
        // This is called from start and will run each phase of the game one after another
        private IEnumerator GameLoop ()
        {
            Debug.Log("GameLoop: started");
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished
            yield return StartCoroutine (RoundStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (RoundEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if the time is up
            if(maxGameTime - gameTime > 0)
            {
                // if time is not up, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine (GameLoop ());
            }
            Debug.Log("GameLoop: finished");
        }


        private IEnumerator RoundStarting()
        {
            Debug.Log("RoundStarting: started");
            // to start the round, make sure the tiles cannot be clicked and init a new riddle
            DisableControls ();
            yield return StartCoroutine(InitRiddle ());

            Debug.Log("RoundStarting: finished");
            // Wait for the specified length of time until yielding control back to the game loop.
            yield return startWait;
            
        }

        private IEnumerator RoundPlaying()
        {
            Debug.Log("RoundPlaying: started");
            // As soon as the round begins playing let the players control the cursor and tiles
            EnableControls();

            // while riddle is not solved and player does not want to have a new riddle...
            while (!riddleSolved && !restartPressed)
            {
                // ... return on the next frame.
                yield return null;
            }
            Debug.Log("RoundPlaying: finished");
        }

        private IEnumerator RoundEnding()
        {
            Debug.Log("RoundEnding: started");
            // stop tiles from reacting
            DisableControls ();

            if (restartPressed)
            {
                Debug.Log("RoundEnding: because restart pressed");
                // round ended because player wants a new riddle
                restartPressed = false;
            }
            else if (riddleSolved)
            {
                Debug.Log("RoundEnding: because riddle solved");
                // round ended because player has solved the riddle
                // increment the difficulty (riddle length), increment the score
                riddleSolved = false;
                riddleLength++;
                numRiddlesSolved++;                
            }
            else
            {
                Debug.Log($"RoundEnding: NOT restart pressed and NOT riddle solved - why are we here???");
            }

            Debug.Log("RoundEnding: finished");
            // Wait for the specified length of time until yielding control back to the game loop.
            yield return endWait;
        }


        private void RestartButtonOnClickEvent()
        {
            restartPressed = true;
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


        private IEnumerator InitRiddle()
        {
            Riddle riddle = new Riddle(riddleLength, Tiles.tileCount);
            Debug.Log($"InitRiddle: have new melody with length {riddleLength}");
            
            yield return tiles.AddAndPlayRiddle(riddle);
            Debug.Log("InitRiddle: finished play melody");
        }

        private void OnRiddleSolved()
        {
            Debug.Log($"riddle solved! ");
            riddleSolved = true;
        }
        
        
        private IEnumerator StartCountdown()
        {
            for(var i = 10; i > 0; --i)
            {
                var countdown = Instantiate(countdownPrefab, countdownLocation);
                countdown.TextMesh.text = $"{i:00}";
                yield return countdownWait;
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

            }
            gameFinished = true;

        }

        private void EnableControls()
        {
            tiles.EnableControl();
            
            startButton.enabled = true;
            cursor.enabled = true;
            cursor.Show();
        }
        
        private void DisableControls()
        {
            startButton.enabled = false;
            cursor.enabled = false;
            cursor.Hide();
            
            tiles.DisableControl();
        }
        
    }    
}

