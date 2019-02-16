using UnityEngine;

/// <summary>
/// Stolen from GreenGameJam2018 :) slightly modified...
/// Used to display a text, that slowly fades out.
///
/// Usage:
/// var object = Instantiate(countdownUiPrefab); //Instantiate a prefabObject of the FadeoutText prefab.
/// var script = object.GetComponent&lt;Countdown&gt;(); //Get the CountdownScript from it.
/// script.TextMesh.text = "textToDisplay"; //Set text you want to display
/// script.TextMesh.color = new Color(1.0F, 0.0F, 0.0F, 1.0F); //Set Text color.
/// 
/// </summary>
public class Countdown : MonoBehaviour
{
    public float flyUpSpeed = 2f;
    public float fadeoutDuration = 2f;

    private float startTime;
    private Color startColour;
    private Color targetColour;
    public TextMesh TextMesh { get; private set; }

    private void Awake()
    {
        TextMesh = GetComponentInChildren<TextMesh>();
    }

    private void Start()
    {
        startTime = Time.time;
        startColour = TextMesh.color;
        targetColour = new Color(startColour.r, startColour.g, startColour.b, 0f);
        Destroy(gameObject, fadeoutDuration);
    }

    private void Update()
    {
        transform.position += Vector3.up * flyUpSpeed * Time.deltaTime;
        TextMesh.color = Color.Lerp(startColour, targetColour, (Time.time - startTime) / fadeoutDuration);
    }
}