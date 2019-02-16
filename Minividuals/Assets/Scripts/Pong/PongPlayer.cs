using UnityEngine;

public class PongPlayer : MonoBehaviour
{
    private int playerNumber;
    private Color playerColor;
    public int playPosition { private get; set; }

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = playerColor;
    }

    public void SetColor(Color color)
    {
        playerColor = color;
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player {playerNumber} - OnTriggerEnter from: " + other);

        PongBallMovement pongBall = other.GetComponent<PongBallMovement>();

        pongBall.ChangeColor(playerColor);
        pongBall.LastTouchedByPlayer = playerNumber;

        if (playPosition == 1)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (playPosition == 2)
        {
            pongBall.ChangeHorizontalDirection();
        }
        else if (playPosition == 3)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (playPosition == 4)
        {
            pongBall.ChangeHorizontalDirection();
        }
    }
}