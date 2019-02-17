using Assets.Scripts.Board;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ColourJumper
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ColourJumperPlayer : MonoBehaviour
    {
        [Tooltip("Renderer that is used to colour the blob in the players colour")]
        public SpriteRenderer colourRenderer;

        [Tooltip("Acceleration of player")]
        public float acceleration = 1f;

        [Tooltip("Breaking acceleration of player")]
        public float deacceleration = 1f;

        [Tooltip("Maximal speed in distance per second")]
        public Vector2 maxVelocity = new Vector2(20f, 20f);

        [Tooltip("Force with which the player jumps vertically")]
        public float jumpForce = 20f;

        [Tooltip("GameObject with particle system that is shown when player dies")]
        public ParticleSystem deathAnimation;

        public Platform CurrentPlatform { get; set; }

        public Player Player { get; private set; }
        private Rigidbody2D rigidbody2d;

        private readonly HashSet<Collider2D> groundCollisions = new HashSet<Collider2D>();

        private bool OnGround => groundCollisions.Count > 0;

        private bool isDead = false;

        private void Awake() => rigidbody2d = GetComponent<Rigidbody2D>();

        public void Setup(Player player)
        {
            Player = player;
            colourRenderer.color = player.Colour;
            var particleMain = deathAnimation.main;
            particleMain.startColor = player.Colour;
        }

        private void Start()
        {
            if (Player == null)
            {
                Setup(new Player(Color.green, "Player1_"));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) => groundCollisions.Add(collision);
        private void OnTriggerExit2D(Collider2D collision) => groundCollisions.Remove(collision);

        private void FixedUpdate()
        {
            if (isDead) { return; }

            var velocity = rigidbody2d.velocity;

            var xMovement = Input.GetAxis($"{Player.InputPrefix}{InputSuffix.Horizontal}");
            if (Mathf.Abs(xMovement) < 0.001)
            {
                var breakVelocity = Mathf.Max(0f, (Mathf.Abs(velocity.x) * deacceleration * Time.deltaTime));
                if (Mathf.Abs(velocity.x) < Mathf.Abs(breakVelocity)) { velocity.x = 0f; }
                else { velocity += (Vector2.left * Mathf.Sign(velocity.x)) * breakVelocity; }
            }
            else { velocity += (Vector2.right * xMovement) * (acceleration * Time.deltaTime); }

            velocity.x = Mathf.Clamp(velocity.x, -maxVelocity.x, maxVelocity.x);
            velocity.y = Mathf.Clamp(velocity.y, -maxVelocity.y, maxVelocity.y);

            rigidbody2d.velocity = velocity;

            if (Input.GetButtonDown($"{Player.InputPrefix}{InputSuffix.A}") && OnGround)
            {
                rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

        }

        public void KillPlayer()
        {
            if (isDead) { return; }
            isDead = true;
            StartCoroutine(KillingPlayer());
        }

        private IEnumerator KillingPlayer()
        {
            deathAnimation.gameObject.SetActive(true);
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            var start = Time.time;
            var duration = 1.5f;
            while (Time.time - start < duration)
            {
                foreach (var sprite in sprites)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f - (Time.time - start) / duration);
                }
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
