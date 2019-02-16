using System;
using System.Collections;
using Assets.Scripts.Board;
using UnityEngine;

/// <summary>
/// EntryPoint for the Sumo game. Instantiates the play board and the players.
/// Starts then a countdown to start the game.
/// </summary>
public class SumoMain : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject playboardPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject countdownUiPrefab;

    //Local Settings in case no main game is running.
    [SerializeField] private Color[] playerColors;
    [SerializeField] private String[] controlPrefixes;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MinPlayerNumber = 1;
    private const int MaxPlayerNumber = 4;

    private BoardController boardController;
    private SumoPlayboard sumoPlayboard;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject playboardObject = Instantiate(playboardPrefab, transform);
        sumoPlayboard = playboardObject.GetComponent<SumoPlayboard>();
        sumoPlayboard.MinPlayerNumber = MinPlayerNumber;
        sumoPlayboard.MaxPlayerNumber = MaxPlayerNumber;

        boardController = GameObject.Find("BoardController")?.GetComponent<BoardController>();
        InstantiatePlayers();

        StartCoroutine(CountDownForStart(TimeTillStart));
    }

    private void Update()
    {
        var players = GetComponentsInChildren<SumoPlayer>();
        if (players.Length <= 1)
        {
            Debug.Log("Game finished");
            //TODO: End the game.
        }
    }

    private void InstantiatePlayers()
    {
        int maxPlayerNumber = boardController != null ? boardController.players.Players.Count : MaxPlayerNumber;
        for (int playerNumber = 1; playerNumber <= maxPlayerNumber; playerNumber++)
        {
            InstantiatePlayer(playerNumber);
        }
    }

    private void InstantiatePlayer(int playerNumber)
    {
        GameObject playerObject = Instantiate(playerPrefab, sumoPlayboard.GetSpawnPointForPlayer(playerNumber),
            Quaternion.identity, transform);

        SumoPlayer player = playerObject.GetComponent<SumoPlayer>();
        player.SetPlayerNumber(playerNumber);

        SumoPlayerControl playerControl = playerObject.GetComponent<SumoPlayerControl>();
        playerControl.SetPlayerNumber(playerNumber);
        playerControl.ControlPrefix = boardController != null
            ? boardController.players.Players[playerNumber - 1].InputPrefix
            : controlPrefixes[playerNumber - 1];

        SumoPlayerStyle playerStyle = playerObject.GetComponent<SumoPlayerStyle>();
        playerStyle.SetColor(boardController != null
            ? boardController.players.Players[playerNumber - 1].Colour
            : playerColors[playerNumber - 1]);

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
    }

    private void EnableControls()
    {
        var players = GetComponentsInChildren<SumoPlayer>();

        foreach (var player in players)
        {
            player.GetComponent<SumoPlayerControl>().enabled = true;
        }
    }
}