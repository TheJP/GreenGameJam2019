using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class BoardController : MonoBehaviour
    {
        public Tiles tiles;
        public PlayerController players;

        private void Start() => tiles.Setup(players.Players); // TODO: Pass minigame information to tiles.Setup
    }
}
