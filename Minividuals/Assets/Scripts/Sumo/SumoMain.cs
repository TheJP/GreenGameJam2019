using UnityEngine;

public class SumoMain : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameObject playboardPrefab;
    [SerializeField] private GameObject playerPrefab;
#pragma warning restore 649

    // Start is called before the first frame update
    private void Start()
    {
        GameObject playboardObject = Instantiate(playboardPrefab);
        Playboard playboard = playboardObject.GetComponent<Playboard>();
        for (int playerNumber = 1; playerNumber <= 4; playerNumber++)
        {
            InstantiatePlayer(playboard, playerNumber);
        }

        //Start Game... Enable Controls...
    }

    private void InstantiatePlayer(Playboard playBoard, int playerNumber)
    {
        GameObject playerObject = Instantiate(playerPrefab, playBoard.GetSpawnpointForPlayer(playerNumber),
            Quaternion.identity);

        PlayerControls playerControls = playerObject.GetComponent<PlayerControls>();
        playerControls.SetPlayerNumber(playerNumber);

        PlayerStyle playerStyle = playerObject.GetComponent<PlayerStyle>();
        playerStyle.SetColor(playBoard.GetColorForPlayer(playerNumber));
    }
}