using System.Collections;
using Assets.Scripts.Board;
using UnityEngine;

namespace Snake
{
    public class SnakeHead
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private float speed;

        [SerializeField]
        private SnakeTail snakeTailPrefab;

#pragma warning restore 649

        private Rigidbody snakeRigidBody;

        private Vector3 previousTailPosition;
        
        public SnakePlayer Player { get; set; }
        
        private void Start()
        {
            snakeRigidBody = GetComponent<Rigidbody>();
            
            previousTailPosition = transform.position;
            
            foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material.color = Player.Color;
            }
        }

        private void Update()
        {
            var horizontalSpeedForce = Input.GetAxis(Player.InputPrefix + "Horizontal") * speed;
            var verticalSpeedForce = Input.GetAxis(Player.InputPrefix + "Vertical") * speed;
            
            var speedVector = new Vector3(horizontalSpeedForce, 0, verticalSpeedForce);
            snakeRigidBody.AddForce(speedVector);

            var direction = transform.position - previousTailPosition;
            Debug.DrawRay(transform.position, -direction * 10);
            if(direction.magnitude > 2)
            {
                var position = transform.position + -direction.normalized;
                var tail = Instantiate(snakeTailPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
                tail.Player = Player;
                tail.transform.LookAt(new Vector3(transform.position.x, 0, transform.position.z));
                previousTailPosition = position;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var tail = other.gameObject.GetComponent<SnakeTail>();
            if(ReferenceEquals(tail, null) || ReferenceEquals(tail.Player, Player))
            {
                return;
            }

            Player.IsDead = true;
            ++tail.Player.Score;
            Destroy(gameObject);
        }
    }
}