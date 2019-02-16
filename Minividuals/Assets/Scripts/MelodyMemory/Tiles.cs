using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    public class Tiles : MonoBehaviour
    {
        private const int width = 8;
        private const int height = 4;
        public const int tileCount = width * height;

#pragma warning disable 649
        [Tooltip("Prefab for creating tiles")]
        [SerializeField] private ColorSoundTile tilePrefab;

        [Tooltip("GameObject in which tiles are added")]
        [SerializeField] private Transform tilesParent;
#pragma warning restore 649


        /// <summary>
        /// Event that gets fired after a player changed a tile location.
        /// </summary>
//        public event Action<Player> PlayerChangedLocation;
        // TODO add event for clicking a tile? (or add this to Tile object?)

        private readonly ColorSoundTile[] tiles = new ColorSoundTile[tileCount];

        private void Start()
        {
            for (int i = 0; i < tileCount; ++i)
            {
                tiles[i] = Instantiate(tilePrefab, tilesParent);
                tiles[i].name = $"ColorSoundTile{i}";
                tiles[i].tileIndex = i;
            }
            UpdateTilePositions();
        }

//        private void Update()
//        {
//            if (width != Screen.width || height != Screen.height)
//            {
//                UpdateTilePositions();
//            }
//        }

        public void Setup()
        {
            // TODO what to do here?
        }

        private void UpdateTilePositions()
        {
            // tiles are normalized: each has width 1, height 1 (for scaling: scale the parent...)
                            
            // position 0 is width/2 minus half a tile left of the center and height/2 minus half a tile below the center 
            float hOffset = 0.5f - (float)width / 2;
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

        public void UpdateTilesRiddle(Riddle riddle)
        {
            for (int i = 0; i < tileCount; ++i)
            {
                Note note = riddle.GetNoteAtPosition(i);
                if (note != null)
                {
                    Debug.Log($"position {i}: note is {note}");
                    tiles[i].setNote(note);
                }
            }            
        }

    }
    
}
