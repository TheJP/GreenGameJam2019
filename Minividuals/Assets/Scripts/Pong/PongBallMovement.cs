using UnityEngine;

public class PongBallMovement : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
#pragma warning restore 649

    private float verticalDirection;
    private float horizontalDirection;

    public int LastTouchedByPlayer { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Random start direction
        horizontalDirection = 1.0F;
        verticalDirection = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(horizontalDirection, 0.0f, verticalDirection);
        transform.Translate(movement * Time.deltaTime * speed);
    }

    public void ChangeVerticalDirection()
    {
        verticalDirection *= -1;
    }

    public void ChangeHorizontalDirection()
    {
        horizontalDirection *= -1;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}