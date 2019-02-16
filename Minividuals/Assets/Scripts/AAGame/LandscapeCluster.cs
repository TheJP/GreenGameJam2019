using UnityEngine;

namespace AAGame
{
    public class LandscapeCluster
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        [Tooltip("The prefab elements this cluster will be composed of")]
        private Transform[] possiblePrefabElements;

        [SerializeField]
        private int maxElements;

        [SerializeField]
        private int minElements;
        
#pragma warning restore 649

        private void Awake()
        {
            var elementCount = Random.Range(minElements, maxElements + 1);
            for(var i = 0; i < elementCount; ++i)
            {
                var position = transform.position + new Vector3(Random.value * 20 - 10, Random.value * -20, Random.value * 20 - 10);
//                position = transform.TransformVector(position);
                Instantiate(possiblePrefabElements[Random.Range(0, possiblePrefabElements.Length)], position,
                    Quaternion.AngleAxis(Random.value * 90.0f, Vector3.up), transform);
            }
        }
    }
}