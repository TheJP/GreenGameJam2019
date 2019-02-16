using System.Collections;
using UnityEngine;

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

    public IEnumerator RollCoroutine()
    {
        finishedRollingParticles.SetActive(false);
        spriteRenderer.gameObject.SetActive(true);
        var waitPerRotation = initialWaitPerRotation;
        var start = Time.time;
        while (Time.time - start < rollDuration)
        {
            spriteRenderer.sprite = diceSprites[Random.Range(0, diceSprites.Length)];
            yield return new WaitForSeconds(waitPerRotation);
            waitPerRotation *= rollSlowdownFactor;
        }
        finishedRollingParticles.SetActive(true);
    }

    public void HideDie()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(RollCoroutine());
        }
    }
}
