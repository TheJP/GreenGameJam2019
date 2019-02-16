using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Board
{

    public class FigureScore : MonoBehaviour
    {
        [Tooltip("Image which is used to colour the figure")]
        public Image colourRenderer;

        [Tooltip("Text that displays the score")]
        public Text score;
    }
}
