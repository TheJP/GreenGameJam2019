using System.Collections.Generic;
using Assets.Scripts.Board;
using UnityEngine;

namespace Networking
{
    public class NetworkTile
        : MonoBehaviour
    {
        private Player owner;

        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
                UpdateColor();
            }
        }

        private IEnumerable<MeshRenderer> meshRenderers;

        private void Start()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }

        private void UpdateColor()
        {
            foreach(var meshRenderer in meshRenderers)
            {
                meshRenderer.material.color = owner.Colour;
            }
        }
    }
}