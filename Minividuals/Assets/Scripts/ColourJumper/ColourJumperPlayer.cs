using Assets.Scripts.Board;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ColourJumperPlayer : MonoBehaviour
{
    [Tooltip("Renderer that is used to colour the blob in the players colour")]
    public SpriteRenderer colourRenderer;

    [Tooltip("Acceleration of player")]
    public float acceleration = 1f;

    [Tooltip("Maximal speed in distance per second")]
    public Vector2 maxVelocity = new Vector2(20f, 20f);

    public float jumpForce = 20f;

    public Player Player { get; private set; }
    private Rigidbody2D rigidbody2d;
    private bool onGround = true;

    private void Awake() => rigidbody2d = GetComponent<Rigidbody2D>();

    public void Setup(Player player)
    {
        Player = player;
        colourRenderer.color = player.Colour;
    }

    private void Start() => Setup(new Player(Color.green, "Player1_"));

    private void OnTriggerEnter2D(Collider2D collision) => onGround = true;
    private void OnTriggerExit2D(Collider2D collision) => onGround = false;

    private void FixedUpdate()
    {
        var velocity = rigidbody2d.velocity;
        var xMovement = Input.GetAxis($"{Player.InputPrefix}{InputSuffix.Horizontal}");
        velocity += (Vector2.right * xMovement) * (acceleration * Time.deltaTime);
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocity.x, maxVelocity.x);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocity.y, maxVelocity.y);

        rigidbody2d.velocity = velocity;

        if (Input.GetButtonDown($"{Player.InputPrefix}{InputSuffix.A}") && onGround)
        {
            rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

    }
}
