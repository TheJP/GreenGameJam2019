using UnityEngine;

public class Tiles : MonoBehaviour
{
    private const int TileCount = 48;

    public Camera boardCamera;
    public Tile tilePrefab;
    public Transform tilesParent;

    private readonly Tile[] tiles = new Tile[TileCount];

    private int width;
    private int height;

    private void Start()
    {
        for (int i = 0; i < TileCount; ++i)
        {
            tiles[i] = Instantiate(tilePrefab, tilesParent);
        }
        UpdateTilePositions();
    }

    private void Update()
    {
        if (width != Screen.width || height != Screen.height)
        {
            UpdateTilePositions();
        }
    }

    private void UpdateTilePositions()
    {
        width = Screen.width;
        height = Screen.height;

        var heightUnits = boardCamera.orthographicSize * 2.0f;
        var widthUnits = heightUnits * boardCamera.aspect;

        var tileWidth = boardCamera.pixelWidth / (TileCount / 4);
        var tileWidthUnits = widthUnits / (TileCount / 4);

        for (int i = 0; i < TileCount; ++i)
        {
            if (i < TileCount / 4)
            {
                var position = boardCamera.ScreenToWorldPoint(new Vector3(i * tileWidth, boardCamera.pixelHeight, 0));
                position.z = 0;
                tiles[i].transform.position = position;
                tiles[i].transform.localScale = new Vector3(tileWidthUnits, 1, 1);
            }
            else if (i >= TileCount / 2 && i < 3 * TileCount / 4)
            {
                var position = boardCamera.ScreenToWorldPoint(new Vector3(((3 * TileCount / 4) - i - 1) * tileWidth, 0, 0));
                position.z = 0;
                position.y += 1;
                tiles[i].transform.position = position;
                tiles[i].transform.localScale = new Vector3(tileWidthUnits, 1, 1);

            }
        }
    }
}
