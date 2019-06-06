DefUseConstOpt_DefUseDeadCodeOpt
��������� def-use ��� ������ ���������� ������ ���, �� ������ ���� ���������� ��������� ��������� �������� �������� � �������� �������� ����.

���������� ������� �� 2 ������:
-����� DefUseConstOpt ��������� ��������� �������� ��������;
-����� DefUseDeadCodeOpt ��������� ��������� �������� �������� ����.

�������� �������� ���� ��������� ������ ���� � �������� ��������� ���� ���, �.� ������ ������� ����������� ���������� ���� �� �� �����, ��� ��� �� ������������ � ������ ���, � ����������� �������� ������ ������ ������ ���.

��� ������� ����������� ���������� �������� �������� ������ DefUseDeadCodeOp/DefUseConstOpt � ��������� ������ AutoThreeCodeOptimiser � �������� ����� Apply:

AutoThreeCodeOptimiser app = new AutoThreeCodeOptimiser();
app.Add(new DefUseConstOpt());
app.Add(new DefUseDeadCodeOpt());
var blocks = app.Apply(treeCode);

��� treeCode - ��������� ������ ThreeAddressCodeVisitor.