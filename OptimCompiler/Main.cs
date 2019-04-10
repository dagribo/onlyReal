using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Block;
using SimpleLang.Optimisations;
using SimpleLang.ControlFlowGraph;
using SimpleLang.ThreeCodeOptimisations;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main(string[] args)
        {
            string FileName = @"../../../data/a.txt";
            if (args.Length > 0)
                FileName = args[0];
            try {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");
                    var r = parser.root;

                    FillParentVisitor generateParrent = new FillParentVisitor();
                    r.Visit(generateParrent);

                    /*Opt2Visitor opt2 = new Opt2Visitor();
                    r.Visit(opt2);

                    PrettyPrintVisitor ppvis = new PrettyPrintVisitor();
                    r.Visit(ppvis);
                    Console.WriteLine(ppvis.Text);

                    Console.WriteLine("\nAssignCountVisitor");
                    AssignCountVisitor vis1 = new AssignCountVisitor();
                    r.Visit(vis1);
                    Console.WriteLine(vis1.Count);

                    Console.WriteLine("\nStatementCountVisitor");
                    StatementCountVisitor vis2 = new StatementCountVisitor();
                    r.Visit(vis2);
                    Console.WriteLine(vis2.Count);

                    Console.WriteLine("\nMaxCountExprOpsVisitor");
                    MaxCountExprOpsVisitor vis3 = new MaxCountExprOpsVisitor();
                    r.Visit(vis3);
                    Console.WriteLine(vis3.Max);

                    Console.WriteLine("\nNestedCyclesVisitor");
                    NestedCyclesVisitor vis4 = new NestedCyclesVisitor();
                    r.Visit(vis4);
                    Console.WriteLine(vis4.HasNestedCycles);

                    Console.WriteLine("\nCycleNestedToIfVisitor");
                    CycleNestedToIfVisitor vis5 = new CycleNestedToIfVisitor();
                    r.Visit(vis5);
                    Console.WriteLine(vis5.HasCycleNestedToIf);

                    Console.WriteLine("\nIfNestedToCycleVisitor");
                    IfNestedToCycleVisitor vis6 = new IfNestedToCycleVisitor();
                    r.Visit(vis6);
                    Console.WriteLine(vis6.HasIfNestedToCycle);

                    Console.WriteLine("\nMaxDepthOfNestedCyclesVisitor");
                    MaxDepthOfNestedCyclesVisitor vis7 = new MaxDepthOfNestedCyclesVisitor();
                    r.Visit(vis7);
                    Console.WriteLine(vis7.Max);*/

                    var opt_sim_diff = new OptSimilarDifference();
                    r.Visit(opt_sim_diff);
                    var opt_sim_assignments = new OptSimilarAssignment();
                    r.Visit(opt_sim_assignments);
                    
                    Console.WriteLine("\nGenerate Three address code");
                    ThreeAddressCodeVisitor treeCode = new ThreeAddressCodeVisitor();
                    r.Visit(treeCode);
                    Console.WriteLine(treeCode.ToString());


                    AutoThreeCodeOptimiser app = new AutoThreeCodeOptimiser();
                    app.Add(new EvalConstExpr());
                    app.Add(new DistributionOfConstants());
                    app.Add(new ApplyAlgebraicIdentities());
                    //ToDo Add new threeCodeOptimisations

                    var blocks = app.Apply(treeCode);
                    Console.WriteLine(ThreeAddressCodeVisitor.ToString(blocks));


                    int i = 1;
                    foreach (var block in blocks)
                    {
                        Console.WriteLine("Block " + i.ToString());
                        foreach (var line in block)
                            Console.WriteLine(line);
                        i += 1;
                    }

                    var cfg = new ControlFlowGraph(treeCode);
                    cfg.GenerateCFG();
                    Console.WriteLine("\nControl flow graph:");
                    Console.WriteLine(cfg.GetAsGraph());

                    /*Opt11Visitor opt11vis = new Opt11Visitor();
                      ppvis.Text = "";
                      r.Visit(opt11vis);
                      r.Visit(ppvis);
                      Console.WriteLine(ppvis.Text);*/


                    /*var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");

                    var pp = new PrettyPrintVisitor();
                    parser.root.Visit(pp);
                    Console.WriteLine(pp.Text);*/
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }

            // Console.ReadLine();
        }

    }
}
