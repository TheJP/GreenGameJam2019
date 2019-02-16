using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tile : MonoBehaviour
    {
        [Tooltip("Renderer which allows to set the colour of the tile")]
        public MeshRenderer colourRenderer;

        [Tooltip("Clouds that hide the tile")]
        public SpriteRenderer[] clouds;

        [Tooltip("Time in seconds until clouds disappear")]
        public float cloudFadeOutDuration = 3f;

        [Tooltip("Distance that the clouds are moved while fading out")]
        public float cloudFadeOutDistance = 0.5f;

        public bool IsPlayerHome => Owner != null;

        public Player Owner { get; private set; }

        public int TileIndex { get; set; }

        private void Start()
        {
            // Add random rotation and translation to clouds, so that they don't look that uniform
            foreach (var cloud in clouds)
            {
                cloud.transform.Rotate(Vector3.forward, Random.Range(-15, 15));
                cloud.transform.Translate(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            }
        }

        public void SetPlayerOwner(Player owner)
        {
            Owner = owner;
            colourRenderer.material = new Material(colourRenderer.material) { color = owner.Colour };
            StartCoroutine(HideCloudsCoroutine());
        }

        public IEnumerator HideCloudsCoroutine()
        {
            if (clouds.Length <= 0) { yield break; }

            // Move clouds away from center and fade them out at the same time
            var middle = clouds.Aggregate(Vector3.zero, (sum, cloud) => sum + cloud.transform.position) / clouds.Length;
            var start = Time.time;
            while (Time.time - start < cloudFadeOutDuration)
            {
                var distance = cloudFadeOutDistance / cloudFadeOutDuration * Time.deltaTime;
                foreach (var cloud in clouds)
                {
                    cloud.transform.position += (cloud.transform.position - middle).normalized * distance;
                    cloud.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (Time.time - start) / cloudFadeOutDuration);
                }
                yield return null; // Wait for next frame (next Update)
            }
            
            // Deactivate invisible clouds
            foreach (var cloud in clouds)
            {
                cloud.gameObject.SetActive(false);
            }
        }
    }
}
