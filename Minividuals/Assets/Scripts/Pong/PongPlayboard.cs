using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayboard : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] spawnPoints;
#pragma warning restore 649
    
    public int MaxPlayerNumber { private get; set; }

    /// <summary>
    /// Returns the spawn position for the given player number.
    /// </summary>
    /// <param name="playerNumber">Number of the Player.</param>
    /// <returns>Spawn point of player with given number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If playerNumber is not valid</exception>
    public Transform GetSpawnPointForPlayer(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex <= MaxPlayerNumber)
        {
            return spawnPoints[playerIndex].transform;
        }
        else
        {
            Debug.LogError(
                $"The given playerNumber is not between 0 and {MaxPlayerNumber}. " +
                $"Given playerNumber was {playerIndex}. " +
                "Returning zero vector");
            throw new ArgumentOutOfRangeException();
        }
    }
}
