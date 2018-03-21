using System;
using System.Collections.Generic;
using System.Text;

namespace YaYSM
{
    //string noComments = instruction.Split("//" , StringSplitOptions.None)[0];
    class Instruction
    {
        protected string _operation { get; set; }

        public override string ToString()
        {
            return _operation;
        }
    }

    class DataProcessing : Instruction
    {
        private static readonly string Prefix = "0000000";
        public DataProcessing(string instruction)
        {
            string[] insPieces = instruction.Split(" ",2);
            string opcode = Literals.OpCodes[insPieces[0].ToUpper()];
            string s = "1";
            OperationType type = Literals.GetOperation(insPieces[0].ToUpper());
            string rd = "r15", rn = "r15", rs = "r15", imm = "00000000";
            int numFinished = 0;
            foreach (string piece in insPieces[1].Split(','))
            {
                var curr = piece.Trim();
                if(type.HasFlag(OperationType.Rd) && numFinished < 1)
                {
                    rd = curr;
                }
                if(numFinished == 0)
                {
                    numFinished++;
                    continue;
                }
                if (type.HasFlag(OperationType.Rn) && numFinished < 2)
                {
                    if (Int32.TryParse(curr[0].ToString(), out int dead))
                    {
                        rn = "r15";
                        imm = Literals.IntToBinaryString(Int32.Parse(curr));
                        break;
                    }
                    else if(curr.Contains("+"))
                    {
                        var split = curr.Split('+');
                        rn = split[0];
                        imm = Literals.IntToBinaryString(Int32.Parse(split[1]));
                        break;
                    }
                    
                }
                if (numFinished == 1)
                {
                    numFinished++;
                    continue;
                }
                if (type.HasFlag(OperationType.Rs) && numFinished < 3)
                {
                    // If it's an int, theres no Rs
                    if (Int32.TryParse(curr[0].ToString(), out int dead))
                    {
                        rs = "r15";
                        imm = Literals.IntToBinaryString(Int32.Parse(curr));
                        break;
                    }
                    else
                    {
                        var split = curr.Split('+');
                        rs = split[0];
                        if (split.Length == 2) imm = Literals.IntToBinaryString(Int32.Parse(split[1]));
                    }
                    break;
                }
                
            }
            _operation = Prefix + opcode + s + Literals.Registers[rn] + Literals.Registers[rd] + Literals.Registers[rs] + imm;
        }
    }

    class Branch : Instruction
    {
        private static readonly string TypeOp = "1010";
        public Branch(string instruction, int jumpLocation)
        {
            string cond, condBinary;
            cond = instruction.Split(" ", 1)[0];
            condBinary = Literals.Branches[cond];
            _operation = condBinary + TypeOp + Literals.IntToBinaryString(jumpLocation, 24);
        }
    }
    
    class LoadStore : Instruction
    {
        private static readonly string Prefix = "00000101110";
        public LoadStore(string instruction)
        {
            string[] insPieces = instruction.Split(" ", 2);
            string l = "0";
            string rd, rn, imm = "000000000000";
            if(insPieces[0].ToUpper() == "LDB")
            {
                string[] split = insPieces[1].Split(",");
                l = "1";
                rd = Literals.Registers[split[0].Trim()];
                string rnRaw = split[1].Trim(new char[] { ' ', '[', ']' });
                if (Int32.TryParse(rnRaw[0].ToString(), out int dead))
                {
                    rn = Literals.Registers["r15"];
                    imm = Literals.IntToBinaryString(Int32.Parse(rnRaw), 12);
                }
                else
                {
                    if(rnRaw.Contains("+"))
                    {
                        rn = Literals.Registers[rnRaw.Split("+")[0]];
                        imm = Literals.IntToBinaryString(Int32.Parse(rnRaw.Split("+")[1]),12);
                    }
                    else
                    {
                        rn = Literals.Registers[rnRaw];
                    }
                }
            }
            else
            {
                string[] split = insPieces[1].Split(",");
                rn = Literals.Registers[split[1].Trim()];
                string rdRaw = split[0].Trim(new char[] { ' ', '[', ']' });
                if (Int32.TryParse(rdRaw[0].ToString(), out int dead))
                {
                    rd = Literals.Registers["r15"];
                    imm = Literals.IntToBinaryString(Int32.Parse(rdRaw), 12);
                }
                else
                {
                    if (rdRaw.Contains("+"))
                    {
                        rd = Literals.Registers[rdRaw.Split("+")[0]];
                        imm = Literals.IntToBinaryString(Int32.Parse(rdRaw.Split("+")[1]), 12);
                    }
                    else
                    {
                        rd = Literals.Registers[rdRaw];
                    }
                }
            }
            _operation = Prefix + l + rd + rn + imm;
        }

    }

    class Yay : Instruction
    {
        public override string ToString()
        {
            return "0000111000000000000000000000000";
        }
    }
}
