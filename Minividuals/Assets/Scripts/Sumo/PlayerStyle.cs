using UnityEngine;

public class PlayerStyle : MonoBehaviour
{
    [SerializeField] private Color playerColor;

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = playerColor;
    }
}