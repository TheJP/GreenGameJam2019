using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace MelodyMemory
{
    public class ColorSoundTile : MonoBehaviour
    {   
        public event Action TileClickEvent;
        
        [Tooltip("Renderer which allows to set the colour of the tile")]            
        public MeshRenderer colourRenderer;

#pragma warning disable 649
        [Tooltip("Color for tiles that are inactive")]
        [SerializeField] private Color defaultColor;

        [Tooltip("Default soundclip, will by dynamically replaced")]
        [SerializeField] private AudioSource sound;
#pragma warning restore 649

        private Note note;     // only set on some tiles  

        private String myName;

        private bool listening;    // if false, it will not react to clicks TODO isn't there something built-in for that?   

        public int tileIndex { get; set; }

        private void Start()
        {
            myName = GetComponent<Renderer>().name;

            sound = GetComponent<AudioSource>();
            
            SetColor(defaultColor);
        }

        public void SetNote(NoteWithPosition noteWPos)
        {            
            if (noteWPos == null)
            {
                note = null;
                sound.clip = null;
            }
            else
            {
                note = noteWPos.Note;

                String audioPath = $"Sounds/MelodyMemory/{note.AudioSourceName}";
                AudioClip newClip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
                sound.clip = newClip;
            }

            
        }

        /// <summary>
        /// Enables the tile (but only if it has a note)
        /// </summary>
        /// <param name="listening"></param>
        public void setListening(bool listening)
        {
            // a tile without note will never listen
            if (note != null)
            {
                this.listening = listening;
                Debug.Log($"enabled listening for tile {name}");
            }
                
        }
        
        
        void Update()
        {
            if (!listening)   return;
            
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
                            // Debug.Log($"Clicked tile {obj.name}, I am {myName}");
                            StartCoroutine("BlinkColor");
                        }
                    }
                }
            }
        
        }

        public void ResetColor()
        {
            colourRenderer.material.color = defaultColor;
        }
        
        public void SetColor(Color color)
        {
            colourRenderer.material.color = color;
        }
    
        IEnumerator BlinkColor() 
        {
            SetColor(note.Color);

            // TODO enable sound again
            sound.Play();
            yield return new WaitForSeconds(1.0f);

            ResetColor();
            
            TileClickEvent?.Invoke();
        }


    }
}

   



