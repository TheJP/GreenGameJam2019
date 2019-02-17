using System;
using Assets.Scripts.Board;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayer
        : MonoBehaviour
    {
        public Player Player { get; set; }
        
        public NetworkMap NetworkMap { get; set; }
        
        public (int x, int y) Location { get; set; }
        
        public Vector3 Direction { get; set; }

        public float Speed { get; set; } = 1;

        private Vector3 nextDecisionPoint;
        private float percentOfDirection;
        
        private void Start()
        {
            foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material.color = Player.Colour;
            }
            
            var pos = transform.position = NetworkMap.GetEdgeVector(Location.x, Location.y) + Vector3.back * 3;
            nextDecisionPoint = pos + Direction;
        }

        private void Update()
        {
            percentOfDirection += Time.deltaTime * Speed;
            transform.position = Vector3.Lerp(nextDecisionPoint - Direction, nextDecisionPoint, percentOfDirection);

            if(percentOfDirection >= 1)
            {
                CaptureLine();

                Location = (Location.x + (int)Direction.x, Location.y - (int)Direction.y);
                
                var vertical = Input.GetAxis(Player.InputPrefix + "Vertical");
                if(Mathf.Abs(vertical) < 0.2f)
                {
                    vertical = 0;
                }
                
                var horizontal = Input.GetAxis(Player.InputPrefix + "Horizontal");
                if(Mathf.Abs(horizontal) < 0.2f)
                {
                    horizontal = 0;
                }

                var nextX = Location.x;
                var nextY = Location.y;
                var nextDirection = Vector3.zero;

                if(Math.Abs(vertical) < 0.2f && Mathf.Abs(horizontal) < 0.2f)
                {
                    if(Direction == Vector3.up && nextY - 1 >= 0)
                    {
                        --nextY;
                        nextDirection = Vector3.up;
                    }
                    else if(Direction == Vector3.down && nextY + 1 <= NetworkMap.Height)
                    {
                        ++nextY;
                        nextDirection = Vector3.down;
                    }
                    else if(Direction == Vector3.right && nextX + 1 <= NetworkMap.Width)
                    {
                        ++nextX;
                        nextDirection = Vector3.right;
                    }
                    else if(Direction == Vector3.left && nextX - 1 >= 0)
                    {
                        --nextX;
                        nextDirection = Vector3.left;
                    }
                }
                else if(Mathf.Abs(vertical) > Mathf.Abs(horizontal))
                {
                    // We try to go vertical
                    if(vertical < 0 && nextY + 1 <= NetworkMap.Height)
                    {
                        ++nextY;
                        nextDirection = Vector3.down;
                    }
                    else if(vertical > 0 && nextY - 1 >= 0)
                    {
                        --nextY;
                        nextDirection = Vector3.up;
                    }
                }
                else
                {
                    // We try to go horizontal
                    if(horizontal < 0 && nextX - 1 >= 0)
                    {
                        --nextX;
                        nextDirection = Vector3.left;
                    }
                    else if(horizontal > 0 && nextX + 1 <= NetworkMap.Width)
                    {
                        ++nextX;
                        nextDirection = Vector3.right;
                    }
                }

                Direction = nextDirection;
                nextDecisionPoint = NetworkMap.GetEdgeVector(nextX, nextY) + Vector3.back * 3;
                percentOfDirection = Direction != Vector3.zero ? 0 : 1;
            }
        }

        private void CaptureLine()
        {
            if(Direction == Vector3.up)
            {
                NetworkMap.CaptureLine(
                    Location.x,
                    Location.y - 1,
                    false,
                    Player);
            }
            else if(Direction == Vector3.left)
            {
                NetworkMap.CaptureLine(
                    Location.x - 1,
                    Location.y,
                    true,
                    Player);
            }
            else if(Direction != Vector3.zero)
            {
                NetworkMap.CaptureLine(
                    Location.x,
                    Location.y,
                    Direction == Vector3.right,
                    Player);
            }
        }
    }
}