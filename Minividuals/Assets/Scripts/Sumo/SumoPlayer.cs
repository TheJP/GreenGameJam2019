using UnityEngine;

/// <summary>
/// This class contains infos about the players state. If the player fells from the platform, he dies.
/// </summary>
public class SumoPlayer : MonoBehaviour
{
    private int playerNumber;
    private const int DepthToDie = -20;

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y < DepthToDie)
        {
            Debug.Log($"Player number {playerNumber} died :( How sad...");
            Destroy(gameObject);
        }
    }

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }
}