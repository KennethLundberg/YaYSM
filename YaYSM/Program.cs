using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YaYSM
{
    class Program
    {
        public static readonly int MAX_INSTRUCTIONS = 512;

        static int curIntermediateNumber = 0;
        static int instructionNumber = 0;

        static void Main(string[] args)
        {

            string path = @"C:\Users\maxlu\Desktop\141L\input.txt";
            var rawAssemblyLines = File.ReadLines(path);

            // Remove comments, create symbol table, create IL mapping, create instruction table.
            List<string> noComments = RemoveComments(rawAssemblyLines);
            FirstPass(noComments);
            List<Instruction> finalBank = SecondPass();
            OutputResults(finalBank, instructionList);
        }

        private static void OutputResults(List<Instruction> finalBank, List<int> finalInstruction)
        {
            List<string> convertedInstruction = finalInstruction.Select(i => Literals.IntToBinaryString(i, 9)).ToList();
            List<string> bankString = finalBank.Select(b => b.ToString()).ToList();
            File.WriteAllLines(@"C:\Users\maxlu\Desktop\141L\instruction.txt", convertedInstruction);
            File.WriteAllLines(@"C:\Users\maxlu\Desktop\141L\bank.txt", bankString);
        }

        private static List<string> RemoveComments(IEnumerable<string> rawAssemblyLines)
        {
            List<string> noComments = new List<string>();
            foreach(string line in rawAssemblyLines)
            {
                string curr = line.Split("#", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                if (!string.IsNullOrEmpty(curr)) noComments.Add(curr);
            }
            return noComments;
        }

        private static List<Instruction> SecondPass()
        {
            List<Instruction> converted = new List<Instruction>();
            foreach (string rawInstruction in rawBank)
            {
                string code = rawInstruction.Trim().Substring(0, 3).ToUpper();
                if (Literals.OpCodes.ContainsKey(code))
                {
                    converted.Add(new DataProcessing(rawInstruction));
                }
                else if (Literals.LoadStore.ContainsKey(code))
                {
                    converted.Add(new LoadStore(rawInstruction));
                }
                else if (Literals.Branches.ContainsKey(code))
                {
                    string jumpLiteral = rawInstruction.Substring(3).Trim();
                    int location = labelToIndex[jumpLiteral];
                    converted.Add(new Branch(rawInstruction, location));
                }
                else
                {
                    converted.Add(new Yay());
                }
            }
            return converted;
        }

        public static List<int> instructionList = new List<int>(512);
        public static Dictionary<string, int> labelToIndex = new Dictionary<string, int>();
        public static List<string> rawBank = new List<string>(512);

        private static void FirstPass(List<string> rawAssemblyLines)
        {

            int instructionIndex = 0, bankIndex = 0;
            for (int i = 0; i < rawAssemblyLines.Count; i++)
            {
                string currentLine = rawAssemblyLines[i];
                if (currentLine.Contains(":"))
                {
                    string newLabel = currentLine.Split(':')[0].Trim();
                    labelToIndex[newLabel] = instructionIndex;
                }
                else
                {
                    if(rawBank.Contains(currentLine))
                    {
                        instructionList.Insert(instructionIndex, rawBank.IndexOf(currentLine));
                        instructionIndex++;
                    }
                    else
                    {
                        rawBank.Insert(bankIndex,currentLine);
                        instructionList.Insert(instructionIndex, bankIndex);
                        bankIndex++;
                        instructionIndex++;
                    }
                }
            }
        }
    }
}
