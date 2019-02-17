using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject pongUiPrefab;

    //Local Settings in case no main game is running.
    [SerializeField] private Color[] playerColors;
    [SerializeField] private string[] controlPrefixes;

    [SerializeField] private GameObject ballContainer;
    [SerializeField] private float gameTime;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MaxPlayerNumber = 3;

    private PongPlayboard pongPlayboard;
    private BoardController boardController;
    private int[] scores;
    private readonly List<int> scoreList = new List<int> {5, 3, -3, -5};

    private float timeSinceLastBall;
    private const float ballRespawnTime = 20;

    private PongUi pongUi;

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

        var pongUiObject = Instantiate(pongUiPrefab, transform);
        pongUi = pongUiObject.GetComponent<PongUi>();

        boardController = GameObject.Find("BoardController")?.GetComponent<BoardController>();
        InstantiatePlayers();

        StartCoroutine(CountDownForStart(TimeTillStart));
    }

    private void Update()
    {
        gameTime = gameTime - Time.deltaTime;
        pongUi.DisplayGameTime(Math.Ceiling(gameTime).ToString());

        timeSinceLastBall = timeSinceLastBall + Time.deltaTime;
        if (timeSinceLastBall > ballRespawnTime)
        {
            ReleaseBall();
            timeSinceLastBall = 0;
        }

        CheckGameOver();
    }

    private void InstantiatePlayers()
    {
        int maxPlayerNumber = boardController != null ? boardController.players.Players.Count : MaxPlayerNumber;
        scores = new int[maxPlayerNumber];

        if (maxPlayerNumber == 1)
        {
            Debug.Log("You started a Pong with one player? Doesn't really make sense, huh?");
            if (boardController != null)
            {
                boardController.FinishedMiniGame(new[] {(boardController.players.Players[0], 1)});
            }
            else
            {
                InstantiatePlayer(0, 0);
                InstantiateWall(1);
                InstantiateWall(2);
                InstantiateWall(3);
            }
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
//        Debug.Log("PongBallDestroyed called with lastTouchedPlayer: " + lastTouchedPlayer);
        if (lastTouchedPlayer != -1)
        {
            scores[lastTouchedPlayer - 1]++;
            pongUi.DisplayScores(scores);
        }

        ReleaseBall();
    }

    private void CheckGameOver()
    {
        if (gameTime <= 0)
        {
            gameTime = 999;

            if (boardController != null)
            {
                var endScores = new List<(Player player, int steps)>(4);


                var scoresList = scores.ToList();
                var sortedByScores = scoresList
                    .Select((x, i) => new KeyValuePair<int, int>(x, i))
                    .OrderBy(x => x.Key)
                    .Reverse()
                    .ToList();
                var ranks = sortedByScores.Select(x => x.Value).ToList();

                for (int index = 0; index < scores.Length; index++)
                {
                    endScores.Add((boardController.players.Players[ranks[index]], scoreList[index]));
                }

                boardController.FinishedMiniGame(endScores);
            }
            else
            {
                for (int index = 0; index < scores.Length; index++)
                {
                    Debug.Log($"Score of player {index + 1} is: {scores[index]}");
                }
            }
        }
    }
}