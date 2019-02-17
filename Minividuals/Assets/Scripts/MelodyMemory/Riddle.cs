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
        
        // First list of unsorted random notes (as they will be played when the riddle starts).
        // Then it will be copied into melodyOnBoard and sorted (in ascending note pitch order!)  
        private List<Note> randomNotes;

        // list of notes with positions, so it can be played initially on the board (same notes as randomNotes, but with positions) 
        private List<NoteWithPosition> melodyOnBoard;
        
        // dictionary to lookup positions of the notes (which tile number has a note?)
        private Dictionary<int, NoteWithPosition> notePositions = new Dictionary<int, NoteWithPosition>();

        private int waitingForPosition = 0;    // index into randomNotes (after it has been sorted)
        
        
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
        /// Set each note to a random position within the given positions.
        /// Also create the expected sorted version and start "listening" for the solution.  
        /// </summary>
        private void SetToPositions()
        {
            Debug.Log(($"Setting melody to board (length {length}:"));
            
            melodyOnBoard = new List<NoteWithPosition>();
            foreach (var note in randomNotes)
            {
                int pos = UnityEngine.Random.Range(0, numPositions - 1);
                if (!notePositions.ContainsKey(pos))
                {
                    NoteWithPosition newNote = new NoteWithPosition(note, pos);
                    notePositions.Add(pos, newNote);
                    melodyOnBoard.Add(newNote);
                    Debug.Log(($"- added to melody: {newNote}"));
                }
            }
            
            // and also sort the melody for the solution
            randomNotes.Sort();
            Debug.Log($"sorted notes:");
            foreach (var note in randomNotes)
            {
                Debug.Log($"- note {note}");
            }
            waitingForPosition = 0;    // index into sortedNotes - we expect the first one now

        }
        
        public NoteWithPosition GetNoteAtPosition(int position)
        {
            NoteWithPosition note = null;
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

        public List<NoteWithPosition> GetRiddleMelody()
        {
            return this.melodyOnBoard;
        }
        
        
        // --- for solving the riddle, we are "listening" and tracking what has been clicked ---------

        /// <summary>
        /// Registers the clicked tile and updates "state machine" accordingly.
        /// </summary>
        /// <param name="position">position of clicked tile</param>
        /// <returns>true if game is won</returns>
        public bool hearTile(int position)
        {
            Note heardNote = notePositions[position].Note;
            Note expectedNote = randomNotes[waitingForPosition];
            if (heardNote.Equals(expectedNote))
            {
                waitingForPosition++;
                Debug.Log($"correctly heard note {heardNote}, number {waitingForPosition} of melody");
            }
            else
            {
                Debug.Log($"that was a wrong note {heardNote}, resetting expected pos to 0");
                waitingForPosition = 0;
            }
            return (waitingForPosition == length);
        }
        
        
        // -------------------------------------------------------------------------------------------
        
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

