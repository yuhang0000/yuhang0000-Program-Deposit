using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Select_To_Open_The_Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.CurrentDirectory + @"\rule.txt";
            List<(string,string,int)> list = new List<(string, string,int)>();

            if(Debugger.IsAttached == true)
            {
                Console.WriteLine(string.Join(" ", args));
                Console.WriteLine("");
                //Console.ReadKey();
            }

            //读取规则
            if (File.Exists(path) == true)
            {
                string[] files = File.ReadAllLines(path);
                foreach (string file in files)
                {
                    string[] rules = file.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (rules.Length == 2) //默认值
                    {
                        list.Add( ("", rules[0].Trim(), int.Parse(rules[1].Trim()) ) );
                    }
                    else if(rules.Length > 2)
                    {
                        list.Add( (rules[0].Trim() , rules[1].Trim(), int.Parse(rules[2].Trim()) ) );
                    }
                }
            }
            else //默认地
            {
                list.Add( ("http://","explorer %1", 0) );
                list.Add( ("https://","explorer %1", 0) );
                list.Add( ("",  "explorer %1", 0) );
            }

            //这里获取帮助
            if (string.Equals(args[0], "help", StringComparison.CurrentCulture) == true ||
                string.Equals(args[0], "/?", StringComparison.CurrentCulture) == true ||
                string.Equals(args[0], "--help", StringComparison.CurrentCulture) == true ||
                string.Equals(args[0], "-h", StringComparison.CurrentCulture) == true )
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);
                Console.WriteLine("一个小程式, 能根据启动参数自动选择正确的程式来启动. ");
                Console.WriteLine("");
                Console.WriteLine("##### 一些帮助 #####");
                Console.WriteLine("");
                Console.WriteLine("# 获取帮助: /? (这段其实是个废话, 因为你都能看见这份支援文档了...)");
                Console.WriteLine("");
                Console.WriteLine("# 创建规则:");
                Console.WriteLine("  格式: <关键词> ||| <指定程式和参数> ||| <检索第几项的指令> ");
                Console.WriteLine("  栗如: http://|||msedge --inprivate %1|||0 ");
                Console.WriteLine("        (默认的超链接通过 Edge 浏览器打开并启动无痕模式)");
                Console.WriteLine("");
                Console.WriteLine("        explorer %1|||0");
                Console.WriteLine("        (该规则作为默认值, 仅当所有规则均不匹配的时候生效)");
                Console.WriteLine("");
                Console.WriteLine("  完成规则配置后请写入到 \"" + path + "\"");
                Console.WriteLine("");
                Console.WriteLine("然后要关闭此帮助的话, 随便按下哪个键就可以关闭了...");
                Console.ReadKey();
                return;
            }
            else
            {
                string defprog = "explorer";
                string defcomm = "%1";

                //遍历规则
                foreach (var rule in list)
                {

                    //只获取指令不包括程序进程
                    string[] rule2 = rule.Item2.Split(' ');
                    string comm = "";
                    string prog = "";
                    int shuangyinghao = 0; //0 没法线 ; 1 发现了 ; 2不要再看了
                    
                    for (int i = 0; i < rule2.Length; i++)
                    {
                        //判断是否有 ""
                        if (rule2[i].IndexOf("\"") != -1)
                        {
                            if (shuangyinghao == 0)
                            {
                                shuangyinghao++;
                            }
                            else if (shuangyinghao == 1)
                            {
                                shuangyinghao++;
                                prog = prog + " " + rule2[i];
                                continue;
                            }
                        }

                        if(shuangyinghao == 1)
                        {
                            prog = (prog + " " + rule2[i]).TrimStart();
                            /*if(i == 0)
                            {
                                prog = prog.Substring(1);
                            }*/
                        }
                        else
                        {
                            if(i == 0)
                            {
                                prog = rule2[i];
                            }
                            else
                            {
                                comm = (comm + " " + rule2[i]).TrimStart();
                                /*if(i == 1)
                                {
                                    comm = comm.Substring(1);
                                }*/
                            }
                        }



                        if(shuangyinghao == 1 && prog.Length > 1 && prog.Substring(1).IndexOf("\"") != -1)
                        {
                            shuangyinghao++;
                        }
                    }

                    //第一项为空值,说明是默认指令
                    if (rule.Item1 == "")
                    {
                        defprog = prog;
                        defcomm = comm;
                        continue;
                    }

                    //追加指令
                    comm = comm.Replace("%1",string.Join(" ", args));

                    if (Debugger.IsAttached == true)
                    {
                        Console.WriteLine("匹配 " + rule.Item1 + "\r\n启动 " + prog + "\r\n参数 " + comm);
                        Console.WriteLine("");
                    }

                    //真正运行的地方起始是在这里
                    if (args[rule.Item3].IndexOf(rule.Item1) != -1)
                    {
                        Process.Start( prog, comm );
                        return;
                    }
                }

                defcomm = defcomm.Replace("%1", string.Join(" ", args));
                if(Debugger.IsAttached == true)
                {
                    Console.WriteLine("##### 如果你能看到这条讯息, 说明匹配失败哩 #####");
                    Console.WriteLine("按下任意键运行指定默认程式: \r\n" + defprog + " " + defcomm);
                    Console.ReadKey();
                }

                //都不匹配的话就用 explorer 打开吧
                Process.Start( defprog, defcomm );
            }

        }
    }
}
