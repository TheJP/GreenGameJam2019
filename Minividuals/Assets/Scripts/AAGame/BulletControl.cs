using UnityEngine;

namespace AAGame
{
    public class BulletControl
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private float maxLifetime;

#pragma warning restore 649

        private float lifetimeLeft;

        private void Awake()
        {
            lifetimeLeft = maxLifetime;
        }

        private void Update()
        {
            lifetimeLeft -= Time.deltaTime;

            if(lifetimeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var plane = other.gameObject.GetComponent<PlaneControl>();
            if(plane != null)
            {
                plane.Hit();
            }

            lifetimeLeft = Mathf.Min(0.2f, lifetimeLeft);
        }
    }
}