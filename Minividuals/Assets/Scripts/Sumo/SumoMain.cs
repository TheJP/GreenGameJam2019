using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject gameTitleCanvasPrefab;

    //Local Settings in case no main game is running.
    [SerializeField] private Color[] playerColors;
    [SerializeField] private String[] controlPrefixes;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MaxPlayerNumber = 4;

    private BoardController boardController;
    private SumoPlayboard sumoPlayboard;

    private readonly List<int> scoreList = new List<int> {-5, -3, 3, 5};
    private readonly List<int> defeatedPlayers = new List<int>(4);
    private readonly List<int> playersInGame = new List<int>(4);

    // Start is called before the first frame update
    private void Start()
    {
        GameObject playboardObject = Instantiate(playboardPrefab, transform);
        sumoPlayboard = playboardObject.GetComponent<SumoPlayboard>();
        sumoPlayboard.MaxPlayerNumber = MaxPlayerNumber;

        boardController = GameObject.Find("BoardController")?.GetComponent<BoardController>();
        InstantiatePlayers();

        var gameTitleCanvasObject = Instantiate(gameTitleCanvasPrefab);
        MinigameTitleScreen minigameTitleScreen = gameTitleCanvasObject.GetComponentInChildren<MinigameTitleScreen>();
        minigameTitleScreen.SetText("SUMO!!!");
        minigameTitleScreen.FadeOut();

        StartCoroutine(CountDownForStart(TimeTillStart));

        defeatedPlayers.Clear();
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
        GameObject playerObject = Instantiate(playerPrefab, sumoPlayboard.GetSpawnPointForPlayer(playerIndex),
            Quaternion.identity, transform);

        SumoPlayer player = playerObject.GetComponent<SumoPlayer>();
        player.SetPlayerNumber(playerIndex + 1);

        SumoPlayerControl playerControl = playerObject.GetComponent<SumoPlayerControl>();
        playerControl.ControlPrefix = boardController != null
            ? boardController.players.Players[playerIndex].InputPrefix
            : controlPrefixes[playerIndex];

        SumoPlayerStyle playerStyle = playerObject.GetComponent<SumoPlayerStyle>();
        playerStyle.SetColor(boardController != null
            ? boardController.players.Players[playerIndex].Colour
            : playerColors[playerIndex]);

        playersInGame.Add(playerIndex + 1);
        playerControl.enabled = false;
    }

    private IEnumerator CountDownForStart(float startTime)
    {
        float currentTime = startTime;
        while (currentTime >= 0)
        {
            var fadeoutObject = Instantiate(countdownUiPrefab);
            var countdownScript = fadeoutObject.GetComponent<Countdown>();
            var nextDigit = Math.Ceiling(currentTime);
            if (Math.Abs(nextDigit) < 0.5)
            {
                countdownScript.TextMesh.text = "GO!!!";
            }
            else
            {
                countdownScript.TextMesh.text = nextDigit.ToString();
            }

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

    /// <summary>
    /// This Method is called through the Unity messaging system every time a player is defeated. 
    /// </summary>
    /// <param name="playerNumber"></param>
    private void PlayerDefeated(int playerNumber)
    {
        defeatedPlayers.Add(playerNumber);
        playersInGame.Remove(playerNumber);

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        //If there is only one player left in the game, the gam is finished.
        if (playersInGame.Count == 1)
        {
            defeatedPlayers.Add(playersInGame[0]);

            List<Tuple<int, int>> scores = new List<Tuple<int, int>>(4);

            for (int i = 0; i < defeatedPlayers.Count; i++)
            {
                scores.Add(new Tuple<int, int>(defeatedPlayers[i], scoreList[i]));
            }
            
            defeatedPlayers.Clear();
            playersInGame.Clear();
            
            //TODO: boardController.MinigameFinished(scores);
        }
    }
}