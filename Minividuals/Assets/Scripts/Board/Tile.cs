using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Tile : MonoBehaviour
    {
        [Tooltip("Renderer which allows to set the colour of the tile")]
        public MeshRenderer colourRenderer;

        public void SetColour(Color colour)
        {
            colourRenderer.material = new Material(colourRenderer.material) { color = colour };
        }
    }
}
