using System;
using System.Collections.Generic;
using System.IO;

namespace YaYSM
{
    class Program
    {
        public static readonly int MAX_INSTRUCTIONS = 512;
        static List<int> intermediateLanguage = new List<int>();
        static Dictionary<string, int> symbolTable = new Dictionary<string, int>();
        static Dictionary<string, int> instructionTable = new Dictionary<string, int>();

        static int curIntermediateNumber = 0;
        static int instructionNumber = 0;

        static void Main(string[] args)
        {

            string path = @"";
            var rawAssemblyLines = File.ReadLines(path);

            // Remove comments, create symbol table, create IL mapping, create instruction table.
            FirstPass(rawAssemblyLines);
            SecondPass();
        }

        private static void SecondPass()
        {
            throw new NotImplementedException();
        }

        private static void FirstPass(IEnumerable<string> rawAssemblyLines)
        {
            foreach (string line in rawAssemblyLines)
            {
                string currentLine = line.Trim();

                // This line is a comment or empty, ignore it.
                if (currentLine.StartsWith('#') || currentLine.Length == 0)
                {
                    continue;
                }
                // Is the line a label?
                else if (currentLine.Contains(":"))
                {
                    // Labels refrence the NEXT instruction, so should map to curr + 1
                    if (!symbolTable.TryAdd(currentLine.Trim(':'), curIntermediateNumber + 1))
                    {
                        Console.Error.WriteLine("Hey! You can't have the same jump label more than once!");
                        return;
                    }
                }
                // At this point, it better be an instruction!
                else
                {
                    ++curIntermediateNumber;

                    // Try and add the instruction.
                    if (instructionTable.TryAdd(currentLine, instructionNumber))
                    {

                        // Looks like we found a new instruction, but by adding it have we gone over our limit?
                        if (instructionNumber > MAX_INSTRUCTIONS)
                        {
                            Console.Error.WriteLine($"You have more than {MAX_INSTRUCTIONS} instructions, optimize your YaYSM!");
                            return;
                        }

                        instructionNumber++;
                    }
                    // Either we just added the instruction or it was already there, either way time to map it!.
                    intermediateLanguage.Add(instructionTable[currentLine]);
                }
            }
        }
    }
}
