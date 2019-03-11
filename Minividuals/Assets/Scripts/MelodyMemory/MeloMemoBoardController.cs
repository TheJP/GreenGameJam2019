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
        [SerializeField] private RestartButtonScript restartButton;
        
        [SerializeField]
        private Countdown countdownPrefab;
        
        [SerializeField]
        private Transform countdownLocation;

        [SerializeField] private Cursor cursor;
        
#pragma warning restore 649
       
        // The delay between the start of RoundStarting and RoundPlaying phases
        private float startDelay = 1.0f; 
        // The delay between the end of RoundPlaying and RoundEnding phases (allow enough time to play "riddle solved animation")
        private float endDelay = 2.0f;

        private BoardController boardController;
        private bool gameFinished;        // game finished because time is up
        private bool restartPressed;      // player wishes to restart the a riddle (of same length)
        private bool riddleSolved;        // player has solved the current riddle 
        private float gameTime;           // time the game is running (counted in Update)
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
            Debug.Log($"BoardController.Start: cursor is {cursor}");
            restartButton.Setup();
            restartButton.ClickEvent += RestartButtonOnClickEvent;

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
            
            // Wait for the specified length of time (here 0) until yielding control back to the game loop.
            yield return null;
            
        }

        private IEnumerator RoundPlaying()
        {
            Debug.Log("RoundPlaying: started");
            // As soon as the round begins playing let the players control the cursor and tiles
            EnableControls();

            // while riddle is not solved and player does not want to have a new riddle...
            while (!riddleSolved && !restartPressed && !gameFinished)
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
                // round ended because player wants a new riddle
                Debug.Log("RoundEnding: because restart pressed");
                tiles.SetTileColors(Color.black);
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
            else if (gameFinished)
            {
                tiles.SetTileColors(Color.white);
                DisableControls ();
            }
            else
            {
                Debug.Log($"RoundEnding: NOT restart pressed, NOT riddle solved, NOT game finished - why are we here???");
            }

            Debug.Log("RoundEnding: finished - wait for specified time (endWait)");
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
            yield return tiles.AddAndPlayRiddle(riddle);
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
            restartButton.enabled = true;
            restartButton.Show();
            cursor.Show();
            cursor.enabled = true;
            tiles.EnableControl();
        }
        
        private void DisableControls()
        {
            restartButton.enabled = false;
            restartButton.Hide();
            cursor.Hide();
            cursor.enabled = false;            
            tiles.DisableControl();
        }
        
    }    
}

