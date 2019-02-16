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

        // TODO: Add players by menu instead of hardcoded
        public IList<Player> Players { get; } = new[] {
            new Player(Color.green, "Player1"),
            new Player(Color.blue, "Player2"),
            new Player(Color.red, "Player3"),
            new Player(Color.yellow, "Player4")
        };

        private void Awake()
        {
            tiles.PlayerChangedLocation += PlayerChangedLocation;
            tiles.TilePositionsUpdated += TilePositionsUpdated;
        }

        public void Setup()
        {
            foreach(var player in Players)
            {
                var figure = Instantiate(playerFigurePrefab, playerFigureParent);
                player.Figure = figure;
                figure.SetOwner(player);
            }
        }

        private void PlayerChangedLocation(Player player)
        {
            var tileMiddle = Vector3.right * (player.Location.transform.localScale.x / 2f);
            player.Figure.transform.position = player.Location.transform.position + tileMiddle;
        }

        private void TilePositionsUpdated()
        {
            foreach(var player in Players)
            {
                if(player.Location != null && player.Figure != null) { PlayerChangedLocation(player); }
            }
        }
    }
}
