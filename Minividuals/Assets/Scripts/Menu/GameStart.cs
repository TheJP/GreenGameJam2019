using Assets.Scripts.Board;
using UnityEngine;
namespace Assets.Scripts.Menu
{
    public class GameStart : MonoBehaviour
    {
        public Player[] Players { get; set; }

        private void Start() => DontDestroyOnLoad(gameObject);
    }
}
