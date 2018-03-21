using System;
using System.Collections.Generic;
using System.Text;

namespace YaYSM
{
    [Flags]
    public enum OperationType
    {
        Rd = 1,
        Rn = 2,
        Rs = 4,
        Imm = 8
    }

    public static class Literals
    {

        public static string IntToBinaryString(int number, int padAmount = 8)
        {
            const int mask = 1;
            var binary = string.Empty;
            while (number > 0)
            {
                // Logical AND the number and prepend it to the result string
                binary = (number & mask) + binary;
                number = number >> 1;
            }

            return binary.PadLeft(padAmount, '0'); ;
        }


        public static OperationType GetOperation(string instruction)
        {
            OperationType type;
            switch(instruction)
            {
                case "ADD":
                    type = OperationType.Rd | 
                           OperationType.Rn |
                           OperationType.Rs |
                           OperationType.Imm;
                    break;
                case "SUB":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs |
                           OperationType.Imm;
                    break;
                case "AND":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs;
                    break;
                case "ORE":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs;
                    break;
                case "XOR":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs;
                    break;
                case "NOT":
                    type = OperationType.Rd |
                           OperationType.Rn;
                    break;
                case "SLL":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Imm;
                    break;
                case "SRL":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Imm;
                    break;
                case "CMP":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs |
                           OperationType.Imm;
                    break;
                case "MOV":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Imm;
                    break;
                case "SAB":
                    type = OperationType.Rd |
                           OperationType.Rn |
                           OperationType.Rs |
                           OperationType.Imm;
                    break;
                default:
                    throw new Exception();
            }

            return type;
        }

        public static Dictionary<string, string> Registers = new Dictionary<string, string>
        {
            ["r0"] = "0000",
            ["r1"] = "0001",
            ["r2"] = "0010",
            ["r3"] = "0011",
            ["r4"] = "0100",
            ["r5"] = "0101",
            ["r6"] = "0110",
            ["r7"] = "0111",
            ["r8"] = "1000",
            ["r9"] = "1001",
            ["r10"] = "1010",
            ["r11"] = "1011",
            ["r12"] = "1100",
            ["r13"] = "1101",
            ["r14"] = "1110",
            ["r15"] = "1111"
        };

        public static Dictionary<string, string> OpCodes = new Dictionary<string, string>
        {
            ["ADD"] = "0000",
            ["SUB"] = "0001",
            ["AND"] = "0010",
            ["ORE"] = "0011",
            ["XOR"] = "0100",
            ["NOT"] = "0101",
            ["SLL"] = "0110",
            ["SRL"] = "0111",
            ["CMP"] = "1000",
            ["MOV"] = "1001",
            ["SAB"] = "1010"
        };

        public static Dictionary<string, string> LoadStore = new Dictionary<string, string>
        {
            ["LDB"] = "0000",
            ["STB"] = "0001"
        };

        public static Dictionary<string, string> Branches = new Dictionary<string, string>
        {
            ["BAL"] = "0000",
            ["BLT"] = "0001",
            ["BGE"] = "0010",
            ["BNE"] = "0011",
            ["BEQ"] = "0100",
            ["BEV"] = "0101",
            ["BNO"] = "0110"
        };
    }
}
