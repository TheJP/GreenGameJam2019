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
        
        [SerializeField]
        [Tooltip("The amount of seconds the tail survival time should increase per seconds")]
        private float tailSurvivalTimeGain;
        
        [SerializeField]
        [Tooltip("The amount of seconds to wait before the tail gain will start to take effect")]
        private float tailSurvivalTimeGainWait;

#pragma warning restore 649

        private Rigidbody snakeRigidBody;

        private Vector3 previousTailPosition;

        private float elapsedTime;
        
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
            elapsedTime += Time.deltaTime;
            
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

                if(elapsedTime >= tailSurvivalTimeGainWait)
                {
                    tail.SurvivalTime += (elapsedTime - tailSurvivalTimeGainWait) * tailSurvivalTimeGain;
                }
                
                previousTailPosition = position;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var tail = other.gameObject.GetComponent<SnakeTail>();
            if(!ReferenceEquals(tail, null))
            {
                if(ReferenceEquals(tail.Player, Player))
                {
                    --Player.Score;
                }
                else
                {
                    ++tail.Player.Score;
                }
            }
            else if(!ReferenceEquals(other.gameObject.GetComponent<TheEvilBorder>(), null))
            {
                --Player.Score;
            }
            else
            {
                return;
            }
            
            Player.IsDead = true;
            Destroy(gameObject);
        }
    }
}