using Assets.Scripts.Board;
using UnityEngine;

public class ColourJumperPlayer : MonoBehaviour
{
    [Tooltip("Renderer that is used to colour the blob in the players colour")]
    public SpriteRenderer colourRenderer;

    [Tooltip("Acceleration of player")]
    public float acceleration = 1f;

    [Tooltip("Maximal speed in distance per second")]
    public float maxVelocity = 5f;

    public float drag = 0.2f;

    [Tooltip("Slowing the player over time")]
    public float dragFactor = 0.1f;

    public Player Player { get; private set; }

    private Vector3 velocity = Vector3.zero;

    public void Setup(Player player)
    {
        Player = player;
        colourRenderer.color = player.Colour;
    }

    private void Start() => Setup(new Player(Color.green, "Player1_"));

    private void Update()
    {
        var xMovement = Input.GetAxis($"{Player.InputPrefix}{InputSuffix.Horizontal}");
        velocity.x -= (velocity.x * dragFactor) * Time.deltaTime;
        velocity.x = Mathf.Sign(velocity.x) * Mathf.Min(0f, Mathf.Abs(velocity.x) - drag * Time.deltaTime);
        velocity += (Vector3.right * xMovement).normalized * (acceleration + drag) * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        transform.position += velocity;

        if (Input.GetButtonDown($"{Player.InputPrefix}{InputSuffix.A}"))
        {

        }

    }
}
