DeadOrAliveOptimization
��������� ����������� �������� ������� ���������� ��� ������� �� ������ � CFG.
����� �������� ��� ����������� ����� ��������� CFG. ������ ����� ���������� ����� ����. ��� ���������� ���� ����� ����� ���������� �������� �� � ����� �����������. ��� �������������:
CFG controlFlowGraph = new CFG(blocks); // ���������� �����
controlFlowGraph = DeadOrAliveOptimization.DeleteDeadVariables(controlFlowGraph); // �����������
