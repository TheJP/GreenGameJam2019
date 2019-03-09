using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace MelodyMemory
{
    public class RestartButtonScript : MonoBehaviour
    {

#pragma warning disable 649

        [SerializeField] private Cursor cursor;
        
        [SerializeField] private GameObject restartIconObject;

#pragma warning restore 649
        
        public event Action ClickEvent;

        private SpriteRenderer iconRenderer;
        

        public void Setup()
        {
            iconRenderer = restartIconObject.GetComponentInChildren<SpriteRenderer>();            
        }
        
        void Update()
        {
            // change here (button and ray) and in ColorSoundTile to play with mouse instead of controller
            if (isActiveAndEnabled && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
//            if (isActiveAndEnabled && Input.GetButtonDown($"{cursor.ControlPrefix}{InputSuffix.A}"))
            {   
                // change here to play with mouse instead of controller
//                Ray ray = cursor.GetRay();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.CompareTag("Button"))
                    {                        
                        // clicking deactivates the button
                        ClickEvent?.Invoke();
                    }
                }
            }
        
        }
        
        public void Show()
        {
            if (iconRenderer != null)
                iconRenderer.color = Color.white;
        }
    
        public void Hide()
        {
            if (iconRenderer != null)
                iconRenderer.color = Color.grey;
        }

    }    
}

