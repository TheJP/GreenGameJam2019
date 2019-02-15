using UnityEngine;

public class SumoMain : MonoBehaviour
{
    [SerializeField] private GameObject playboardPrefab;
    [SerializeField] private GameObject playerPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject playboardObject = Instantiate(playboardPrefab);
        Playboard playboard = playboardObject.GetComponent<Playboard>();
        for (int playerNumber = 1; playerNumber <= 4; playerNumber++)
        {
            Instantiate(playerPrefab, playboard.GetSpawnpointForPlayer(playerNumber), Quaternion.identity);
        }
    }
}