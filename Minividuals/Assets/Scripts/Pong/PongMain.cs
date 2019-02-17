using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Board;
using UnityEngine;

public class PongMain : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject pongBallPrefab;
    [SerializeField] private GameObject pongPlayBoardPrefab;
    [SerializeField] private GameObject pongBoardWallPrefab;
    [SerializeField] private GameObject pongPlayerPrefab;
    [SerializeField] private GameObject countdownUiPrefab;
    [SerializeField] private GameObject gameTitleCanvasPrefab;

    //Local Settings in case no main game is running.
    [SerializeField] private Color[] playerColors;
    [SerializeField] private string[] controlPrefixes;

    [SerializeField] private GameObject ballContainer;
    [SerializeField] private float gameTime;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MaxPlayerNumber = 2;

    private PongPlayboard pongPlayboard;
    private BoardController boardController;
    private Dictionary<int, int> scores = new Dictionary<int, int>(4);

    private float timeSinceLastBall;
    private const float ballRespawnTime = 20;

    // Start is called before the first frame update
    void Start()
    {
        var gameTitleCanvasObject = Instantiate(gameTitleCanvasPrefab);
        MinigameTitleScreen minigameTitleScreen = gameTitleCanvasObject.GetComponentInChildren<MinigameTitleScreen>();
        minigameTitleScreen.SetText("PONG!!!");
        minigameTitleScreen.FadeOut();

        var playBoardObject = Instantiate(pongPlayBoardPrefab, transform);
        pongPlayboard = playBoardObject.GetComponent<PongPlayboard>();
        pongPlayboard.MaxPlayerNumber = MaxPlayerNumber;

        boardController = GameObject.Find("BoardController")?.GetComponent<BoardController>();
        InstantiatePlayers();

        StartCoroutine(CountDownForStart(TimeTillStart));
    }

    private void Update()
    {
        gameTime = gameTime - Time.deltaTime;


        timeSinceLastBall = timeSinceLastBall + Time.deltaTime;
        if (timeSinceLastBall > ballRespawnTime)
        {
            ReleaseBall();
            timeSinceLastBall = 0;
        }

        if (gameTime <= 0)
        {
            gameTime = 999;
            if (boardController != null)
            {
                List<(Player player, int steps)> endScores = new List<(Player player, int steps)>(4);
                foreach (var player in scores.Keys)
                {
                    scores.TryGetValue(player, out var score);
                    endScores.Add((boardController.players.Players[player - 1], score));
                }

                boardController.FinishedMiniGame(endScores);
            }
            else
            {
                foreach (var player in scores.Keys)
                {
                    scores.TryGetValue(player, out var score);
                    Debug.Log($"Score of player {player} is: {score}");
                }
            }
        }
    }

    private void InstantiatePlayers()
    {
        int maxPlayerNumber = boardController != null ? boardController.players.Players.Count : MaxPlayerNumber;

        if (maxPlayerNumber == 1)
        {
            Debug.Log("You started a Pong with one player? Doesn't really make sense, huh?");
            //TODO: Handle single player properly
        }
        else if (maxPlayerNumber == 2)
        {
            InstantiatePlayer(0, 1);
            InstantiatePlayer(1, 3);
            InstantiateWall(0);
            InstantiateWall(2);
        }
        else if (maxPlayerNumber == 3)
        {
            InstantiatePlayer(0, 0);
            InstantiatePlayer(1, 1);
            InstantiatePlayer(2, 3);
            InstantiateWall(2);
        }
        else if (maxPlayerNumber == 4)
        {
            for (int playerIndex = 0; playerIndex < maxPlayerNumber; playerIndex++)
            {
                InstantiatePlayer(playerIndex, playerIndex);
            }
        }
    }

    private void InstantiatePlayer(int playerIndex, int positionOnField)
    {
        var spawnTransform = pongPlayboard.GetSpawnPointForPosition(positionOnField);
        GameObject playerObject = Instantiate(pongPlayerPrefab, spawnTransform.position,
            spawnTransform.rotation, transform);

        PongPlayer player = playerObject.GetComponent<PongPlayer>();
        player.SetPlayerNumber(playerIndex + 1);
        player.PlayPosition = positionOnField;
        player.SetColor(boardController != null
            ? boardController.players.Players[playerIndex].Colour
            : playerColors[playerIndex]);

        PongPlayerControl playerControl = playerObject.GetComponent<PongPlayerControl>();
        playerControl.ControlPrefix = boardController != null
            ? boardController.players.Players[playerIndex].InputPrefix
            : controlPrefixes[playerIndex];

        playerControl.enabled = false;
    }

    private void InstantiateWall(int position)
    {
        var spawnTransform = pongPlayboard.GetSpawnPointForPosition(position);
        GameObject wallObject =
            Instantiate(pongBoardWallPrefab, spawnTransform.position, spawnTransform.rotation, transform);
        PongWall pongWall = wallObject.GetComponent<PongWall>();
        pongWall.WallPosition = position;
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
        var pongBallObject = Instantiate(pongBallPrefab, ballContainer.transform);
        //PongBallMovement pongBall = pongBallObject.GetComponent<PongBallMovement>();
    }

    public void PongBallDestroyed(int lastTouchedPlayer)
    {
        Debug.Log("PongBallDestroyed called with lastTouchedPlayer: " + lastTouchedPlayer);
        if (lastTouchedPlayer != -1)
        {
            if (scores.TryGetValue(lastTouchedPlayer, out var currentScore))
            {
                scores.Remove(lastTouchedPlayer);
                scores.Add(lastTouchedPlayer, ++currentScore);
            }
            else
            {
                scores.Add(lastTouchedPlayer, 1);
            }
        }

        ReleaseBall();
    }
}