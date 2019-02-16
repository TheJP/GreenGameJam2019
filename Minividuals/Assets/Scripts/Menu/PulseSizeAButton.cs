using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(RectTransform))]
    public class PulseSizeAButton : MonoBehaviour
    {
        [Tooltip("Duration of one pulse animation")]
        public float animationCycle = 3f;

        [Tooltip("Factor by which button size is increased to pulse")]
        public float scaleFactor = 1.2f;

        private RectTransform rectTransform;
        private float start;
        private Vector2 startSize;
        private Vector2 targetSize;

        private void Awake() => rectTransform = GetComponent<RectTransform>();

        private void Start()
        {
            start = Time.time;
            startSize = rectTransform.sizeDelta;
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
                rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, time / halfCycle);
            }
            else
            {
                rectTransform.sizeDelta = Vector2.Lerp(targetSize, startSize, (time - halfCycle) / halfCycle);
            }
        }
    }
}
