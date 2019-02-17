using Assets.Scripts.Board;
using System.Linq;
using UnityEngine;

public class ColourJumperController : MonoBehaviour
{
    [Tooltip("GameObject that contains all spawns")]
    public Transform spawnParent;

    /// <summary>Board controller used to interface with the rest of the games. May be null!</summary>
    private BoardController boardController;

    private void Awake() => boardController = FindObjectOfType<BoardController>();

    private void Start()
    {
        var players = boardController?.players?.Players ??
            new[] { new Player(Color.green, "Player1_"), new Player(Color.blue, "Player2_") };

        
    }

    private void Update()
    {

    }
}
