using UnityEngine;
using UnityEngine.UI;

public class PongUi : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Text gameTime;
    [SerializeField] private Text position0Text;
    [SerializeField] private Text position1Text;
    [SerializeField] private Text position2Text;
    [SerializeField] private Text position3Text;
#pragma warning restore 649

    public void DisplayGameTime(string timeToDisplay)
    {
        gameTime.text = timeToDisplay;
    }

    public void DisplayScores(int[] scores)
    {
        position3Text.text = scores.Length >= 4 ? $"Player 4:  {scores[3]}" : "";
        position2Text.text = scores.Length >= 3 ? $"Player 3:  {scores[2]}" : "";
        position1Text.text = scores.Length >= 2 ? $"Player 2:  {scores[1]}" : "";
        position0Text.text = scores.Length >= 1 ? $"Player 1:  {scores[0]}" : "";
    }
}