using UnityEngine;

public class PlayerControls : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
    [SerializeField] private int playerNumber;
#pragma warning restore 649

    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis($"Horizontal_{playerNumber}");
        float moveVertical = Input.GetAxis($"Vertical_{playerNumber}");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        playerRigidbody.AddForce(movement * speed);
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }
}