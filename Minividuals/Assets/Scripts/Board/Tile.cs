using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tile : MonoBehaviour
    {
        [Tooltip("Renderer which allows to set the colour of the tile")]
        public MeshRenderer colourRenderer;

        [Tooltip("Clouds that hide the tile")]
        public SpriteRenderer[] clouds;

        public bool IsPlayerHome => Owner != null;

        public Player Owner { get; private set; }

        private void Start()
        {
            foreach(var cloud in clouds)
            {
                cloud.transform.Rotate(Vector3.forward, Random.Range(-15, 15));
                cloud.transform.Translate(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            }
        }

        public void SetPlayerOwner(Player owner)
        {
            Owner = owner;
            colourRenderer.material = new Material(colourRenderer.material) { color = owner.Colour };
            HideClouds();
        }

        public void HideClouds()
        {
            foreach (var cloud in clouds)
            {
                cloud.gameObject.SetActive(false);
            }
        }
    }
}
