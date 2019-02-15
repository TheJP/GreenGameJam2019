using System;
using UnityEngine;

public class Playboard : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Returns the spawn position for the given player number (1-4)
    /// </summary>
    /// <param name="playerNumber">Number of the Player (1-4).</param>
    /// <returns>Spawn point of player with given number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If playerNumber is not between 1 and 4</exception>
    public Vector3 GetSpawnpointForPlayer(int playerNumber)
    {
        const int minPlayerNumber = 1;
        const int maxPlayerNumber = 4;
        if (playerNumber >= minPlayerNumber && playerNumber <= maxPlayerNumber)
        {
            return spawnPoints[playerNumber-1].transform.position;
        }
        else
        {
            Debug.LogError(
                $"The given playerNumber is not between {minPlayerNumber} and {maxPlayerNumber}. " +
                $"Given playerNumber was {playerNumber}. " +
                "Returning zero vector");
            throw new ArgumentOutOfRangeException();
        }
    }
}