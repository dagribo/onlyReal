DeadOrAliveOnGraph
��������� ����������� �������� ������� ���������� ��� ����� CFG.
����� �������� ��� ����������� ����� ��������� CFG. � ����� ������ �������������� ��������� ���� ����� � ������ ���������. ������ ����� ���������� ����� ����. ��� ���������� ���� ����� ����� ���������� �������� �� � ����� �����������. ��� �������������:

CFG controlFlowGraph = new CFG(blocks); // ���������� �����
var DefUse = new DefUseBlocks(controlFlowGraph); // ���������� Def,Use
InOutActiveVariables inOutActive = new InOutActiveVariables(DefUse, controlFlowGraph); // ��� ��� �������� ����������

controlFlowGraph = ControlFlowOptimisations.DeadOrAliveOnGraph(inOutActive.OutBlocks, controlFlowGraph); // �����������