using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTitleScreen : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float fadeoutSpeed;
#pragma warning restore 649

    private Text text;
    private RectTransform rectTransform;

    private void Awake()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(String text)
    {
        this.text.text = text;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeoutEffect());
    }

    private IEnumerator FadeoutEffect()
    {
        while (text.fontSize > 30)
        {
            text.fontSize = text.fontSize - (int) (fadeoutSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(transform.parent.gameObject);
    }
}