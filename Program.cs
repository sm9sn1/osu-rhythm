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
        //static StreamWriter sw;

        static void Main(string[] args)
        {
            //StreamWriter sw = new StreamWriter("local_BRCA2.txt");
            if (args.Count() != 3)
            {
                Console.WriteLine("You done messed up. Use the following form when calling this executable:");
                Console.WriteLine("$ <test executable> <input file containing sequence s> <input alphabet file>");
                return;
            }
            SuffixTrie trie = new SuffixTrie(getInput(args[1]), getAlphabet(args[2]));

            //do the things
        }

        static string getInput(string fileName)
        {
            string line;
            string retVal = "";
            using (StreamReader file = new StreamReader(fileName))
            {
                Console.Write(fileName + "\n");
                while ((line = file.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        break;
                    }
                    else if (line[0] != '>')
                    {
                        retVal += line.Trim();
                    }
                }
            }
            return retVal += "$";
        }

        static string getAlphabet(string alphabetFileName) {
            string line;
            string retVal = "$ ";
            using (StreamReader file = new StreamReader(alphabetFileName))
            {
                Console.Write(alphabetFileName + "\n");
                while ((line = file.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        break;
                    }
                    else
                    {
                        retVal += line.Trim();
                    }
                }
            }
            return retVal;
        }
    }
}
