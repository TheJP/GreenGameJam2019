using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    public class MeloMemoMain : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject playboardPrefab;
        [SerializeField] private GameObject tilePrefab;
#pragma warning restore 649

        // Start is called before the first frame update
        private void Start()
        {
            GameObject playboardObject = Instantiate(playboardPrefab);
            Playboard tile = playboardObject.GetComponent<Playboard>();
            for (int tileNumber = 1; tileNumber <= 32; tileNumber++)
            {
                InstantiateTile(tile, tileNumber);
            }

            //Start Game... Enable Controls...

        
            GameObject tileObject = Instantiate(tilePrefab);        // TODO we need an array of these
            // TODO init tiles
            Playboard playboard = tileObject.GetComponent<Playboard>();
            for (int playerNumber = 1; playerNumber <= 4; playerNumber++)
            {
                InstantiateTile(playboard, playerNumber);
            }

            //Start Game... Enable Controls...
        }

        private void InstantiateTile(Playboard playboard, int tileNumber)
        {
            GameObject playerObject = Instantiate(tilePrefab, playboard.GetSpawnpointForPlayer(tileNumber),
                Quaternion.identity);

            PlayerControls playerControls = playerObject.GetComponent<PlayerControls>();
            playerControls.SetPlayerNumber(tileNumber);

            PlayerStyle playerStyle = playerObject.GetComponent<PlayerStyle>();
            playerStyle.SetColor(playboard.GetColorForPlayer(tileNumber));
        }
    }
    
}

