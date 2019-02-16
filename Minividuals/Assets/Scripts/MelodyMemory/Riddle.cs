using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MelodyMemory
{
    public class Riddle
    {
        private NotesUtils notesUtils = NotesUtils.GetInstance();
        
        private int length;
        private int numPositions;
        private List<Note> randomNotes;
        private Dictionary<int, Note> notePositions = new Dictionary<int, Note>();
    
        // objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="len">length of riddle (length of melody)</param>
        /// <param name="numTiles">number of positions (tiles)</param>
        public Riddle (int len, int numTiles)
        {
            length = len;
            numPositions = numTiles;
            randomNotes = InitRandomNotes(length);
            Debug.Log($"random notes initialized, riddle is : {this.ToString()}");
            
            SetToPositions();
            Debug.Log($"random notes positioned, riddle is : {this.ToString()}");
        }

        private List<Note> InitRandomNotes(int length)
        {
            List<Note> rNotes = new List<Note>();
            while (rNotes.Count < length)
            {
                Note randomNote = notesUtils.GetRandomNote();
                if (!rNotes.Contains(randomNote))
                    rNotes.Add(randomNote);
            }
            Debug.Log($"random notes initialized");
            return rNotes;
        }

        /// <summary>
        /// set each note to a random position within the given positions 
        /// </summary>
        private void SetToPositions()
        {
            foreach (var note in randomNotes)
            {
                int pos = UnityEngine.Random.Range(0, numPositions - 1);
                if (!notePositions.ContainsKey(pos))
                    notePositions.Add(pos, note);
            }
        }
        
        public Note GetNoteAtPosition(int position)
        {
            Note note = null;
            if (notePositions.TryGetValue(position, out note))
            {
                Console.WriteLine($"no note at position {position}");
            }
            else
            {
                Console.WriteLine($"note at position {position}: {note}");
            }
            return note;
        }

        public override string ToString()
        {
            String s = $"Riddle ({length}):";
            foreach (var note in randomNotes)
            {
                String noteString = $", {note}";
                s = s + noteString;
            }
            return s;
        }
    }
}

