using UnityEngine;

public class PongMain : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject pongBallPrefab;
    [SerializeField] private GameObject pongPlayBoardPrefab;
    [SerializeField] private GameObject pongPlayerPrefab;
    [SerializeField] private Color[] playerColors;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MinPlayerNumber = 1;
    private const int MaxPlayerNumber = 2;

    // Start is called before the first frame update
    void Start()
    {
        var playBoardObject = Instantiate(pongPlayBoardPrefab, transform);
        PongPlayboard pongPlayboard = playBoardObject.GetComponent<PongPlayboard>();
        pongPlayboard.MinPlayerNumber = MinPlayerNumber;
        pongPlayboard.MaxPlayerNumber = MaxPlayerNumber;

        var pongBallObject = Instantiate(pongBallPrefab, transform);
        PongBall pongBall = pongBallObject.GetComponent<PongBall>();

        for (int playerNumber = MinPlayerNumber; playerNumber <= MaxPlayerNumber; playerNumber++)
        {
            InstantiatePlayer(pongPlayboard, playerNumber);
        }
    }


    private void InstantiatePlayer(PongPlayboard playBoard, int playerNumber)
    {
        var spawnTransform = playBoard.GetSpawnPointForPlayer(playerNumber);
        GameObject playerObject = Instantiate(pongPlayerPrefab, spawnTransform.position,
            spawnTransform.rotation, transform);

        PongPlayer player = playerObject.GetComponent<PongPlayer>();
        player.SetPlayerNumber(playerNumber);

        PongPlayerControl playerControl = playerObject.GetComponent<PongPlayerControl>();
        playerControl.SetPlayerNumber(playerNumber);
//        playerControl.enabled = false;

        PongPlayerStyle playerStyle = playerObject.GetComponent<PongPlayerStyle>();
        playerStyle.SetColor(playerColors[playerNumber-1]);
    }
}