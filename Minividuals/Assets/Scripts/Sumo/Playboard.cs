using System;
using UnityEngine;

/// <summary>
/// Class to control the current play board.
/// </summary>
public class Playboard : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Color[] playerColors;
#pragma warning restore 649

    public int MinPlayerNumber { private get; set; }
    public int MaxPlayerNumber { private get; set; }

    private void Awake()
    {
        MinPlayerNumber = 1;
        MaxPlayerNumber = 4;
    }

    /// <summary>
    /// Returns the spawn position for the given player number (1-4)
    /// </summary>
    /// <param name="playerNumber">Number of the Player (1-4).</param>
    /// <returns>Spawn point of player with given number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If playerNumber is not between 1 and 4</exception>
    public Vector3 GetSpawnPointForPlayer(int playerNumber)
    {
        if (playerNumber >= MinPlayerNumber && playerNumber <= MaxPlayerNumber)
        {
            return spawnPoints[playerNumber - 1].transform.position;
        }
        else
        {
            Debug.LogError(
                $"The given playerNumber is not between {MinPlayerNumber} and {MaxPlayerNumber}. " +
                $"Given playerNumber was {playerNumber}. " +
                "Returning zero vector");
            throw new ArgumentOutOfRangeException();
        }
    }

    public Color GetColorForPlayer(int playerNumber)
    {
        return playerColors[playerNumber - 1];
    }
}