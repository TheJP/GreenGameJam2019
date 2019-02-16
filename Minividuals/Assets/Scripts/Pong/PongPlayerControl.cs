using UnityEngine;

public class PongPlayerControl : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
    [SerializeField] private int playerNumber;
#pragma warning restore 649

    private void Update()
    {
        float moveHorizontal = Input.GetAxis($"Player{playerNumber}_Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.Translate(movement * Time.deltaTime * speed);
    }

    public void SetPlayerNumber(int playerNumber)
    {
        //TODO: Check if player number is in Range of MinPlayerNumber and MaxPlayerNumber, which has also have to been Set First.
        this.playerNumber = playerNumber;
    }
}