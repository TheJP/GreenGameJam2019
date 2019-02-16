using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        public Tiles tiles;

        // TODO: Add players by menu instead of hardcoded
        public IList<Player> Players { get; } = new[] {
            new Player(Color.green, "Player1"),
            new Player(Color.blue, "Player2"),
            new Player(Color.red, "Player3"),
            new Player(Color.yellow, "Player4")
        };

        private void Start() => tiles.Setup(Players);
    }
}
