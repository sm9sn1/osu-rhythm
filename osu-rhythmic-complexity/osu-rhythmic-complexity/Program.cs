using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ppcalc
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: use CLI for parameters instead of argv
            if (args.Count() != 2)
            {
                Console.WriteLine("You done messed up. Use the following form when calling this executable:");
                Console.WriteLine("$<input file containing the map>");
            }
            SuffixTrie<Note> trie = new SuffixTrie<Note>(compress(buildArray(args[1])));

            //do the analysis
            //output results
        }

        static List<Note> buildArray(string fileName)
        {
            string line;
            List<Note> notes = new List<Note>();
            using (StreamReader file = new StreamReader(fileName))
            {
                //skip past metadata to notes section
                while ((line = file.ReadLine()) != null && line != "[HitObjects]")
                {
                }
                //1 notes per line, comma delineated
                while ((line = file.ReadLine()) != null)
                {
                    string[] noteData = line.Split(',');
                    //note type is stored as a bitmap
                    notes.Add(new Note(Int32.Parse(noteData[2]), 1, (NoteType)(Int32.Parse(noteData[3]) & 11)));
                }
            }
            //timing value in file is milliseconds since map start
            for (int i = notes.Count; i > 0; i--)
            {
                //calculate time until next note
                notes[i - 1].duration = notes[i].duration - notes[i - 1].duration;
            }
            //last note has no next note
            notes[notes.Count - 1].duration = 0;
            return notes;
        }

        static List<Note> compress(List<Note> notes) {
            for (int i = notes.Count - 1; i > 0; i--) {
                //if 2 sequential notes are equal, group them together
                if (notes[i] == notes[i - 1]) {
                    notes[i - 1].quantity += notes[i].quantity;
                    notes.RemoveAt(i);
                }
            }
            return notes;
        }
    }
}
