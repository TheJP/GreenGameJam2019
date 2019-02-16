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
            var planeForwardVector = transform.forward;
            var angle = -Input.GetAxis(player.InputPrefix + "Horizontal") * Time.deltaTime * 45;
            var currentRollAngle = Vector3.SignedAngle(Vector3.up, transform.up, planeForwardVector);
//            var currentPitchAngle = Vector3.SignedAngle(Vector3.forward, planeForwardVector, transform.right);
            
            currentRollAngle += angle;

            if(currentRollAngle > 90)
            {
                angle -= currentRollAngle - 90;
            }
            else if(currentRollAngle < -90)
            {
                angle -= currentRollAngle + 90;
            }
            
//            verticalMarker.transform.RotateAround(transform.position, planeForwardVector, -angle);

            var pitchAngle = Input.GetAxis(player.InputPrefix + "Vertical") * Time.deltaTime * 45;

            transform.Rotate(Vector3.forward, angle);
            transform.Rotate(Vector3.up, Time.deltaTime * -currentRollAngle / 10);
            transform.Rotate(Vector3.right, pitchAngle);

            planeRigidBody.velocity = planeForwardVector * flySpeed;
            
            verticalMarker.transform.rotation = Quaternion.identity;
            verticalMarker.transform.position = new Vector3(transform.position.x, transform.position.y - 100, transform.position.z);
        }
    }
}