using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBallMovement : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float initialSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float currentSpeed;
#pragma warning restore 649

    private float verticalDirection;
    private float horizontalDirection;

    private const int xBorder = 4;
    private const int zBorder = 4;

    public int LastTouchedByPlayer { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        horizontalDirection = Random.Range(0.0F, 1.0F);
        verticalDirection = Random.Range(0.0F, 1.0F);

        LastTouchedByPlayer = -1;
        currentSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(horizontalDirection, 0.0f, verticalDirection);
        transform.Translate(movement * Time.deltaTime * currentSpeed);

        if (transform.position.x < -xBorder
            || transform.position.x > xBorder
            || transform.position.z < -zBorder
            || transform.position.z > zBorder)
        {
            Debug.Log($"Yeeey :) Player {LastTouchedByPlayer} made a point. Isn't this hilarious?");
            SendMessageUpwards("PongBallDestroyed", LastTouchedByPlayer);
            Destroy(gameObject);
        }
    }

    public void ChangeVerticalDirection()
    {
        verticalDirection *= -1;
        verticalDirection += Random.Range(-0.3F, 0.3F);
    }

    public void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;
        horizontalDirection += Random.Range(-0.3F, 0.3F);
    }

    public void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    public void IncreaseSpeed()
    {
        currentSpeed += 0.5F;
        currentSpeed = Math.Min(currentSpeed, maxSpeed);
    }
}