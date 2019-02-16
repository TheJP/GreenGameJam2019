using UnityEngine;

public class PongWall : MonoBehaviour
{
    public int WallPosition { private get; set; }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Wall {WallPosition} - OnTriggerEnter from: " + other);

        PongBallMovement pongBall = other.GetComponent<PongBallMovement>();

        if (WallPosition == 0)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (WallPosition == 1)
        {
            pongBall.ChangeHorizontalDirection();
        }
        else if (WallPosition == 2)
        {
            pongBall.ChangeVerticalDirection();
        }
        else if (WallPosition == 3)
        {
            pongBall.ChangeHorizontalDirection();
        }
    }
}