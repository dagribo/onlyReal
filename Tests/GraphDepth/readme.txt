GraphDepth
��������� ������� CFG.
����� �������� ��� ����������� ����� ��������� CFG � ��������� ��������� ���:
using SimpleLang.ControlFlowGraph;

var blocks = new Block(treeCode).GenerateBlocks();
CFG controlFlowGraph = new CFG(blocks);
int depth = GraphDepth.GetGraphDepth(controlFlowGraph);