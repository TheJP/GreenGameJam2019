using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class mouseScript : MonoBehaviour
{
    
    public AudioSource sound;

    void Update()
    {
        if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log($"This is a Player, ray is {ray}");
                    StartCoroutine("blinkColor");

                    //showColor(Color.green);
                    
                }
                else
                {
                    Debug.Log("This isn't a Player");
                }
            }
        }
        
    }

    void showColor(Color color)
    {
        GetComponent<Renderer> ().material.color = Color.green;
    }

    void removeColor()
    {
        GetComponent<Renderer> ().material.color = Color.white;
    }
    
    IEnumerator blinkColor() 
    {
        GetComponent<Renderer> ().material.color = Color.green;
        sound.Play();
        yield return new WaitForSeconds(1.0f);
//        for (float f = 1f; f >= 0; f -= 0.1f) 
//        {
//            Debug.Log($"f is {f}");
////            Color c = GetComponent<Renderer> ().material.color;
////            c.a = f;
////            GetComponent<Renderer> ().material.color = c;
//            yield return new WaitForSeconds(.1f);
//        }
        GetComponent<Renderer> ().material.color = Color.white;
    }
}
