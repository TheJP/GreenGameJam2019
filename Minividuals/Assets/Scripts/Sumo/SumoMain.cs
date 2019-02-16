using System;
using System.Collections;
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

    [SerializeField] private Color[] playerColors;
#pragma warning restore 649

    private const int TimeTillStart = 3;
    private const int MinPlayerNumber = 1;
    private const int MaxPlayerNumber = 4;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject playboardObject = Instantiate(playboardPrefab, transform);
        SumoPlayboard playboard = playboardObject.GetComponent<SumoPlayboard>();
        playboard.MinPlayerNumber = MinPlayerNumber;
        playboard.MaxPlayerNumber = MaxPlayerNumber;

        for (int playerNumber = MinPlayerNumber; playerNumber <= MaxPlayerNumber; playerNumber++)
        {
            InstantiatePlayer(playboard, playerNumber);
        }

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

    private void InstantiatePlayer(SumoPlayboard playBoard, int playerNumber)
    {
        GameObject playerObject = Instantiate(playerPrefab, playBoard.GetSpawnPointForPlayer(playerNumber),
            Quaternion.identity, transform);

        SumoPlayer player = playerObject.GetComponent<SumoPlayer>();
        player.SetPlayerNumber(playerNumber);

        SumoPlayerControl playerControl = playerObject.GetComponent<SumoPlayerControl>();
        playerControl.SetPlayerNumber(playerNumber);
        playerControl.enabled = false;

        SumoPlayerStyle playerStyle = playerObject.GetComponent<SumoPlayerStyle>();
        playerStyle.SetColor(playerColors[playerNumber - 1]);
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