using System;
using UnityEngine;

/// <summary>
/// Class to control the sumo play board.
/// </summary>
public class SumoPlayboard : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject[] spawnPoints;
#pragma warning restore 649

    /// <summary>
    /// Returns the spawn position for the given number. (1 to 4)
    /// The 4 spawn points are predefined within the Prefab.
    /// </summary>
    /// <param name="positionNumber">Number of the Position (zero based)(0-3).</param>
    /// <returns>Position of the spawn point with the given number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If position number is not between 0 and 3</exception>
    public Vector3 GetSpawnPoint(int positionNumber)
    {
        if (positionNumber < 0 || positionNumber > 3)
        {
            throw new ArgumentOutOfRangeException(
                $"The given positionindex is not between 0 and {spawnPoints.Length - 1}. " +
                $"Given playerNumber was {positionNumber}. " +
                "Returning zero vector");
        }

        return spawnPoints[positionNumber].transform.position;
    }
}