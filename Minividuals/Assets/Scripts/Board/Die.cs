using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class Die : MonoBehaviour
    {
        [Tooltip("Dice sprites in ascending order")]
        public Sprite[] diceSprites;

        [Tooltip("Renderer of the die")]
        public SpriteRenderer spriteRenderer;

        [Tooltip("Duration that a die roll will take from start to finish")]
        public float rollDuration = 3f;

        [Tooltip("Time between rotations of the die when it starts rolling.")]
        public float initialWaitPerRotation = 0.1f;

        [Tooltip("Factor by which the wait per rotation increases")]
        public float rollSlowdownFactor = 1.5f;

        [Tooltip("GameObject with particles that will be activated after the player rolled the die")]
        public GameObject finishedRollingParticles;

        [Tooltip("GameObject that is shown to animate the player to roll the die")]
        public GameObject rollHint;

        [Tooltip("Figure in the roll hint that displays the colour of the player that has to roll")]
        public PlayerFigure rollHintFigure;

        /// <summary>
        /// Result that the dice rolled. Make sure to wait until the <see cref="RollCoroutine"/> coroutine finished before reading the result.
        /// </summary>
        public int DieResult { get; private set; }

        public void PrepareRoll(Player player)
        {
            rollHint.SetActive(true);
            rollHintFigure.SetOwner(player);
            spriteRenderer.gameObject.SetActive(true);
        }

        public IEnumerator RollCoroutine()
        {
            rollHint.SetActive(false);
            finishedRollingParticles.SetActive(false);
            var waitPerRotation = initialWaitPerRotation;
            var start = Time.time;
            int currentIndex;
            do
            {
                currentIndex = Random.Range(0, diceSprites.Length);
                spriteRenderer.sprite = diceSprites[currentIndex];
                yield return new WaitForSeconds(waitPerRotation);
                waitPerRotation *= rollSlowdownFactor;
            } while (Time.time - start < rollDuration);
            finishedRollingParticles.SetActive(true);
            DieResult = currentIndex + 1;
        }

        public void HideDie()
        {
            spriteRenderer.gameObject.SetActive(false);
            rollHint.SetActive(false);
            finishedRollingParticles.SetActive(false);
        }
    }
}
