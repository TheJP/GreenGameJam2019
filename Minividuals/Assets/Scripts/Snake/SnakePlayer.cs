using System;
using Assets.Scripts.Board;
using UnityEngine;

namespace Snake
{
    public class SnakePlayer
    {
        private bool isDead;
        
        public Player Player { get; }

        public Color Color => Player.Colour;
        public String InputPrefix => Player.InputPrefix;
        
        public int Score { get; set; }

        public bool IsDead
        {
            get => isDead;
            set
            {
                isDead = value;
                TimeOfDeath = Time.time;
            }
        }
        
        public float TimeOfDeath { get; private set; }

        public SnakePlayer(Player player)
        {
            Player = player;
        }
    }
}