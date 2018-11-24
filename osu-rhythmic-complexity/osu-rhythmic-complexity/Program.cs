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
                Console.WriteLine("Use the following form when calling this executable:");
                Console.WriteLine("<this exe's name> <map's filename, must be .osu> $<output filename, including extension>");
                Console.ReadKey();
                return;
            }
            SuffixTrie trie = new SuffixTrie(compress(buildArray(args[0])));
            List<List<Note>> subsequences = new List<List<Note>>();
            //do the analysis
            trie.getSubSequences(subsequences);
            //sort the list but subsequence length
            subsequences.Sort((a, b) => a.Count.CompareTo(b.Count));
            //output results
            using (StreamWriter outfile = new StreamWriter(args[1]))
            {
                printSubsequences(subsequences, outfile);
                printStats(subsequences, outfile);
            }
            Console.ReadKey();
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
                //1 note per line, comma delineated attributes
                while ((line = file.ReadLine()) != null)
                {
                    string[] noteData = line.Split(',');
                    //note type is stored as a bitmap
                    notes.Add(new Note(Int32.Parse(noteData[2]), 1, (NoteType)(Int32.Parse(noteData[3]) & 11)));
                }
            }
            //timing value in file is milliseconds since map start
            for (int i = notes.Count - 1; i > 0; i--)
            {
                //calculate time until next note
                notes[i].duration = notes[i].duration - notes[i - 1].duration;
            }
            //last note has no next note
            notes[notes.Count - 1].duration = 0;
            notes.Add(new Note(-1, -1, NoteType.circle));
            return notes;
        }

        static List<Note> compress(List<Note> notes) {
            for (int i = notes.Count - 1; i > 0; i--) {
                //if 2 sequential notes are equal, group them together
                if (Math.Abs(notes[i].duration - notes[i - 1].duration) <= 2 && notes[i].type == notes[i - 1].type) {
                    notes[i - 1].quantity += notes[i].quantity;
                    notes.RemoveAt(i);
                }
            }
            return notes;
        }

        private static void printSubsequences(List<List<Note>> subsequences, StreamWriter outfile)
        {
            foreach (List<Note> seq in subsequences)
            {
                outfile.Write(seq[seq.Count - 1].quantity + "/");
                outfile.WriteLine(seq.ToString());
            }
        }

        private static void printStats(List<List<Note>> subsequences, StreamWriter outfile)
        {
            double average = 0;
            foreach (List<Note> seq in subsequences)
            {
                average += seq.Last().quantity;
            }
            average /= subsequences.Count;
            outfile.WriteLine("Average # for each subsequence: " + average);
            outfile.WriteLine("Total subsequences: " + subsequences.Count);
        }
    }
}

/*
 TODO:
    bugfixing
        findpath out of bounds issue 
            start += i = s.Count, not sure if this is the incorrect state 
            or if it is a math error

Done:
 the compression issue has 2 cases afaik
 1. the compressed note is the only note the subsequence, this
    is where the frequency needs to be counted to reflect the number 
    of times that note exists in the map (note.quantity + node.frequency)
 2. the compressed note isn't the only note in the subsequence,
    the trie currently doesn't care about quantity at all during construction 
    so it will think notes are the same when they arent. a fix for this might 
    be as simple as adding quantity as an equality condition

 get several sample map files to test with
 maybe revert the generic versions of trie and node
    they may prove more trouble then they are worth during testing
 write a note.ToString() to make output readable
    I would like to use terms of quarter/half/etc notes but songs
    can have sections at different bpms, and ms is enough to 
    calculate map difficulty anyway
 write results to a file instead of the cli

Abandoned:
 rewrite the suffixtrie code to use lists instead of linked 
 lists
    this is probably not worthwhile unless i have to redo it anyway
 switch from compressed suffix trie to uncompressed
    to track the frequency of each note properly
    currently, each node and has a range of notes under it, and 
    the current trie doesn't know about the identical note 
    compression. it would be very hard to add this directly
 unify the note and node classes
    continuation of the first todo, may be easier to have a node 
    wrapper around exactly 1 note (uncompressed)
 abandon the compression and find some other way to remove 
    the noise from identical subsequences
    this would involve adding a special terminal note somehow,
    maybe i should try it without compression and see if its
*/
