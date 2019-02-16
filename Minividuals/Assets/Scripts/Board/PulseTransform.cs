using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class PulseTransform : MonoBehaviour
    {
        [Tooltip("Duration of one pulse animation")]
        public float animationCycle = 3f;

        [Tooltip("Factor by which button size is increased to pulse")]
        public float scaleFactor = 1.2f;

        private Transform pulseTransform;
        private float start;
        private Vector2 startSize;
        private Vector2 targetSize;

        private void Awake() => pulseTransform = GetComponent<Transform>();

        private void Start()
        {
            start = Time.time;
            startSize = pulseTransform.localScale;
            targetSize = startSize * scaleFactor;
        }

        // Update is called once per frame
        private void Update()
        {
            var halfCycle = animationCycle / 2f;
            var time = Time.time - start;
            if (time > animationCycle)
            {
                start += animationCycle * (int)(time / animationCycle);
                time = Time.time - start;
            }

            if (time < halfCycle)
            {
                pulseTransform.localScale = Vector2.Lerp(startSize, targetSize, time / halfCycle);
            }
            else
            {
                pulseTransform.localScale = Vector2.Lerp(targetSize, startSize, (time - halfCycle) / halfCycle);
            }
        }
    }
}
