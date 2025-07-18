using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static Bili_favorites_list.Form1;
using static System.Net.Mime.MediaTypeNames;

namespace Bili_favorites_list
{
    public partial class 合并 : Form
    {
        public 合并()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
        }

        public static Dictionary<string, string> ESC = new Dictionary<string, string> {
            { "\r",@"\r"},
            { "\n",@"\n"},
        }; //转义字符

        private void 合并_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.button5.Enabled = true;
            //GC.Collect();
        }

        //打开
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dig = new OpenFileDialog();
            dig.Title = "选择文件";
            dig.Filter = "文本文件(*.txt)|*.txt";
            dig.Multiselect = true;
            dig.CheckFileExists = true;
            dig.AddExtension = true;
            dig.AutoUpgradeEnabled = true;
            dig.CheckPathExists = true;
            dig.DefaultExt = "*.txt";
            dig.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            dig.RestoreDirectory = true;
            if (dig.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = "";
                foreach (string s in dig.FileNames)
                {
                    string file = s;
                    /*if(s.IndexOf(" ") != -1)
                    {
                        file = "\"" + file + "\"";
                    }*/
                    this.textBox1.Text = this.textBox1.Text + ";" + file;
                }
                this.textBox1.Text = this.textBox1.Text.Substring(1);
            }
            dig.Dispose();
        }

        //保存位置
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dig = new FolderBrowserDialog();
            dig.Description = "选择输出位置";
            //dig.SelectedPath = this.textBox1.Text.Substring(0,this.textBox1.Text.LastIndexOf("\\"));
            dig.SelectedPath = System.Windows.Forms.Application.StartupPath;
            dig.ShowNewFolderButton = true;
            if (dig.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dig.SelectedPath;
            };
            dig.Dispose();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
            this.label3.Text = "进度: 0";
            this.label4.Text = "总计: 计算中...";
            this.label5.Text = "已用时间: 00:00:00";
            this.label6.Text = "剩余时间: 00:00:00";
            this.Text = "合并数据: 准备中...";
            await Task.Run(() => { run(this.textBox1.Text.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries)); });
        }

        public void run(string[] files)
        {
            processbar(this.progressBar2, -1);

            long num = 0; //总数
            long num1 = 0; //当前数
            float num2 = 0; //历史数
            long num3 = 0; //已处理文件数
            long num4 = 0; //总文件数
            long time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(); //开始时间
            long time1; //当前时间
            StringBuilder sb = new StringBuilder(); //Dump //二改, 拿去手搓读取 流 去了
            //long readstreamlength = 1024 * 1024; //在字节流设定读取长度
            //byte[] buffer = new Byte[readstreamlength]; //Dump 但是以字节流的形式
            int cachesize = 4096; //文件读取缓冲区大小
            string text; //读取的文件放这里
            string[] textsub = { }; //单个条目
            string texttemp; //零食用
            long[] textindex = new long[] { -1, -1, -1, 0 }; //存放序号 //0: 最大值 1: 最小值 2:上一个数 3:总数据量
            //SortedSet<long> textinedx1 = new SortedSet<long>();
            //long no1 = 0; //上一行编号
            //long no2 = 0; //下一行编号
            string savepath; //文档保存未知
            string savepathtemp = AppDomain.CurrentDomain.BaseDirectory + @"Output.tmp"; //零食文档保存未知
            bool canisort = false; //是否要求排序
            List<long> sorttempfilelist = new List<long>()
            {
                -1,
            }; //零食存放排序文件序号

            //计时器
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler( (object obj, ElapsedEventArgs e) =>
            {
                time1 = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                time1 = time1 - time;
                UIupdate(this.label3,"进度: " + num1.ToString());
                UIupdate(this.label5,"已用时间: " + totime(time1 * 1000));
                processbar(this.progressBar1, (int)((1000 * num1) / num));

                //num2 = (float)num3 / (float)files.LongLength; //仅文件
                num2 = (float)(num3 * num + num1) / (float)(files.LongLength * num);
                num2 = (float)(time1 * 1000) / num2;
                UIupdate(this.label6,"剩余时间: " + totime( (int)(num2 - (float)(time1 * 1000) ) ));
            });

            try
            {
                //初始化
                num4 = files.LongLength;
                if (File.Exists(savepathtemp) == true)
                {
                    File.Delete( savepathtemp );
                }
                if (Directory.Exists(System.Windows.Forms.Application.StartupPath + "/Temp") == true)
                {
                    Directory.Delete(System.Windows.Forms.Application.StartupPath + "/Temp", true);
                }
                sb.Clear();
                if(num4 == 0)
                {
                    throw new Exception("没有选择文件");
                }
                //sb.Append("#####  " + DateTime.Now.ToString() + "  #####\r\n\r\n");
                File.WriteAllText(savepathtemp, "#####  " + DateTime.Now.ToString() + "  #####\r\n\r\n");
                timer.Start();

                //读取文件
                UIupdate(this.label7, "已处理文件: (" + num3 + "/" + num4 + ")");
                processbar(this.progressBar2,0);
                foreach (string file in files)
                {
                    if (run(file) == false)
                    {
                        continue;
                    }
                }

                //排序
                if (textindex[2] == -1)
                {
                    throw new Exception("无效内容");
                }
                if (canisort == true)
                {
                    processbar(this.progressBar1, -1);
                    processbar(this.progressBar2, -1);
                    this.Invoke(new MethodInvoker(() => { this.Text = "合并数据: 排序中"; }));
                    sort();
                }

                //输出
                this.Invoke(new MethodInvoker(() => { this.Text = "合并数据: 导出中..."; }));
                timer.Stop();
                
                //选择文档保存未知
                SaveFileDialog dig = new SaveFileDialog();
                dig.AddExtension = true;
                dig.AutoUpgradeEnabled = true;
                dig.CheckPathExists = true;
                //dig.CreatePrompt = true;
                dig.DefaultExt = "*.txt";
                dig.FileName = textindex.Min().ToString() + " - " + textindex.Max().ToString() + ".txt";
                dig.Filter = "文本文档(*.txt)|*.txt";
                dig.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                dig.OverwritePrompt = true;
                dig.RestoreDirectory = true;
                dig.Title = "选择文档保存位置";
                this.Invoke(new MethodInvoker( () =>
                {
                    if(dig.ShowDialog() == DialogResult.OK)
                    {
                        savepath = dig.FileName;
                        //texttemp = sb.ToString();
                        //File.WriteAllText(dig.FileName, texttemp);
                        if (File.Exists(savepath) == true) //善待哦旧的
                        {
                            File.Delete(savepath);
                        }
                        if(Directory.Exists(System.Windows.Forms.Application.StartupPath + "/Temp") == true)
                        {
                            Directory.Delete(System.Windows.Forms.Application.StartupPath + "/Temp", true);
                        }
                        File.Move(savepathtemp, savepath);
                    }
                    else
                    {
                        if (File.Exists(savepathtemp) == true) //清楚残留文件
                        {
                            File.Delete(savepathtemp);
                        }
                        if(Directory.Exists(System.Windows.Forms.Application.StartupPath + "/Temp") == true)
                        {
                            Directory.Delete(System.Windows.Forms.Application.StartupPath + "/Temp", true);
                        }
                    }
                }));
                dig.Dispose();
                this.Invoke(new MethodInvoker(() => { this.Text = "合并数据: 完成"; }));

            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                if(Debugger.IsAttached == true)
                {
                    MessageBox.Show(ex.ToString(),"Oops! ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message,"Oops! ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                this.Invoke(new MethodInvoker(() => { this.Text = "合并数据: 失败哩"; }));
                return;
            }
            finally
            {
                processbar(this.progressBar1,1000);
                processbar(this.progressBar2,1000);
                enablebutton();
            }

            //读取流数据
            string readline2(StreamReader srr, string key = "\r\t#\t")
            {
                bool find = false;
                sb.Clear();
                while (find != true)
                {
                    int t = srr.Read();
                    if (t == -1) //读完了
                    {
                        if (sb.Length == 0)
                        {
                            return null; 
                        }
                        else
                        {
                            return sb.ToString();
                        }
                    }
                    sb.Append((char)t);
                    //if (sb.ToString().IndexOf(key) != -1)
                    if (sb.Length > 4 && sb[sb.Length - 1] == '\t' && sb[sb.Length - 2] == '#' &&
                        sb[sb.Length - 3] == '\n' && sb[sb.Length - 4] == '\r')
                    {
                        find = true; //虽说这里是多余的
                        return sb.ToString().Substring(0, sb.Length - 4); //去掉后面 4 个
                    }
                }
                return null;
            }

            //初合并
            bool run(string file)
            {
                num1 = 0;
                if(File.Exists(file) == false) //文档不存在
                {
                    return false;
                }
                this.Invoke( new MethodInvoker(() =>{ this.Text = "合并数据: 合并中..."; } ));
                using(FileStream fsr = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, cachesize))
                {
                    num = fsr.Length;
                    UIupdate(this.label4, "总计: " + num);
                    using (StreamReader sr = new StreamReader(fsr))
                    {

                        using (FileStream fsw = new FileStream(savepathtemp, FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(fsw))
                            {
                                //真正循环在这里
                                //while ( (text = sr.ReadLine()) != null)
                                while ((text = readline2(sr)) != null)
                                {
                                    if (num1 < 1 && text.IndexOf("  #####") == -1) //无效文件
                                    {
                                        return false;
                                    }
                                    else if (num1 < 1)
                                    {
                                        num1 = fsr.Position;
                                        continue;
                                    }
                                    //Debugger.Break();

                                    textsub = text.Split('\t');
                                    //textindex.Add(long.Parse(textsub[0].Substring(2))); //添加索引
                                    //索引赋值
                                    texttemp = textsub[0].Substring(2);
                                    if (textindex[0] == -1 && textindex[1] == -1 && textindex[2] == -1) //初始化的值
                                    {
                                        textindex[0] = long.Parse(texttemp);
                                        textindex[1] = long.Parse(texttemp);
                                        textindex[2] = long.Parse(texttemp);
                                    }
                                    else
                                    {
                                        //存储最大校址
                                        if ( textindex[0] < long.Parse(texttemp) ){
                                            textindex[0] = long.Parse(texttemp);
                                        }
                                        else if ( textindex[1] > long.Parse(texttemp) ){
                                            textindex[1] = long.Parse(texttemp);
                                        }
                                        //如果当前序号比上一个小, 就重新排序
                                        if (canisort == false && textindex[2] > long.Parse(texttemp) )
                                        {
                                            canisort = true;
                                        }

                                        textindex[2] = long.Parse(texttemp);
                                    }
                                    textindex[3] = textindex[3] + 1;

                                    foreach(var t in ESC) //转义字符
                                    {
                                        textsub[2] = textsub[2].Replace(t.Key, t.Value);
                                        textsub[3] = textsub[3].Replace(t.Key, t.Value);
                                        textsub[5] = textsub[5].Replace(t.Key, t.Value);
                                    }
                                    sw.WriteLine("#\t" + string.Join("\t", textsub));
                                    num1 = fsr.Position;
                                }

                                sw.Flush();
                            }
                        }

                    }
                }

                num3++;
                UIupdate(this.label3, "进度: " + num1);
                UIupdate(this.label7, "已处理文件: (" + num3 + "/" + files.LongLength + ")");
                processbar(this.progressBar1, 1000);
                processbar(this.progressBar2, (int)((1000 * num3) / files.LongLength));
                return true;
            }

            //排序
            bool sort()
            {
                this.Invoke(new MethodInvoker( () => { this.Text = "合并数据: 排序中 > 拆分"; }));
                using (FileStream fsr = new FileStream(savepathtemp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, cachesize))
                {
                    num1 = 0;
                    num = textindex[3];
                    num3 = 0;
                    UIupdate(this.label4, "总计: " + num.ToString());
                    UIupdate(this.label3, "当前: 0");
                    processbar (this.progressBar2, 0);
                    Directory.CreateDirectory( System.Windows.Forms.Application.StartupPath + "\\Temp");
                    textindex[2] = -1;
                    using (StreamReader sr = new StreamReader(fsr))
                    {
                        bool sortstatus = false;
                        string sorttemptext = null;
                        while (sortstatus != true)
                        {
                            (sortstatus,sorttemptext) = sortspilt(sr,sorttemptext);
                        }
                        fsr.Dispose();
                        sr.Dispose();
                    }
                    
                    //拆分不连续项目
                    (bool,string) sortspilt(StreamReader sr, string text1 = null){
                        //创建零食文件
                        if (text1 != null)
                        {
                            File.WriteAllText(System.Windows.Forms.Application.StartupPath + "\\Temp\\Temp.tmp", "#\t" + text1 + "\r\n");
                        }
                        else
                        {
                            File.Create(System.Windows.Forms.Application.StartupPath + "\\Temp\\Temp.tmp").Close();
                        }

                        using (FileStream fsw = new FileStream(System.Windows.Forms.Application.StartupPath + "\\Temp\\Temp.tmp", FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(fsw))
                            {
                                long num2split = 0;
                                while ((text = readline2(sr)) != null)
                                {
                                    if (num2split < 1 && text.IndexOf("  #####\r\n") != -1)
                                    {
                                        continue;
                                    }
                                    else if(num2split < 1) //去掉开头的 "#\t"
                                    {
                                        text = text.TrimStart('#', '\t');
                                    }
                                    num1++;
                                    num2split++;
                                    textsub = text.Substring(2).Split('\t');
                                    if (num1 == 1)
                                    {
                                        sorttempfilelist[sorttempfilelist.Count - 1] = long.Parse(textsub[0]);
                                        textindex[2] = int.Parse(textsub[0]) - 1;
                                    }
                                    //if (textindex[2] > int.Parse(textsub[0])) //如果小了
                                    if (textindex[2] + 1 != int.Parse(textsub[0])) //如果小了
                                    {
                                        //sorttempfilelist[sorttempfilelist.Count - 1] = sorttempfilelist[sorttempfilelist.Count - 1] + " - " + textindex[2];
                                        sw.Flush();
                                        sw.Dispose();
                                        fsw.Dispose();
                                        break; //跳出循环
                                    }
                                    textindex[2] = int.Parse(textsub[0]);
                                    //sw.Write("#\t" + text + "\r\n");
                                    //text = text.TrimStart('#', '\t');
                                    text = text.TrimEnd('\r', '\n');
                                    sw.WriteLine("#\t" + text);
                                }
                                /*if (text == null)
                                {
                                    return (true, null);
                                }*/
                            }
                        }

                        //跳出文档流, 然后移动文件
                        File.Move(System.Windows.Forms.Application.StartupPath + "\\Temp\\Temp.tmp",
                            System.Windows.Forms.Application.StartupPath + "\\Temp\\" +
                            sorttempfilelist[sorttempfilelist.Count - 1] + ".txt");
                        sorttempfilelist.Add( long.Parse(textsub[0]) );
                        //File.Create(System.Windows.Forms.Application.StartupPath + "/Temp/Temp.tmp");
                        textindex[2] = int.Parse(textsub[0]);

                        if (text == null)
                        {
                            return (true, null);
                        }
                        else
                        {
                            return (false, text);
                        }
                    }
                }
                //num1 = num;
                //给已存储的零食序号排序, 偷懒, 直接用 .sort()
                sorttempfilelist.Sort();
                this.Invoke(new MethodInvoker( () => { this.Text = "合并数据: 排序中 > 合并"; }));
                File.Delete(savepathtemp);
                UIupdate(this.label7,"已处理文件: (0/" + sorttempfilelist.Count + ")");
                num1 = 0;
                File.WriteAllText(savepathtemp, "#####  " + DateTime.Now.ToString() + "  #####\r\n\r\n");
                foreach (long tempfileindex in sorttempfilelist)
                {
                    using (FileStream fsr = new FileStream(System.Windows.Forms.Application.StartupPath + "\\Temp\\" + tempfileindex.ToString() + ".txt",
                         FileMode.Open, FileAccess.Read, FileShare.ReadWrite, cachesize))
                    {
                        using (StreamReader sr = new StreamReader(fsr))
                        {

                            using (FileStream fsw = new FileStream(savepathtemp, FileMode.Append, FileAccess.Write))
                            {
                                using (StreamWriter sw = new StreamWriter(fsw))
                                {
                                    long num2hebing = 0;
                                    //sw.Write( "#####  " + DateTime.Now.ToString() + "  #####\r\n\r\n");
                                    while ((text = readline2(sr)) != null)
                                    {
                                        if (num2hebing < 1 && text.IndexOf("  #####\r\n") != -1)
                                        {
                                            continue;
                                        }
                                        else if(num2hebing < 1)
                                        {
                                            text = text.TrimStart('#','\t');
                                        }
                                        num1++;
                                        num2hebing++;
                                        //sw.Write("#\t" + text + "\r\n");
                                        text = text.TrimEnd('\r','\n');
                                        sw.WriteLine("#\t" + text);
                                    }
                                    sw.Flush();
                                    sw.Dispose();
                                    fsw.Dispose();
                                }
                            }
                        }
                    }
                    num2++;
                    UIupdate(this.label7,"已处理文件: (" + num2 + "/" + sorttempfilelist.Count + ")");
                }

                this.Invoke(new MethodInvoker( () => { this.Text = "合并数据: 排序中 > 完成"; }));
                return true;
            }

            //任务结束后运行
            void enablebutton()
            {
                timer.Stop();
                timer.Dispose();
                sb.Clear();
                sb = null;
                //textindex.Clear();
                textindex = null;
                texttemp = null;

                if (this.IsHandleCreated == false)
                {
                    this.button3.Enabled = true;
                }
                else
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.button3.Enabled = true;
                    }));
                }

                GC.Collect();
            }

            string totime(long tttt)
            {
                if (tttt < 0)
                {
                    return "00:00:00";
                }
                long ttttt = tttt / 1000;
                string hh = "0";
                string mm = "0";
                string ss = "0";
                if(ttttt > 3600)
                {
                    hh = (ttttt / 3600).ToString();
                }
                if (ttttt > 60)
                {
                    mm = ((ttttt / 60) - long.Parse(hh) * 60).ToString();
                }
                ss = (ttttt - (long.Parse(hh) * 3600) - (long.Parse(mm) * 60) ).ToString();
                if(ss.Length < 2)
                {
                    ss = "0" + ss;
                }
                if(mm.Length < 2)
                {
                    mm = "0" + mm;
                }
                if(hh.Length < 2)
                {
                    hh = "0" + hh;
                }
                return hh + ":" + mm + ":" + ss;
            }
        }

        public void UIupdate(Control control,string text)
        {
            if (this.IsHandleCreated == false)
            {
                control.Text = text;
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    control.Text = text;
                }));
            }
        }

        public void processbar(ProgressBar bar,int num)
        {
            if (this.IsHandleCreated == false)
            {
                if (num == -1)
                {
                    bar.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    bar.Style = ProgressBarStyle.Blocks;
                    bar.Value = num;
                }
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (num == -1)
                    {
                        bar.Style = ProgressBarStyle.Marquee;
                    }
                    else
                    {
                        bar.Style = ProgressBarStyle.Blocks;
                        bar.Value = num;
                    }
                }));
            }
        }

    }
}
