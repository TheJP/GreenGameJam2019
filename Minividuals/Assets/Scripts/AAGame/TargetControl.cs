using UnityEngine;

namespace AAGame
{
    public class TargetControl
        : MonoBehaviour
    {
        public bool IsDestroyed { get; private set; }
        public void Hit()
        {
            IsDestroyed = true;
            Destroy(gameObject);
        }
    }
}
