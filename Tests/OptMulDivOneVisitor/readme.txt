OptMulDivOneVisitor
�������� ��������� ���� 1 * ex, ex * 1, ex / 1 �� ex �� AST.
����� �������� ��� ����������� ����� ��������� AST, � ��������� ��������� ���:
var r = parser.root;    // ������ AST
r.Visit(new FillParentVisitor());   // ��������� ������ �� ��������� �� AST
r.Visit(new OptMulDivOneVisitor());	// ���������� ������� �����������
