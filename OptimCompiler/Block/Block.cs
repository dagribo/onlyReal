using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using ProgramTree;
using SimpleLang.Visitors;

namespace SimpleLang.Block
{
    public class Block
    {
        public LinkedList<ThreeCode> code;
        public Block(ThreeAddressCodeVisitor _code)
        {
            this.code = _code.GetCode();
        }

        public List<int> FindLeaders()
        {
            var Leaders = new List<int>();
            int i = 1;

            bool PreviousIsGoto = false;

            foreach (var line in this.code)
            {
                if (i == 1)
                    Leaders.Add(i);
                else
                    if (!String.IsNullOrEmpty(line.label))
                        Leaders.Add(i);
                    else
                        if (PreviousIsGoto)
                            Leaders.Add(i);

                PreviousIsGoto = line.operation == ThreeOperator.Goto || line.operation == ThreeOperator.IfGoto;
                
                i += 1;
            }

            return Leaders;
        }

        public List<LinkedList<ThreeCode>> GenerateBlocks()
        {
            var Leaders = FindLeaders();
            int i = 1;
            int LiderInd = 0;
            
            var Blocks = new List<LinkedList<ThreeCode>>();

            foreach (var line in this.code)
            {
                if (LiderInd < Leaders.Count && i == Leaders[LiderInd])
                {
                    Blocks.Add(new LinkedList<ThreeCode>());
                    LiderInd += 1;
                }
                Blocks.Last().AddLast(line);
                i += 1;
            }

            return Blocks;
        }
    }
}