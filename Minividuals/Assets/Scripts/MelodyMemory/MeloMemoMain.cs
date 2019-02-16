using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    public class MeloMemoMain : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject tilePrefab;
#pragma warning restore 649

        // Start is called before the first frame update
        private void Start()
        {
            GameObject tileObject = Instantiate(tilePrefab);
            ColorSoundTile tile = tileObject.GetComponent<ColorSoundTile>();
            for (int tileNumber = 0; tileNumber < 32; tileNumber++)
            {
                InstantiateTile(tile, tileNumber);
            }
        }

        private void InstantiateTile(ColorSoundTile tile, int tileNumber)
        {
            
//            PlayerControls playerControls = playerObject.GetComponent<PlayerControls>();
//            playerControls.SetPlayerNumber(tileNumber);

//            PlayerStyle playerStyle = playerObject.GetComponent<PlayerStyle>();
//            playerStyle.SetColor(playboard.GetColorForPlayer(tileNumber));
        }
    }
    
}

