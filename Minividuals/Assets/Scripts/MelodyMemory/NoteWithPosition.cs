using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MelodyMemory
{
    [Serializable]
    public class NoteWithPosition : IEquatable<NoteWithPosition>,  IComparable<NoteWithPosition>
    {
        [SerializeField] private Note note;

        [SerializeField] private int position;

        public NoteWithPosition(Note note, int position)
        {
            this.note = note;
            this.position = position;
        }
   	

        public Note Note
        {
            get { return note; }
        }

        public int Position
        {
            get { return position; }
        }
    
        public override string ToString()
        {
            return $"NoteWithPos {note.FullName}@{position}";
        }

    
        public bool Equals(NoteWithPosition other)
        {
            if (other == null) return false;
            return position == other.position && note == other.note;
        } 
    
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as NoteWithPosition);
        }

        // just sort the notes, we do not care about positions
        public int CompareTo(NoteWithPosition other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;
        
            return this.Note.CompareTo(other.Note);
        }

    
        public override int GetHashCode()
        {
            return 112 * note.GetHashCode() + position.GetHashCode();
        }

    }
}

