using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace MelodyMemory
{
    public class ColorSoundTile : MonoBehaviour
    {           
        [Tooltip("Renderer which allows to set the colour of the tile")]            
        public MeshRenderer colourRenderer;

#pragma warning disable 649
        [Tooltip("Color for tiles that are inactive")]
        [SerializeField] private Color defaultColor;

        [Tooltip("Default soundclip, maybe not used")]
        [SerializeField] private AudioSource sound;
#pragma warning restore 649

        private Note note;     // only set on some tiles  


        private String myName;

        public int tileIndex { get; set; }

        private void Start()
        {
            myName = GetComponent<Renderer>().name;
            
            SetColor(defaultColor);
        }

        public void setNote(Note note)
        {
            this.note = note;

            AudioClip clip = null;
            // AudioClip clip = load AudioSource from note.AudioSourceName
            if (clip != null)
                sound.clip = clip;
            
        }
        
        
        void Update()
        {
            // no need to listen for mouse if this tile has no note!
            if (note != null && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        GameObject obj = hit.collider.gameObject;
                        if (obj.name.Equals(myName))
                        {
                            Debug.Log($"Clicked tile {obj.name}, I am {myName}");
                            
                            StartCoroutine("BlinkColor");
                        }
                        
                        //showColor(Color.green);
                    
                    }
                    else
                    {
                        Debug.Log("This isn't a Player");
                    }
                }
            }
        
        }
        
        private void SetColor(Color color)
        {
            colourRenderer.material.color =  color;
        }
    
        IEnumerator BlinkColor() 
        {
            SetColor(note.Color);

            // TODO enable sound again
            // sound.Play();
            yield return new WaitForSeconds(1.0f);

            SetColor(defaultColor);
        }


    }
}

   



