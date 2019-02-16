using System;
using UnityEngine;

/// <summary>
/// Class to control the current play board.
/// </summary>
public class SumoPlayboard : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] spawnPoints;
#pragma warning restore 649

    public int MaxPlayerNumber { private get; set; }

    private void Awake()
    {
        MaxPlayerNumber = 4;
    }

    /// <summary>
    /// Returns the spawn position for the given player number (1-4)
    /// </summary>
    /// <param name="playerIndex">Number of the Player (1-4).</param>
    /// <returns>Spawn point of player with given number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If playerNumber is not between 1 and 4</exception>
    public Vector3 GetSpawnPointForPlayer(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < MaxPlayerNumber)
        {
            return spawnPoints[playerIndex].transform.position;
        }
        else
        {
            Debug.LogError(
                $"The given playerIndex is not between 0 and {MaxPlayerNumber-1}. " +
                $"Given playerNumber was {playerIndex}. " +
                "Returning zero vector");
            throw new ArgumentOutOfRangeException();
        }
    }
}