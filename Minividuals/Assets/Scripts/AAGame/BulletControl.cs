using UnityEngine;

namespace AAGame
{
    public class BulletControl
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private float maxLifetime;
        
        [SerializeField]
        private GameObject explosionPrefab;

#pragma warning restore 649

        private float lifetimeLeft;
        
        public GunControl Gun { get; set; }

        private void Awake()
        {
            lifetimeLeft = maxLifetime;
        }

        private void Update()
        {
            lifetimeLeft -= Time.deltaTime;

            if(lifetimeLeft <= 0)
            {
                Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), 1.0f);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var plane = other.gameObject.GetComponent<PlaneControl>();
            if(plane != null)
            {
                Gun.HasHitThePlane = true;
                plane.Hit();
            }

            lifetimeLeft = Mathf.Min(0.2f, lifetimeLeft);
        }
    }
}