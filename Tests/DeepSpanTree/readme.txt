CFG edge classification+DeepSpanTree
������������� ���� CFG+���������� ���������� ��������� ������ � ��������������� ���������� ������
���� DeepSpanTree.cs
var controlFlowGraph=new CFG(treeCode);
var tt = new SpanTree(controlFlowGraph);
var edges=tt.buildSpanTree();