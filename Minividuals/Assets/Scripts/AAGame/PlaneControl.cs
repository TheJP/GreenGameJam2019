using UnityEngine;

namespace AAGame
{
    public class PlaneControl
        : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The speed the plane is flying forward")]
        private float flySpeed = 10;

        private float turnAngle;

        private Rigidbody planeRigidBody;

        private void Awake()
        {
            planeRigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var angle = -Input.GetAxis("Horizontal") * Time.deltaTime * 45;
            turnAngle += angle;

            if(turnAngle > 90)
            {
                angle -= turnAngle - 90;
                turnAngle = 90;
            }
            else if(turnAngle < -90)
            {
                angle -= turnAngle + 90;
                turnAngle = -90;
            }

            transform.Rotate(Vector3.forward, angle);
            transform.Rotate(Vector3.up, Time.deltaTime * -turnAngle / 10);
            transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * Time.deltaTime * 45);

            planeRigidBody.velocity = transform.forward * flySpeed;
        }
    }
}