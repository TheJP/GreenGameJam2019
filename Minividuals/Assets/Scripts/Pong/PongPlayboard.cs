using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayboard : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] spawnPoints;
#pragma warning restore 649
    
    public int MinPlayerNumber { private get; set; }
    public int MaxPlayerNumber { private get; set; }
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
    public Transform GetSpawnPointForPlayer(int playerNumber)
    {
        if (playerNumber >= MinPlayerNumber && playerNumber <= MaxPlayerNumber)
        {
            return spawnPoints[playerNumber - 1].transform;
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
}
