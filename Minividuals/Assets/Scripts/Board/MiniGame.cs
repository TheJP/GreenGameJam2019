using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Board
{
    [Serializable]
    public class MiniGame
    {
        [Tooltip("Name of the minigame that can be displayed to the player")]
        public string name;

        [Tooltip("Name of the scene to be loaded to start this minigame")]
        public string sceneName;

        [Tooltip("(Optional) icon of the minigame that can be displayed on the tiles of the boardgame")]
        public Sprite icon;

        [Tooltip("Minimal amount of this mini game  per board")]
        public int minPerGame;

        [Tooltip("Maximal amount of this mini game per board")]
        public int maxPerGame;
    }
}
