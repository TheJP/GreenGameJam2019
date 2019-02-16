using UnityEngine;

/// <summary>
/// Responsible for controlling the look of a player.
/// </summary>
public class PlayerStyle : MonoBehaviour
{
    private Color playerColor;

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = playerColor;
    }

    public void SetColor(Color color)
    {
        playerColor = color;
    }
}