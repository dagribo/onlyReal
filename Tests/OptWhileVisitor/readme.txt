OptWhileVisitor
�������� ����� while(false) st; �� null �� AST.
����� �������� ��� ����������� ����� ��������� AST, � ��������� ��������� ���:
var r = parser.root;    // ������ AST
r.Visit(new FillParentVisitor());   // ��������� ������ �� ��������� �� AST
r.Visit(new OptWhileVisitor());	// ���������� ������� �����������
