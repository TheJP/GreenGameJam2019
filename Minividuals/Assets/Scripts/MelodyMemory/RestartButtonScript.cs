using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;

namespace MelodyMemory
{
    public class RestartButtonScript : MonoBehaviour
    {

#pragma warning disable 649

        [SerializeField] private Cursor cursor;

#pragma warning restore 649
        
        public event Action ClickEvent;
        
        private bool active = false;

        
        void Update()
        {
            if (active && Input.GetButtonDown($"{cursor.ControlPrefix}{InputSuffix.A}"))
            {
                Ray ray = cursor.GetRay();
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

