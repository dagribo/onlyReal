using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ProgramTree;
using SimpleLang.Visitors;
using SimpleLang.Block;
using System.Text;
namespace SimpleLang.ControlFlowGraph
{
    public class Graph
    {
        public int[] g;
        public int n;

        public Graph(int _n)
        {
            this.n = _n;
            this.g = new int[this.n*this.n];
            
            for (var i = 0; i < this.n * this.n; ++i)
                this.g[i] = 0;
        }

        public void AddArc(int i, int j)
        {
            this.g[i * this.n + j] = 1;
        }
        
        public void ResetArc(int i, int j)
        {
            this.g[i * this.n + j] = 0;
        }

        public int[,] GetAdjacencyMatrix()
        {
            var result = new int[this.n, this.n];
            
            for (var i = 0; i < this.n; ++i)
                for (var j = 0; j < this.n; ++j)
                    result[i, j] = this.g[i * this.n + j];

            return result;
        }

        public List<List<int>> GetAdjacencyList()
        {
            var result = new List<List<int>>();

            for (var i = 0; i < this.n; ++i)
            {
                result.Add(new List<int>());
                for (var j = 0; j < this.n; ++j)
                    if (this.g[i*this.n + j] == 1)
                        result.Last().Add(j);
            }

            return result;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < this.n; ++i)
            {
                result += i.ToString() + ": ";
                for (var j = 0; j < this.n; ++j)
                    if (this.g[i * this.n + j] == 1)
                        result += j.ToString() + " ";
                result += '\n'.ToString();
            }

            return result;
        }

        public List<int> GetInputNodes(int node_id)
        {
            var res = new List<int>();
            for (var i = 0; i < this.n; ++i)
                if (this.g[i * this.n + node_id] == 1)
                    res.Add(i);
            return res;
        }
        
        public List<int> GetOutputNodes(int node_id)
        {
            var res = new List<int>();
            for (var j = 0; j < this.n; ++j)
                if (this.g[node_id * this.n + j] == 1)
                    res.Add(j);
            return res;
        }
    }
    
    public class ControlFlowGraph
    {
        public List<LinkedList<ThreeCode>> blocks;
        public Graph cfg;
        
        public ControlFlowGraph(List<LinkedList<ThreeCode>> b)
        {
            this.blocks = b;
            cfg = new Graph(this.blocks.Count);
            GenerateCFG();
        }

        public ControlFlowGraph(ThreeAddressCodeVisitor code)
        {
            var code_blocks = new Block.Block(code);
            this.blocks = code_blocks.GenerateBlocks();
            cfg = new Graph(this.blocks.Count);
            GenerateCFG();
        }

        private ControlFlowGraph GenerateCFG()
        {
            //�.�. ���� ����� ����������� ���� ������ �����, �� ������������ �������, �� ��������
            //�� ����� ����� ����� �������� ������ ����� � �����
            var Labels = new Dictionary<string, int>();

            //����� ����� ���������� ���� � ������ �����, ����� ������ ���������� ����� �� �������� �������
            for (int i = 0; i < blocks.Count; i++)
                if (blocks[i].First.Value.label.Length > 0)
                    Labels[blocks[i].First.Value.label] = i;

            // ���� ��������� �������� � ����� �� �������� GOTO, �� ���� 
            // ����� �� ����� ����� ���� ������� � �����, ���������� �� ��� �� �������
            // ����� �� ����� ����� ������������ ������� �� ����� ��������� ��������� goto.
            // ��������� ���� �� �������������, �.�. ��� ����� �� ���������. ���� ���������,
            // ��� �� ���� ����� ���� ������� �� �����, �� ����� �� ������ ��� �������� �� ����� 
            // �� ���������� ��������� ����� ���� �������, ��� ������� ��� ������ �����
            for (int i = 0; i < blocks.Count-1; i++)
                switch (blocks[i].Last.Value.operation)
                {
                    case ThreeOperator.Goto:
                        cfg.AddArc(i, Labels[blocks[i].Last.Value.arg1.ToString()]);
                        break;
                    case ThreeOperator.IfGoto:
                        cfg.AddArc(i, Labels[blocks[i].Last.Value.arg2.ToString()]);
                        cfg.AddArc(i, i + 1);
                        break;
                    default:
                        cfg.AddArc(i, i + 1);
                        break;
                }

            //for (var i = 0; i < this.blocks.Count; ++i)
            //    for (var j = 0; j < this.blocks.Count; ++j)
            //        if (j == i + 1)
            //            this.cfg.AddArc(i, j); // �� �� ������� ����� ���� ������� � ���� � �������� �� 1 ������
            //    else
            //        {
            //            int bi = 0;
            //            foreach (var line1 in this.blocks[i])
            //            {
            //                int bj = 0;
            //                foreach (var line2 in this.blocks[j])
            //                {
            //                    if (line1.operation == ThreeOperator.Goto || line1.operation == ThreeOperator.IfGoto)
            //                        if (String.Equals(line2.label, line1.ToString().Split(new string[] {"goto "}, StringSplitOptions.None).Last()))
            //                            this.cfg.AddArc(bi, bj); // ������������ ���������� ��������, OutOfRangeException
            //                    bj++;
            //                }
            //                bi++;
            //            }
            //        }

            return this;
        }

        public int[,] GetAsAdjacencyMatrix()
        {
            return this.cfg.GetAdjacencyMatrix();
        }

        public List<List<int>> GetAsAdjacencyList()
        {
            return this.cfg.GetAdjacencyList();
        }

        public Graph GetAsGraph()
        {
            return this.cfg;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var block in blocks)
                foreach (var line in block)
                    sb.Append(line.ToString() + "\n");
            return sb.ToString();
        }
    }
}