using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    public class Tiles : MonoBehaviour
    {
        public event Action RiddleSolved;
        
        private const int width = 8;
        private const int height = 4;
        public const int tileCount = width * height;

#pragma warning disable 649
        [Tooltip("Prefab for creating tiles")] [SerializeField]
        private ColorSoundTile tilePrefab;

        [Tooltip("GameObject in which tiles are added")] [SerializeField]
        private Transform tilesParent;
#pragma warning restore 649


        /// <summary>
        /// Event that gets fired after a player changed a tile location.
        /// </summary>
//        public event Action<Player> PlayerChangedLocation;
        // TODO add event for clicking a tile? (or add this to Tile object?)

        private readonly ColorSoundTile[] tiles = new ColorSoundTile[tileCount];

        private List<NoteWithPosition> melody; // to be played when riddle starts

        private Riddle riddle;

        private void Start()
        {            
            for (int i = 0; i < tileCount; ++i)
            {
                tiles[i] = Instantiate(tilePrefab, tilesParent);
                tiles[i].name = $"ColorSoundTile{i}";
                tiles[i].tileIndex = i;

                var index = i;
                tiles[i].TileClickEvent += () => ClickedTile(index);
            }
            
            SetListening(false);

            UpdateTilePositions();
        }


        public void Setup()
        {
            ResetTileColors();
            SetListening(false);
        }

        private void UpdateTilePositions()
        {
            // tiles are normalized: each has width 1, height 1 (for scaling: scale the parent...)

            // position 0 is width/2 minus half a tile left of the center and height/2 minus half a tile below the center 
            float hOffset = 0.5f - (float) width / 2;
            float vOffset = .5f - (float) height / 2;


            for (int i = 0; i < tileCount; ++i)
            {
                int row = i / width;
                int col = i % width;
                //Debug.Log($"Tile {i}: row {row}, col {col}");

                // TODO set position (inspired by Board.Tiles)
                Vector3 newPosition = tilesParent.transform.position;
                newPosition.x = col + hOffset;
                newPosition.y = row + vOffset;
                newPosition.z = 0;
                tiles[i].transform.position = newPosition;
            }

        }

        private void ResetTileColors()
        {
            for (int i = 0; i < tileCount; ++i)
                tiles[i].ResetColor();
        }

        // with false, tiles will not react when clicked 
        private void SetListening(bool listening)
        {
            for (int i = 0; i < tileCount; ++i)
                tiles[i].setListening(listening);
        }
        

        public void AddAndPlayRiddle(Riddle riddle)
        {
            this.riddle = riddle;

            ResetTileColors();
            UpdateTilesFromRiddle(riddle);

            melody = riddle.GetRiddleMelody();
            StartCoroutine("PlayMelody");
        }

        private void UpdateTilesFromRiddle(Riddle riddle)
        {
            for (int i = 0; i < tileCount; i++)
            {
                NoteWithPosition note = riddle.GetNoteAtPosition(i);
                if (note != null)
                {
                    Debug.Log($"position {i}: note is {note}");
                    tiles[i].SetNote(note);                    
                }
            }
        }


        IEnumerator PlayMelody()
        {
            foreach (var noteWithPosition in melody)
            {
                ColorSoundTile tile = tiles[noteWithPosition.Position];
                Debug.Log($"- will blink tile {tile}");
                tile.StartCoroutine("BlinkColor");
                //yield return null;
                yield return new WaitForSeconds(1.0f);
            }

            Debug.Log($"finished blinking");
            SetListening(true);
        }

        private void ClickedTile(int index)
        {
            Debug.Log($"Tiles: heard tile {index}");
            bool gameWon = riddle.hearTile(index);

            if (gameWon)
            {
                SetListening(false);
                ShowRiddleSolved();
                RiddleSolved?.Invoke();
            }
            
        }

        private void ShowRiddleSolved()
        {
            DisplayCheckerboard(Color.black, Color.white);
        }

        private void DisplayCheckerboard(Color col1, Color col2)
        {

            for (int i = 0; i < tileCount; i++)
            {
                int row = i / width;
                int col = i % width;

                int sum = row + col;
                if (sum % 2 == 0)
                    tiles[i].SetColor(col1);
                else
                    tiles[i].SetColor(col2);
            }
        }
        
    }

}
