using UnityEngine;

namespace AAGame
{
    public class BombControl
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private GameObject explosionPrefab;
        
#pragma warning restore 649
        
        private MeshRenderer meshRenderer;

        public Color Color
        {
            get => meshRenderer.material.color;
            set => meshRenderer.material.color = value;
        }

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if(transform.position.y < 0)
            {
                Destroy();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var target = other.gameObject.GetComponent<TargetControl>();
            if(target != null)
            {
                target.Hit();
            }

            Destroy();
        }

        private void Destroy()
        {
            Destroy(Instantiate(explosionPrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity), 1.0f);
            Destroy(gameObject);
        }
    }
}
