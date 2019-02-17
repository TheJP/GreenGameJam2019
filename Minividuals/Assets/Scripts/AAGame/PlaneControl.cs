using Assets.Scripts.Board;
using UnityEngine;

namespace AAGame
{
    public class PlaneControl
        : MonoBehaviour
    {
#pragma warning disable 649
        
        [SerializeField]
        [Tooltip("The speed the plane is flying forward")]
        private float flySpeed = 10;

        [SerializeField]
        [Tooltip("The position where a bomb will spawn")]
        private GameObject bombBay;

        [SerializeField]
        [Tooltip("The bomb prefab")]
        private BombControl bombPrefab;

        [SerializeField]
        [Tooltip("Vertical marker")]
        private GameObject verticalMarker;

#pragma warning restore 649
        
        private Rigidbody planeRigidBody;
        private Player player;
        
        public Vector3 MaxFence { get; set; }
        public Vector3 MinFence { get; set; }

        public Player Player
        {
            get => player;
            set
            {
                player = value;
                UpdateColor(player.Colour);
            }
        }

        public float FlySpeed
        {
            get => flySpeed;
            set => flySpeed = value;
        }
        
        public bool IsDead { get; private set; }

        public void Hit()
        {
            IsDead = true;
            Destroy(gameObject);
        }

        private void UpdateColor(Color color)
        {
            foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material.color = color;
            }
        }

        private void Awake()
        {
            planeRigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(Input.GetButtonDown(player.InputPrefix + "A"))
            {
                var bomb = Instantiate(bombPrefab, bombBay.transform.position, Quaternion.identity);
                bomb.GetComponent<Rigidbody>().velocity = planeRigidBody.velocity;
                bomb.Color = player.Colour;
            }
        }

        private void FixedUpdate()
        {
            var planeTransform = transform;
            var currentPosition = planeTransform.position;

            var translateX = 0.0f;
            if(currentPosition.x < MinFence.x)
            {
                translateX = MaxFence.x - MinFence.x;
            }
            else if(currentPosition.x > MaxFence.x)
            {
                translateX = MinFence.x - MaxFence.x;
            }

            var translateZ = 0.0f;
            if(currentPosition.z < MinFence.z)
            {
                translateZ = MaxFence.z - MinFence.z;
            }
            else if(currentPosition.z > MaxFence.z)
            {
                translateZ = MinFence.z - MaxFence.z;
            }

            var translateY = 0.0f;
            if(currentPosition.y < MinFence.y)
            {
                Hit();
            }
            else if(currentPosition.y > MaxFence.y)
            {
                translateY = MaxFence.y - currentPosition.y;
            }
            
            planeTransform.Translate(translateX, translateY, translateZ, Space.World);
            
            var planeForwardVector = planeTransform.forward;
            var rollAngle = -Input.GetAxis(player.InputPrefix + "Horizontal") * Time.fixedDeltaTime * 45;
            var currentRollAngle = Vector3.SignedAngle(Vector3.up, planeTransform.up, planeForwardVector);
            
            currentRollAngle += rollAngle;

            if(currentRollAngle > 90)
            {
                rollAngle -= currentRollAngle - 90;
            }
            else if(currentRollAngle < -90)
            {
                rollAngle -= currentRollAngle + 90;
            }

            var pitchAngle = Input.GetAxis(player.InputPrefix + "Vertical") * Time.fixedDeltaTime * 45;

            planeTransform.Rotate(Vector3.forward, rollAngle);
            planeTransform.Rotate(Vector3.up, Time.fixedDeltaTime * -currentRollAngle / 10);
            planeTransform.Rotate(Vector3.right, pitchAngle);

            planeRigidBody.velocity = planeForwardVector * flySpeed;
            
            verticalMarker.transform.rotation = Quaternion.identity;
            verticalMarker.transform.position = new Vector3(planeTransform.position.x, currentPosition.y - 100,
                currentPosition.z);
        }
    }
}