using Assets.Scripts.Board;
using UnityEngine;

namespace Networking
{
    public class NetworkMap
        : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private NetworkTile networkTilePrefab;

        [SerializeField]
        private NetworkLine networkLinePrefab;

#pragma warning restore 649

        public int Height { get; set; }

        public int Width { get; set; }

        private NetworkTile[,] tiles;
        private (NetworkLine top, NetworkLine left)[,] lines;

        public Vector3 TopLeft => new Vector3(-Width / 2.0f, Height / 2.0f, 0);
        public Vector3 BottomLeft => TopLeft + Vector3.down * Height;
        public Vector3 TopRight => TopLeft + Vector3.right * Width;
        public Vector3 BottomRight => BottomLeft + Vector3.right * Width;

        private void Start()
        {
            var startPos = TopLeft + Vector3.down / 2 + Vector3.right / 2;

            tiles = new NetworkTile[Height, Width];
            for(var h = 0; h < Height; ++h)
            {
                for(var w = 0; w < Width; ++w)
                {
                    tiles[h, w] = Instantiate(networkTilePrefab, startPos + h * Vector3.down + w * Vector3.right,
                        Quaternion.identity);
                }
            }

            startPos -= Vector3.forward;
            lines = new (NetworkLine top, NetworkLine left)[Height + 1, Width + 1];
            for(var h = 0; h < Height + 1; ++h)
            {
                for(var w = 0; w < Width + 1; ++w)
                {
                    NetworkLine top = null;
                    NetworkLine left = null;
                    
                    if(w < Width)
                    {
                        top = Instantiate(
                            networkLinePrefab,
                            startPos + h * Vector3.down + w * Vector3.right + new Vector3(0, 0.5f),
                            Quaternion.identity);
                        
                        top.transform.Rotate(Vector3.forward, 90);
                    }

                    if(h < Height)
                    {
                        left = Instantiate(
                            networkLinePrefab,
                            startPos + h * Vector3.down + w * Vector3.right + new Vector3(-0.5f, 0),
                            Quaternion.identity);
                    }

                    lines[h, w] = (top, left);
                }
            }
        }

        public Vector3 GetEdgeVector(int x, int y)
        {
            return TopLeft + Vector3.right * x + Vector3.down * y;
        }

        public void CaptureLine(int x, int y, bool top, Player player)
        {
            var line = lines[y, x];

            if(top && !ReferenceEquals(line.top, null))
            {
                line.top.Owner = player;
                TryCaptureTile(x, y, player);
                TryCaptureTile(x, y - 1, player);
            }
            else if(!top && !ReferenceEquals(line.left, null))
            {
                line.left.Owner = player;
                TryCaptureTile(x, y, player);
                TryCaptureTile(x - 1, y, player);
            }
        }

        public int CountTilesOwnedBy(Player player)
        {
            var count = 0;
            for(var x = 0; x < Width; ++x)
            {
                for(var y = 0; y < Height; ++y)
                {
                    if(tiles[y, x].Owner == player)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }

        private void TryCaptureTile(int x, int y, Player player)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            if(lines[y, x].top.Owner == player
               && lines[y, x].left.Owner == player
               && lines[y + 1, x].top.Owner == player
               && lines[y, x + 1].left.Owner == player)
            {
                tiles[y, x].Owner = player;
            }
        }
    }
}