using UnityEngine;

namespace AAGame
{
    public class TargetControl
        : MonoBehaviour
    {
        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}
