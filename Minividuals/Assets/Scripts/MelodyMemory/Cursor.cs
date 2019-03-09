using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.Board;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Cursor : MonoBehaviour
{
    
#pragma warning disable 649
    [SerializeField] private float speed = 3;

    [SerializeField] private SpriteRenderer colorRenderer; 

#pragma warning restore 649
    

    public string ControlPrefix { get; private set; } = "Player1_";

    void Update()
    {
        float moveHorizontal = Input.GetAxis($"{ControlPrefix}{InputSuffix.Horizontal}");
        float moveVertical = Input.GetAxis($"{ControlPrefix}{InputSuffix.Vertical}");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        transform.Translate(movement * Time.deltaTime * speed);
    }

    public Ray GetRay()
    {
        return new Ray(transform.position, Vector3.forward);
    }

    public void SetPlayer(Player player)
    {
        ControlPrefix = player.InputPrefix;
        colorRenderer.color = player.Colour;
    }

    public void Show()
    {
        colorRenderer.enabled = true;
    }
    
    public void Hide()
    {
        colorRenderer.enabled = false;
    }

}
