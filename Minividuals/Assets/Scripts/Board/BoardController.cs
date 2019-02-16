using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        public Tiles tiles;
        public PlayerController players;
        public Die die;

        private void Start()
        {
            players.Setup();
            tiles.Setup(players.Players); // TODO: Pass minigame information to tiles.Setup
        }
    }
}
