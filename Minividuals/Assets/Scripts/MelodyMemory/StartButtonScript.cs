using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;

namespace MelodyMemory
{
    public class StartButtonScript : MonoBehaviour
    {
        
        public event Action ClickEvent;
        
        private bool active = false;

        void Start()
        {
            // myName = GetComponent<Renderer>().name;
        }
        
        void Update()
        {
            if (active && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {            
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.CompareTag("Button"))
                    {
                        // clicking deactivates the button
                        this.active = false;
                        ClickEvent?.Invoke();
                    }
                }
            }
        
        }

        public void setActive(bool active)
        {
            this.active = active;
        }

    }    
}

