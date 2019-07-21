using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace MachineLanguageProgramming
{
    public class MLPCompX
    {
        // operation codes
        const int Read = 10;       // Read the data word
        const int Write = 11;      // Write the data word
        const int Load = 20;       // Load the data word
        const int Store = 21;      // Store the data word
        const int Add = 30;        // Add the data word
        const int Subtract = 31;   // Subtract the data word
        const int Divide = 32;     // Divide the data word
        const int Multiply = 33;   // Multiple the data word
        const int Branch = 40;     // Branch to a specific location 
        const int BranchNeg = 41;  // Branch to a specific location if the accumulator is negative 
        const int Halt = 42;       // Halt the program task  

         int accumulator { get; set; } // add information to the accumulator before it is utilized for calculations

        List<MemoryAddrData> MemoryHashTable = new List<MemoryAddrData>();  // List of words , where each word is a signed 4 digit decimal number
        // the first two digits of the word are the operation code, the last two digits is the operand
         int memoryAddress { get; set; }

        MLPCompX()
         {
            memoryAddress = 0;
       
         }

       public static void demoMLPCompX()
         {
            //   Console.WriteLine("Enter a string");
            //   string input = Console.ReadLine();
            //   string[] words = Regex.Split(input, ",");

            Console.WriteLine(" \n Simple Sum of Variables \n ");
            MLPCompX compilerobjectA = new MLPCompX();
            string[] MachineCodeProgramA = {"100734", "100856", "200700", "300800", "210900", "110900", "420000"};
            compilerobjectA.ExecuteProgram(compilerobjectA, MachineCodeProgramA);

           Console.WriteLine("\n Branching with Variables, display variable is the largest data integer \n");
            MLPCompX compilerobjectB = new MLPCompX();
            string[] MachineCodeProgramB = {"100975", "101076", "200900", "311000", "410700", "110900", "420000","111000","420000"};
            compilerobjectB.ExecuteProgram(compilerobjectB, MachineCodeProgramB);

        } // static void demoMLPCompX

        // Load memory hash table
        int ExecuteProgram(MLPCompX compilerobject , string[] words)
          {

            foreach (var element in words)  // load all the instructions
            {
                compilerobject.MemoryHashTable.Add(new MemoryAddrData(element, compilerobject.memoryAddress)); // memory address
                compilerobject.memoryAddress++;
            }

            int DisplayVariable = 0;
            bool HaltExecuted = false;
            bool BranchpredictiontableFlag = false;
            int opcode = 0;
            int extractedMemAddr = 0;
            string extractedDataValue = "0";
            StringBuilder dataword = new StringBuilder("000000");

            // Split the instructions with operation code and operands
            for (int i = 0; i < words.Length; i++)
            {
                if (HaltExecuted)
                {
                    i = words.Length;
                }
                else
                {
                    if (!BranchpredictiontableFlag)
                    {
                        opcode = int.Parse(words[i].Substring(0, 2));
                        extractedMemAddr = int.Parse(words[i].Substring(2, 2));
                        extractedDataValue = words[i].Substring(4, 2);
                    }

                    switch (opcode)
                    {
                        case Read:
                            {
                                dataword.Remove(4, 2);
                                dataword.Append(extractedDataValue);
                                compilerobject.MemoryHashTable.Add(new MemoryAddrData(dataword.ToString(), extractedMemAddr)); // memory address
                                BranchpredictiontableFlag = false;

                            }
                            break;
                        case Write:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                    {
                                        DisplayVariable = int.Parse(element.dataVariable.Substring(4, 2));
                                    }
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Load:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                        accumulator = int.Parse(element.dataVariable.Substring(4, 2));
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Store:
                            {
                                dataword.Remove(4, 2);
                                dataword.Append(accumulator);
                                compilerobject.MemoryHashTable.Add(new MemoryAddrData(dataword.ToString(), extractedMemAddr)); // memory address
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Add:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                        accumulator = accumulator + int.Parse((element.dataVariable).Substring(4, 2));
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Subtract:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                        accumulator = accumulator - int.Parse((element.dataVariable).Substring(4, 2));
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Divide:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                        accumulator = int.Parse((element.dataVariable).Substring(4, 2)) / accumulator;
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Multiply:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (element.addressVariable == extractedMemAddr)
                                        accumulator = int.Parse((element.dataVariable).Substring(4, 2)) * accumulator;
                                }
                                BranchpredictiontableFlag = false;
                            }
                            break;
                        case Branch:
                            {
                                foreach (var element in compilerobject.MemoryHashTable)
                                {
                                    if (extractedMemAddr == element.addressVariable)
                                    {
                                        opcode = int.Parse(element.dataVariable.Substring(0, 2));
                                        extractedMemAddr = int.Parse(element.dataVariable.Substring(2, 2));
                                        extractedDataValue = element.dataVariable.Substring(4, 2);

                                        Console.WriteLine($" Opcode {opcode} Extracted Memory Address {extractedMemAddr} Data Value {extractedDataValue} ");
                                        break;
                                    }
                                }

                                BranchpredictiontableFlag = true;
                            }
                            break;
                        case BranchNeg:
                            {

                                if (accumulator < 0)
                                {
                                    foreach (var element in compilerobject.MemoryHashTable)
                                    {
                                        if (extractedMemAddr == element.addressVariable)
                                        {
                                            opcode = int.Parse(element.dataVariable.Substring(0, 2));
                                            extractedMemAddr = int.Parse(element.dataVariable.Substring(2, 2));
                                            extractedDataValue = element.dataVariable.Substring(4, 2);

                                            break;
                                        }
                                    }

                                    BranchpredictiontableFlag = true;

                                }

                            }
                            break;
                        case Halt:
                            {
                                HaltExecuted = true;
                                BranchpredictiontableFlag = false;
                                break;
                            }
                            break;
                        default:
                            break;
                    } // switch statement

                }  // else loop

            } //for loop
            foreach (var element in compilerobject.MemoryHashTable)
                Console.WriteLine(element);

            if(HaltExecuted)
            Console.WriteLine($"Display Variable is {DisplayVariable}");
            else
                Console.WriteLine("Halt is not executed, error");

            return 0;              
        } // end of program

    } // public class  MLPCompX
} // namespace compiler

 
