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
    /// <param name="position">Position on the Filed. (Starts down left with 0 CCW till 3</param>
    /// <returns></returns>
    public Transform GetSpawnPointForPosition(int position)
    {
            return spawnPoints[position].transform;
    }
}
