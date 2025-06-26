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
            await Task.Run(() => { run(this.textBox1.Text.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries)); });
        }

        public void run(string[] files)
        {
            processbar(this.progressBar2, -1);

            long num = 0; //总数
            long num1 = 0; //当前数
            float num2 = 0; //历史数
            long num3 = 0; //已处理文件数
            long time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(); //开始时间
            long time1; //当前时间
            StringBuilder sb = new StringBuilder(); //Dump //二改, 拿去手搓读取 流 去了
            long readstreamlength = 1024 * 1024; //在字节流设定读取长度
            byte[] buffer = new Byte[readstreamlength]; //Dump 但是以字节流的形式
            string text; //读取的文件放这里
            string[] textsub; //单个条目
            string texttemp; //零食用
            List<long> textindex = new List<long> { }; //存放序号
            //SortedSet<long> textinedx1 = new SortedSet<long>();
            long no1 = 0; //上一行编号
            long no2 = 0; //下一行编号
            string savepath; //文档保存未知

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
                sb.Clear();
                sb.Append("#####  " + DateTime.Now.ToString() + "  #####\r\n\r\n");
                timer.Start();
                if(files.Length == 0)
                {
                    throw new Exception("没有选择文件");
                }

                //读取文件
                UIupdate(this.label7, "已处理文件: (" + num3 + "/" + files.LongLength + ")");
                processbar(this.progressBar2,0);
                foreach (string file in files)
                {
                    if (run(file) == false)
                    {
                        continue;
                    }
                }

                //输出
                timer.Stop();
                if(textindex.Count == 0)
                {
                    throw new Exception("无效内容");
                }
                
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
                    }
                }));
                dig.Dispose();

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
                return;
            }
            finally
            {
                processbar(this.progressBar1,1000);
                processbar(this.progressBar2,1000);
                enablebutton();
            }

            bool run(string file)
            {
                string readline2(StreamReader srr)
                {
                    bool find = false;
                    sb.Clear();
                    while(find != true)
                    {
                        int t = srr.Read();
                        if(t == -1)
                        {
                            return sb.ToString();
                        }
                        sb.Append((char)t);
                        if (sb.ToString().IndexOf("\r\t") != -1)
                        {
                            find = true; //虽说这里是多余的
                            return sb.ToString();
                        }
                    }
                    return null;
                }

                num1 = 0;
                if(File.Exists(file) == false) //文档不存在
                {
                    return false;
                }
                using(FileStream fsw = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    num = fsw.Length;
                    UIupdate(this.label4, "总计: " + num);
                    using (StreamReader sr = new StreamReader(fsw))
                    {
                        while ( (text = sr.ReadLine()) != null)
                        {
                            if(num1 < 1 && text.IndexOf("  #####") == -1) //无效文件
                            {
                                return false;
                            }
                            //Debugger.Break();

                            textsub = text.Split('\t');

                            num1 = fsw.Position;
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

            void enablebutton()
            {
                timer.Stop();
                timer.Dispose();
                sb.Clear();
                sb = null;
                textindex.Clear();
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
