using UnityEngine;

public class PongPlayer : MonoBehaviour
{
    private int playerNumber;
    private Color playerColor;
    public int PlayPosition { private get; set; }

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
        PongBallMovement pongBall = other.GetComponent<PongBallMovement>();

        pongBall.ChangeColor(playerColor);
        pongBall.LastTouchedByPlayer = playerNumber;
        pongBall.IncreaseSpeed();

        if (PlayPosition == 0)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (PlayPosition == 1)
        {
            pongBall.ChangeHorizontalDirection();
        }
        else if (PlayPosition == 2)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (PlayPosition == 3)
        {
            pongBall.ChangeHorizontalDirection();
        }
    }
}