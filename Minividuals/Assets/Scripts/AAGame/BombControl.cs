using UnityEngine;

namespace AAGame
{
    public class BombControl
        : MonoBehaviour
    {
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

        private void OnCollisionEnter(Collision other)
        {
            var target = other.gameObject.GetComponent<TargetControl>();
            if(target != null)
            {
                target.Hit();
            }
            
            Destroy(gameObject);
        }
    }
}
