using UnityEngine;

namespace AAGame
{
    public class PlaneControl
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The speed the plane is flying forward")]
        private float flySpeed = 10;

        private Rigidbody planeRigidBody;

        private void Awake()
        {
            planeRigidBody = GetComponent<Rigidbody>();
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