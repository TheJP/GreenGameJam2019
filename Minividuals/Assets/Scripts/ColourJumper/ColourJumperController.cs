using Assets.Scripts.Board;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColourJumperController : MonoBehaviour
{
    [Tooltip("GameObject that contains all spawns")]
    public Transform spawnParent;

    [Tooltip("Prefab of the colour jumper player")]
    public ColourJumperPlayer blobPrefab;

    [Tooltip("Player parent GameObject")]
    public Transform playerParent;

    [Tooltip("Textbox that allows information to be displayed")]
    public Text displayText;

    [Tooltip("Time during which colours are shuffled")]
    public float colourShuffleTime = 3f;

    [Tooltip("Time that the shuffler waits between multiple shuffles")]
    public float shuffleWait = 0.2f;

    [Tooltip("Time the players get to reach their platform at the start")]
    public float initialRoundTime = 20f;

    [Tooltip("Time by which the round time is reduced each round")]
    public float roundTimeCutoff = 2f;

    /// <summary>Board controller used to interface with the rest of the games. May be null!</summary>
    private BoardController boardController;

    private List<ColourJumperPlayer> blobs;

    private Platform[] platforms;

    private void Awake() => boardController = FindObjectOfType<BoardController>();

    private IEnumerator Start()
    {
        platforms = FindObjectsOfType<Platform>();

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

        yield return GameLoop();
    }

    private bool MissingAColour() =>
        blobs.Any(blob => !platforms.Any(platform => blob.Player.Colour == platform.Colour));

    private void ShufflePlatformColours()
    {
        Debug.Assert(blobs.Count > 0);
        foreach(var platform in platforms)
        {
            platform.SetColour(blobs[Random.Range(0, blobs.Count)].Player.Colour);
        }
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            var roundTime = initialRoundTime;
            var startShuffle = Time.time;
            while (Time.time - startShuffle < colourShuffleTime || MissingAColour())
            {
                ShufflePlatformColours();
                yield return new WaitForSeconds(shuffleWait);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
