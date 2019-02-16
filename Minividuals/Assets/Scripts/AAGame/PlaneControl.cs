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

#pragma warning restore 649
        
        private Rigidbody planeRigidBody;

        private void Awake()
        {
            planeRigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                var bomb = Instantiate(bombPrefab, bombBay.transform.position, Quaternion.identity);
                bomb.GetComponent<Rigidbody>().velocity = planeRigidBody.velocity;
                bomb.Color = Color.cyan;
            }
        }

        private void FixedUpdate()
        {
            var planeForwardVector = transform.forward;
            var angle = -Input.GetAxis("Horizontal") * Time.deltaTime * 45;
            var currentTurnAngle = Vector3.SignedAngle(Vector3.up, transform.up, planeForwardVector);
            
            currentTurnAngle += angle;

            if(currentTurnAngle > 90)
            {
                angle -= currentTurnAngle - 90;
            }
            else if(currentTurnAngle < -90)
            {
                angle -= currentTurnAngle + 90;
            }

            transform.Rotate(Vector3.forward, angle);
            transform.Rotate(Vector3.up, Time.deltaTime * -currentTurnAngle / 10);
            transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * 45);

            planeRigidBody.velocity = planeForwardVector * flySpeed;
        }
    }
}