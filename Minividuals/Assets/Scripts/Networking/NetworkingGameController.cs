using UnityEngine;

namespace Networking
{
    public class NetworkingGameController
        : MonoBehaviour
    {
        #pragma warning disable 649

        [SerializeField]
        private NetworkMap mapPrefab;

        [SerializeField]
        private int width = 10;

        [SerializeField]
        private int height = 7;
        
        #pragma warning restore 649

        private NetworkMap map;
        
        private void Start()
        {
            map = Instantiate(mapPrefab);
            map.Width = width;
            map.Height = height;
        }
    }
}