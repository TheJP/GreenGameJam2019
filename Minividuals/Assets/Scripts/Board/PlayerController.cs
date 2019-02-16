using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class PlayerController : MonoBehaviour
    {
        public Tiles tiles;

        [Tooltip("Prefabt used for creating player figures")]
        public PlayerFigure playerFigurePrefab;

        [Tooltip("GameObject in which figures are added")]
        public Transform playerFigureParent;


        private int activePlayer = 0;

        public Player ActivePlayer => Players[activePlayer];

        public IList<Player> Players { get; private set; }

        private void Awake()
        {
            tiles.PlayerChangedLocation += PlayerChangedLocation;
            tiles.TilePositionsUpdated += TilePositionsUpdated;
        }

        public void Setup(Player[] playerArray)
        {
            Players = playerArray;
            foreach (var player in Players)
            {
                var figure = Instantiate(playerFigurePrefab, playerFigureParent);
                player.Figure = figure;
                figure.SetOwner(player);
            }
            activePlayer = Random.Range(0, Players.Count);
        }

        public void NextPlayer() => activePlayer = (activePlayer + 1) % Players.Count;

        private void PlayerChangedLocation(Player player) => player.TeleportToPlayerLocation();

        private void TilePositionsUpdated()
        {
            foreach (var player in Players)
            {
                if (player.Location != null && player.Figure != null) { PlayerChangedLocation(player); }
            }
        }
    }
}
