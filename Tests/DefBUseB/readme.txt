���������� �������� DEFb � USEb ��� �������� ����������

����������� �������� � ������ ������� ����� B � ����� ������ ����������.

DEFb - ��-�� ����������, ������������ � B
USEb - ��-�� ����������, �������� ������� ����� �������������� � B �� ������ �� �����������. 

�������� ���:
{
  int i, j, k, l;
  i = k + 1;
  j = l + 1;
  k = i;
  l = j;
}

���������:
DEFb: i j k l
USEb: k l

������:
var blocks = new Block(treeCode).GenerateBlocks(); //���������� ������� �����
CFG controlFlowGraph = new CFG(blocks); //������ CFG (using CFG = SimpleLang.ControlFlowGraph.ControlFlowGraph;)
var DefUse = new DefUseBlocks(controlFlowGraph); //� ���� ������ ��� ���� - DefBs � UseBs = new List<HashSet<string>>();