using UnityEngine;

/// <summary>
/// Class for the Sumo player controls.
/// </summary>
public class SumoPlayerControl : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
#pragma warning restore 649

    public string ControlPrefix { get; set; }
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis($"{ControlPrefix}Horizontal");
        float moveVertical = Input.GetAxis($"{ControlPrefix}Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        playerRigidbody.AddForce(movement * speed);
    }
}