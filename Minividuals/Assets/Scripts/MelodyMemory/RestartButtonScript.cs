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
            Debug.Log($"RestartButtonScript.Setup: cursor is {cursor}");
        }
        
        void Update()
        {
            if (!isActiveAndEnabled) return;
            
            // TODO why are we here even if MeoMemoBoardController.DisableControls sets restartButton.enabled = false; ??
//            Debug.Log($"RestartButtonScript: isActiveAndEnabled: {isActiveAndEnabled} and enabled: {enabled}");
            
            // change here (button and ray) and in ColorSoundTile to play with mouse instead of controller
//            if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            if (Input.GetButtonDown($"{cursor.ControlPrefix}{InputSuffix.A}"))
            {   
//                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Ray ray = cursor.GetRay();
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.CompareTag("Button"))
                    {
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

