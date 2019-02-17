using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Tooltip("Renderer used to give the platform a colour tint")]
    public SpriteRenderer colourRenderer;

    public Color Colour { get; private set; }

    public void SetColour(Color colour)
    {
        Colour = colour;
        colourRenderer.color = colour;
    }
}
