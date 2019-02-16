using System;
using System.Collections;
using Assets.Scripts.Board;
using UnityEngine;

public class PongMain : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject pongBallPrefab;
    [SerializeField] private GameObject pongPlayBoardPrefab;
    [SerializeField] private GameObject pongPlayerPrefab;
    [SerializeField] private GameObject countdownUiPrefab;

    //Local Settings in case no main game is running.
    [SerializeField] private Color[] playerColors;
    [SerializeField] private String[] controlPrefixes;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MaxPlayerNumber = 4;

    private PongPlayboard pongPlayboard;
    private BoardController boardController;

    // Start is called before the first frame update
    void Start()
    {
        var playBoardObject = Instantiate(pongPlayBoardPrefab, transform);
        pongPlayboard = playBoardObject.GetComponent<PongPlayboard>();
        pongPlayboard.MaxPlayerNumber = MaxPlayerNumber;

        boardController = GameObject.Find("BoardController")?.GetComponent<BoardController>();
        InstantiatePlayers();

        StartCoroutine(CountDownForStart(TimeTillStart));
    }

    private void InstantiatePlayers()
    {
        int maxPlayerNumber = boardController != null ? boardController.players.Players.Count : MaxPlayerNumber;
        for (int playerIndex = 0; playerIndex < maxPlayerNumber; playerIndex++)
        {
            InstantiatePlayer(playerIndex);
        }
    }

    private void InstantiatePlayer(int playerIndex)
    {
        var spawnTransform = pongPlayboard.GetSpawnPointForPlayer(playerIndex);
        GameObject playerObject = Instantiate(pongPlayerPrefab, spawnTransform.position,
            spawnTransform.rotation, transform);

        PongPlayer player = playerObject.GetComponent<PongPlayer>();
        player.SetPlayerNumber(playerIndex + 1);
        player.playPosition = playerIndex + 1;
        player.SetColor(boardController != null
            ? boardController.players.Players[playerIndex].Colour
            : playerColors[playerIndex]);

        PongPlayerControl playerControl = playerObject.GetComponent<PongPlayerControl>();
        playerControl.ControlPrefix = boardController != null
            ? boardController.players.Players[playerIndex].InputPrefix
            : controlPrefixes[playerIndex];

        playerControl.enabled = false;
    }

    private IEnumerator CountDownForStart(float startTime)
    {
        float currentTime = startTime;
        while (currentTime >= 0)
        {
            var fadeoutObject = Instantiate(countdownUiPrefab);
            var countdownScript = fadeoutObject.GetComponent<Countdown>();
            countdownScript.TextMesh.text = Math.Ceiling(currentTime).ToString();
            countdownScript.TextMesh.color = new Color(0.6392157F, 0.5019608F, 0.3892157F, 1.0F);

            currentTime -= 1;
            yield return new WaitForSeconds(1);
        }

        EnableControls();

        ReleaseBall();
    }

    private void EnableControls()
    {
        var players = GetComponentsInChildren<PongPlayer>();

        foreach (var player in players)
        {
            player.GetComponent<PongPlayerControl>().enabled = true;
        }
    }

    private void ReleaseBall()
    {
        var pongBallObject = Instantiate(pongBallPrefab, transform);
        //PongBallMovement pongBall = pongBallObject.GetComponent<PongBallMovement>();
    }
}