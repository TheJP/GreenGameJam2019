using UnityEngine;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        public Tiles tiles;

        private void Start()
        {
            tiles.Setup(new[] { new Player(Color.green, ""), new Player(Color.blue, ""), new Player(Color.red, ""), new Player(Color.yellow, "") });
        }
    }
}
