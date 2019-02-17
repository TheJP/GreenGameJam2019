using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MelodyMemory
{
    [Serializable]
    public class Note : IEquatable<Note>,  IComparable<Note>
    {
        [SerializeField] private Octave oct;

        [SerializeField] private NoteName name;

        public Note(Octave oct, NoteName name)
        {
            this.oct = oct;
            this.name = name;
        }

   	

        public Octave Octave
        {
            get { return oct; }
            set { oct = value; }
        }

        public String FullName
        {
            get { return $"{name.ToString()}{NotesUtils.OctaveToString(oct)}"; }
        }

        public Color Color
        {
            get { return NotesUtils.ColorForNote(name) ; }
        }

        public String AudioSourceName
        {
            get { return $"flute-{name.ToString()}{NotesUtils.OctaveToString(oct)}_1sec" ; }
        }
    
        public override string ToString()
        {
            return $"Note {FullName}";
        }

    
        public bool Equals(Note other)
        {
            if (other == null) return false;
            return oct == other.oct && name == other.name;
        } 
    
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Note);
        }

    
        public int CompareTo(Note other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;

            if (this.Octave.Equals(other.Octave))
                return this.name.CompareTo(other.name);
        
            return this.Octave.CompareTo(other.Octave);
        }

    
        public override int GetHashCode()
        {
            return 100 * oct.GetHashCode() + name.GetHashCode();
        }

    }
}

