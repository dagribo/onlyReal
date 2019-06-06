IterativeAlgorithm
��������� ����������� ��������� ������������ ����������, �������� �� ����� ���������.
���� ������ ����������� (�������� ������� ���������� �� ������ ��� ��� �������� ����������) � ������� ������� ������������ ������� � ����������� ���.

using SimpleLang.GenericIterativeAlgorithm;
using GenericTransferFunction;


CFG controlFlowGraph = new CFG(blocks);

using SimpleLang.GenericIterativeAlgorithm;
using GenericTransferFunction;


CFG controlFlowGraph = new CFG(blocks);

                    // �������� ���������� � ������
                    var blocksInfo = new List<BlockInfo<string>>();

                    // ���������� �������� Def � Use ��� ����� ����� ������� ������
                    var DefUse = new DefUseBlocks(controlFlowGraph);
                    // �������� ���������� � ������

                    for (int i = 0; i < DefUse.DefBs.Count; i++)
                        blocksInfo.Add(new BlockInfo<string>(DefUse.DefBs[i], DefUse.UseBs[i]));

                    // �������� ����� ��� ������� �������� ����������
                    Func<List<BlockInfo<string>>, CFG, int, BlockInfo<string>> meetOperator = (blocksInfos, graph, index) =>
                    {
                        var successorIndexes = graph.cfg.GetOutputNodes(index);
                        var resInfo = new BlockInfo<string>(blocksInfos[index]);
                        foreach (var i in successorIndexes)
                            resInfo.OUT.UnionWith(blocksInfos[i].IN);
                        return resInfo;
                    };

                    // ������� ������������ ������� ��� ������� �������� ����������
                    Func<BlockInfo<string>, BlockInfo<string>> tFunc = (blockInfo) =>
                    {
                        blockInfo.IN = new HashSet<string>();
                        blockInfo.IN.UnionWith(blockInfo.OUT);
                        blockInfo.IN.ExceptWith(blockInfo.HelpFirst);
                        blockInfo.IN.UnionWith(blockInfo.HelpSecond);
                        return blockInfo;
                    };

                    var transferFunction = new TransferFunction<BlockInfo<string>>(tFunc);

                    // �������� ������� ������������� ���������
                    var iterativeAlgorithm = new IterativeAlgorithm<string>(blocksInfo, controlFlowGraph, meetOperator,
                    false, new HashSet<string>(), new HashSet<string>(), transferFunction);

                    // ���������� ��������� - ���������� IN � OUT
                    iterativeAlgorithm.Perform();

                    controlFlowGraph = ControlFlowOptimisations.DeadOrAliveOnGraph(iterativeAlgorithm.GetOUTs(), controlFlowGraph); // ���������� �����������
