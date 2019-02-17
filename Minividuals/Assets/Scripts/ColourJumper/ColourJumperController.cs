using Assets.Scripts.Board;
using System.Collections.Generic;
using UnityEngine;

public class ColourJumperController : MonoBehaviour
{
    [Tooltip("GameObject that contains all spawns")]
    public Transform spawnParent;

    [Tooltip("Prefab of the colour jumper player")]
    public ColourJumperPlayer blobPrefab;

    [Tooltip("Player parent GameObject")]
    public Transform playerParent;

    /// <summary>Board controller used to interface with the rest of the games. May be null!</summary>
    private BoardController boardController;

    private List<ColourJumperPlayer> blobs;

    private void Awake() => boardController = FindObjectOfType<BoardController>();

    private void Start()
    {
        var players = boardController?.players?.Players ??
            new[] { new Player(Color.green, "Player1_"), new Player(Color.blue, "Player2_") };

        blobs = new List<ColourJumperPlayer>();
        foreach (var player in players)
        {
            // Spawn a blob for each player
            var spawn = spawnParent.GetChild(Random.Range(0, spawnParent.childCount));
            var blob = Instantiate(blobPrefab, spawn.position, Quaternion.identity, playerParent);
            blobs.Add(blob);
            blob.Setup(player);
        }
    }

    private void Update()
    {

    }
}
