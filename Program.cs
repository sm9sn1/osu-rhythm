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
            if (args.Count() != 2)
            {
                Console.WriteLine("You done messed up. Use the following form when calling this executable:");
                Console.WriteLine("$<input file containing the map>");
            }
            SuffixTrie trie = new SuffixTrie(compress(buildArray(args[1])));

            //do the analysis
            //output results
        }

        static Note[] buildArray(string fileName)
        {
            string line;
            Note[] notes = {};
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null && line != "[HitObjects]")
                {
                }

                while ((line = file.ReadLine()) != null)
                {
                    string[] noteData = line.Split(',');
                    notes.append(new Note(Int32.Parse(noteData[2]), 1, Int32.Parse(noteData[3]) & 11));
                }
            }
            for (int i = notes.length; i > 0; i--)
            {
                notes[i - 1].duration = notes[i].duration - notes[i - 1].duration;
            }
            notes[notes.length - 1].duration = 0;
            return notes;
        }

        Note[] compress(Note[] notes) {
            for (int i = notes.length; i > 0; i--;) {
                if (notes[i] = notes[i - 1]) {
                    notes[i - 1].quantity = notes[i].quantity++;
                    notes.removeAt(i);
                }
            } 
        }
    }
}
