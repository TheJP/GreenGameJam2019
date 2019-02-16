using UnityEngine;

namespace AAGame
{
    public class PlaneDestroyer
        : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            other.gameObject.GetComponent<PlaneControl>()?.Hit();
        }
    }
}