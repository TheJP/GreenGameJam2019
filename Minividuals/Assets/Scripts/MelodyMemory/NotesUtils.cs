using System;
using System.Collections.Generic;
using UnityEngine;

namespace MelodyMemory
{
    // there should be only one instance of NotesUtils, implement as singleton
    public class NotesUtils
    {
        private static NotesUtils nu;

        public static NotesUtils GetInstance()
        {
            if (nu == null)
                nu = new NotesUtils();
            return nu;
        } 

        // Stores all available notes and their order. 
        private Dictionary<int, Note> noteTable;
        
        private NotesUtils ()
        {
            noteTable = new Dictionary<int, Note>();
    
            // first octave
            noteTable.Add(0, new Note(Octave.O4, NoteName.C));
            noteTable.Add(1, new Note(Octave.O4, NoteName.D));
            noteTable.Add(2, new Note(Octave.O4, NoteName.E));
            noteTable.Add(3, new Note(Octave.O4, NoteName.F));
            noteTable.Add(4, new Note(Octave.O4, NoteName.G));
            noteTable.Add(5, new Note(Octave.O4, NoteName.A));
            noteTable.Add(6, new Note(Octave.O4, NoteName.H));
    
            // second octave
            noteTable.Add(7, new Note(Octave.O5, NoteName.C));
            noteTable.Add(8, new Note(Octave.O5, NoteName.D));
            noteTable.Add(9, new Note(Octave.O5, NoteName.E));
            noteTable.Add(10, new Note(Octave.O5, NoteName.F));
            noteTable.Add(11, new Note(Octave.O5, NoteName.G));
            noteTable.Add(12, new Note(Octave.O5, NoteName.A));
            noteTable.Add(13, new Note(Octave.O5, NoteName.H));
    
            // first note of third octave - would be nice...
            // mixTable.Add(14, new Note(Octave.O6, NoteName.C));
    
        }

        public Note GetRandomNote()
        {
            int randomKey = UnityEngine.Random.Range(0, noteTable.Count);
            return noteTable[randomKey];
        }

        public static Color ColorForNote(NoteName noteName)
        {
            switch (noteName)
            {
                case NoteName.C:
                    return Color.red;
                case NoteName.D:
                    return new Color(1, 0xa0 / 255.0f, 0);    // Orange
                case NoteName.E:
                    return Color.yellow;
                case NoteName.F:
                    return Color.green;
                case NoteName.G:
                    return new Color(0, 0x99 / 255.0f, 0x99 / 255.0f);    // darker green
                case NoteName.A:
                    return Color.blue;
                case NoteName.H:
                    return new Color(0x88 / 255.0f, 0, 1);    // Violet
                default:
                    return Color.black;
            }
        }

        
        public static String OctaveToString(Octave value)
        {
            String s = value.ToString();
            return s.Substring(s.Length - 1);
        }
    }    
}
