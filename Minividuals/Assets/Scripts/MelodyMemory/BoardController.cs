﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    public class BoardController : MonoBehaviour
    {
        
#pragma warning disable 649
      [SerializeField] private Tiles tiles;
      [SerializeField] private StartButtonScript startButton;
#pragma warning restore 649

        private const int minRiddleLength = 3;
        
        
        private int riddleLength;
        
        private void Start()
        {
            tiles.Setup();
            tiles.RiddleSolved += OnRiddleSolved;
            
            startButton.activate();    // is inactive by default   
            startButton.ClickEvent += StartButtonOnClickEvent;

            riddleLength = minRiddleLength;

        }

        private void StartButtonOnClickEvent()
        {
            StartRiddle();
        }

        // Update is called once per frame
//        void Update()
//        {
//        
//        }

        private void tempTest()
        {
            Note c4 = new Note(Octave.O4, NoteName.C);
            Note d4 = new Note(Octave.O4, NoteName.D);
            Note c5 = new Note(Octave.O5, NoteName.C);
            Note d5 = new Note(Octave.O5, NoteName.D);
            
//            Debug.Log($"compared c4 to d4: ${c4.CompareTo(d4)}");
//            Debug.Log($"compared d4 to c4: ${d4.CompareTo(c4)}");
//            
//            Debug.Log($"compared c4 to c5: ${c4.CompareTo(c5)}");
//            Debug.Log($"compared c5 to c4: ${c5.CompareTo(c4)}");
//            
//            Debug.Log($"compared c4 to c4: ${c4.CompareTo(c4)}");
//            
//            Debug.Log($"note c4: fullname is '{c4.FullName}', color is '{c4.Color}', audio is '{c4.AudioSourceName}'");
//            Debug.Log($"note d5: fullname is '{d5.FullName}', color is '{d5.Color}', audio is '{d5.AudioSourceName}'");
            
        }

        private void StartRiddle()
        {
            Riddle riddle = new Riddle(riddleLength, Tiles.tileCount);
            Debug.Log($"Riddle is {riddle}");
            
            tiles.AddAndPlayRiddle(riddle);
            startButton.activate();    // so the player can start a new game if he/she cannot solve it

        }

        private void OnRiddleSolved()
        {
            // restart a game with a longer melody
            riddleLength++;
            StartCoroutine("NextRiddle");
            
        }
        
        IEnumerator NextRiddle() 
        {
            yield return new WaitForSeconds(3.0f);

            StartRiddle();
        }

        
    }    
}

