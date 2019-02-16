using System;
using Assets.Scripts.Board;
using UnityEngine;

namespace Snake
{
    public class SnakePlayer
    {
        public Player Player { get; }

        public Color Color => Player.Colour;
        public String InputPrefix => Player.InputPrefix;
        
        public int Score { get; set; }
        
        public bool IsDead { get; set; }

        public SnakePlayer(Player player)
        {
            Player = player;
        }
    }
}