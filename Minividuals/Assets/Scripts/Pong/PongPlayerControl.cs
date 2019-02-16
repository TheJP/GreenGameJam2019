using UnityEngine;

public class PongPlayerControl : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private float speed;
#pragma warning restore 649

    public string ControlPrefix { get; set; }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis($"{ControlPrefix}Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.Translate(movement * Time.deltaTime * speed);
    }
}