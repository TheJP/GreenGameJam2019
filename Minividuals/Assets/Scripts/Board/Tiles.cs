﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tiles : MonoBehaviour
    {
        private const int Width = 15;
        private const int Height = 9;
        private const int TileCount = 2 * (Width + Height);

        public int StepsToWin => TileCount;

        [Tooltip("Camera that shows the board")]
        public Camera boardCamera;

        [Tooltip("Prefab for creating tiles")]
        public Tile tilePrefab;

        [Tooltip("GameObject in which tiles are added")]
        public Transform tilesParent;

        /// <summary>
        /// Event that gets fired after a player changed a tile location.
        /// </summary>
        public event System.Action<Player> PlayerChangedLocation;

        /// <summary>
        /// Event that gets fired afer the tile positions update.
        /// </summary>
        public event System.Action TilePositionsUpdated;

        private readonly Tile[] tiles = new Tile[TileCount];

        private int width;
        private int height;

        private void Update()
        {
            if (width != Screen.width || height != Screen.height)
            {
                UpdateTilePositions();
            }
        }

        public void Setup(IList<Player> players, IList<MiniGame> miniGames)
        {
            // Create tiles
            for (int i = 0; i < TileCount; ++i)
            {
                tiles[i] = Instantiate(tilePrefab, tilesParent);
                tiles[i].TileIndex = i;
                tiles[i].name = $"BoardTile{i}";
            }
            UpdateTilePositions();

            // Setup players
            var spacing = TileCount / players.Count;
            for(int i = 0; i < players.Count; ++i)
            {
                int index = i * spacing;
                tiles[index].SetPlayerOwner(players[i]);
                players[i].Location = tiles[index];
                PlayerChangedLocation?.Invoke(players[i]);
            }

            // Add minigames to tiles
            var freeTiles = tiles.Where(tile => tile.Owner == null).ToList();
            foreach(var miniGame in miniGames)
            {
                var amount = Random.Range(miniGame.minPerGame, miniGame.maxPerGame + 1);
                for(int i = 0; i < amount && freeTiles.Count > 0; ++i)
                {
                    var index = Random.Range(0, freeTiles.Count);
                    var tile = freeTiles[index];
                    freeTiles.RemoveAt(index);
                    tile.SetMiniGame(miniGame); //TODO: Improve tile setup (i.e. reveal)
                }
            }
        }

        private void UpdateTilePositions()
        {
            width = Screen.width;
            height = Screen.height;

            // Height and width of screen in world units
            var heightUnits = boardCamera.orthographicSize * 2.0f;
            var widthUnits = heightUnits * boardCamera.aspect;

            // Screen width and world unit width of tiles
            var tileWidth = (float)boardCamera.pixelWidth / (Width + 1);
            var tileWidthUnits = widthUnits / (Width + 1);

            void SetPosition(int i, Vector3 screenPosition, int yOffset)
            {
                var position = boardCamera.ScreenToWorldPoint(screenPosition);
                position.z = 0;
                position.y -= yOffset;
                tiles[i].transform.position = position;
                tiles[i].transform.localScale = new Vector3(tileWidthUnits, 1, 1);
            }

            for (int i = 0; i < TileCount; ++i)
            {
                if (i < Width) // Top row
                {
                    SetPosition(i, new Vector3(i * tileWidth, boardCamera.pixelHeight, 0), 0);
                }
                else if (i >= Width && i < TileCount / 2) // Right column
                {
                    SetPosition(i, new Vector3(Width * tileWidth, boardCamera.pixelHeight, 0), i - Width);
                }
                else if (i >= TileCount / 2 && i < TileCount - Height) // Bottom row
                {
                    SetPosition(i, new Vector3(((TileCount - Height) - i) * tileWidth, 0, 0), -1);
                }
                else // Left column
                {
                    SetPosition(i, new Vector3(0, boardCamera.pixelHeight, 0), TileCount - i);
                }
            }

            TilePositionsUpdated?.Invoke();
        }

        public Tile TileAfter(Tile tile) => tiles[(tile.TileIndex + 1) % TileCount];

        public Tile TileBefore(Tile tile) => tiles[(tile.TileIndex - 1 + TileCount) % TileCount];
    }
}
