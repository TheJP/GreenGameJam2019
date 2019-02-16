using Assets.Scripts.Board;
using UnityEngine;

namespace Snake
{
    public class SnakeTail
        : MonoBehaviour
    {
#pragma warning disable 649
        
        [SerializeField]
        private float survivalTime;
        
#pragma warning restore 649
        
        public SnakePlayer Player { get; set; }

        private void Start()
        {
            foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material.color = Player.Color;
            }
        }

        private void Update()
        {
            survivalTime -= Time.deltaTime;
            if(survivalTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}