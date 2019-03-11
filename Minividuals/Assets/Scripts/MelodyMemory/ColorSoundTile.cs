using System;
using System.Collections;
using System.Runtime.InteropServices;
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

        public Cursor cursor { get; set; }       
        public int tileIndex { get; set; }

        private Note note;           // only set on some tiles  
        private String myName;
        private bool listening;      // if false, it will not react to clicks and to the sound being played
        private float blinkDuration = 1.0f;     // sounds have length 1 second, best to change tile color for the same duration
        
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
        /// Disables the tile, or enables it (but only if it has a note)
        /// </summary>
        /// <param name="listen"></param>
        public void setListening(bool listen)
        {
            if (!listen)
            {
                enabled = false;
                listening = false;
            }
            else if (note != null)    // a tile without note will never listen
            {
                enabled = true;
                listening = true;
            }
                
        }
        
        
        void Update()
        {
            if (!listening)   return;            
            // no need to listen for mouse if this tile has no note!
            
            // change here (button and ray) and in RestartButtonScript to play with mouse instead of controller
            if (note != null && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
//            if (note != null && Input.GetButtonDown($"{Cursor.ControlPrefix}{InputSuffix.A}"))
            {
//                Ray ray = Cursor.GetRay();
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
    
        // show note color for the specified duration and play the note 
        IEnumerator BlinkColor() 
        {
            SetColor(note.Color);
            sound.Play();
            if (listening)
            {
                TileClickEvent?.Invoke();
            }

            yield return new WaitForSeconds(blinkDuration);
            ResetColor();
        }


    }
}

   



