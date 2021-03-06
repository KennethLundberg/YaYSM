﻿using System;
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
        private static readonly string Prefix = "000000";
        public DataProcessing(string instruction)
        {
            string[] insPieces = instruction.Split(" ",2);
            string opcode = Literals.OpCodes[insPieces[0].ToUpper()];
            string s = "1";
            string I = "0";
            OperationType type = Literals.GetOperation(insPieces[0].ToUpper());
            string rd = "r15", rn = "r15", rs = "r15", imm = "00000000";
            //Compare is weird because it doesn't have an rd, add it anyways because it isn't used.
            if(opcode.Equals("1000"))
            {
                insPieces[1] = "r15," + insPieces[1];
            }
            string[] registers = insPieces[1].Split(',');
            if(type.HasFlag(OperationType.Rd))
            {
                rd = registers[0];
            }
            if(type.HasFlag(OperationType.Rn))
            {
                if (Int32.TryParse(registers[1].ToString(), out int immInt))
                {
                    rn = "r15";
                    imm = Literals.IntToBinaryString(immInt);
                    I = "1";
                }
                else
                {
                    rn = registers[1];
                }
            }
            if(type.HasFlag(OperationType.Rs) || type.HasFlag(OperationType.Imm))
            {
                if (!type.HasFlag(OperationType.Rs) && registers.Length == 2)
                {
                    //Do nothing, we already did it in the last step
                }
                else if (Int32.TryParse(registers[2].ToString(), out int immInt))
                {
                    rs = "r15";
                    imm = Literals.IntToBinaryString(immInt);
                    I = "1";
                }
                else
                {
                    rs = registers[2];
                }
            }
            _operation = Prefix + I + opcode + s + Literals.Registers[rn] + Literals.Registers[rd] + Literals.Registers[rs] + imm;
        }
    }

    class Branch : Instruction
    {
        private static readonly string TypeOp = "1010";
        public Branch(string instruction, int jumpLocation)
        {
            string cond, condBinary;
            cond = instruction.Split(" ", 2)[0].Trim().ToUpper();
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
            _operation = Prefix + l + rn + rd + imm;
        }

    }

    class Yay : Instruction
    {
        public override string ToString()
        {
            return "00001110000000000000000000000000";
        }
    }
}
