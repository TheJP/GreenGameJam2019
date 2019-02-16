using UnityEngine;

/// <summary>
/// Class for the Sumo player controls.
/// </summary>
public class SumoPlayerControl : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
    [SerializeField] private int playerNumber;
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

    public void SetPlayerNumber(int playerNumber)
    {
        //TODO: Check if player number is in Range of MinPlayerNumber and MaxPlayerNumber, which has also have to been Set First.
        this.playerNumber = playerNumber;
    }
}