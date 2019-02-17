using UnityEngine;

/// <summary>
/// Class for the Sumo player controls.
/// </summary>
public class SumoPlayerControl : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
    [SerializeField] private int boostCooldownSeconds;
#pragma warning restore 649

    public string ControlPrefix { private get; set; }
    private Rigidbody playerRigidbody;
    private float timeSinceLastBoost;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis($"{ControlPrefix + InputSuffix.Horizontal}");
        float moveVertical = Input.GetAxis($"{ControlPrefix + InputSuffix.Vertical}");
        float moveY = 0.0F;

        if (Input.GetButtonDown($"{ControlPrefix + InputSuffix.A}") && timeSinceLastBoost < boostCooldownSeconds)
        {
            Debug.Log("BOOOST!!!");
            timeSinceLastBoost = 0;
            moveHorizontal *= 1.5F;
            moveVertical *= 1.5F;
            moveY += 7.0F;
        }

        Vector3 movement = new Vector3(moveHorizontal, moveY, moveVertical);
        playerRigidbody.AddForce(movement * speed);
    }
}