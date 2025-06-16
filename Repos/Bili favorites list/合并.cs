using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
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

        //public static StringBuilder output = new StringBuilder();
        public static Dictionary<long, string[]> output = new Dictionary<long, string[]> { };
        public static Dictionary<string, string> ESC = new Dictionary<string, string> {
            { "\r",@"\r"},
            { "\n",@"\n"},
        }; //转义字符

        private void 合并_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.button5.Enabled = true;
            GC.Collect();
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
            else
            {
                return;
            };
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
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
            output.Clear();
            this.label3.Text = "进度: 0";
            this.label4.Text = "总计: 计算中...";
            this.label5.Text = "已用时间: 00:00:00";
            this.label6.Text = "剩余时间: 00:00:00";
            await Task.Run(() => { run(this.textBox1.Text); });
        }

        public void run(string files)
        {
            processbar(-1);
            List<string[]> list = new List<string[]>();
            long num = 0; //总数
            long num1 = 0; //当前数
            float num2 = 0; //历史数
            int time = 0;

            try
            {
                //计时器
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 100;
                timer.AutoReset = true;
                timer.Elapsed += new System.Timers.ElapsedEventHandler((object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    time = time + 100;
                    //Console.WriteLine(num1);
                    UIupdate(this.label3, "进度: " + num1.ToString());
                    UIupdate(this.label5, "已用时间: " + totime(time));
                    if (num1 > 0)
                    {
                        processbar((int)((100 * num1) / num));
                    }

                    //求剩余时间
                    num2 = (float)num / (float)num1;
                    num2 = (float)time * num2;
                    num2 = num2 - (float)time;
                    UIupdate(this.label6, "剩余时间: " + totime((int)num2));
                    num2 = num1;
                });

                //读取文档
                string[] input = files.Split(';');
                foreach (string file in input)
                {
                    string[] text = File.ReadAllText(file).Split(new string[] { "\r\n#\t" },StringSplitOptions.None);
                    if (text.LongLength > 1)
                    {
                        list.Add(text);
                        num = num + text.LongLength - 1;
                    }
                }
                UIupdate(this.label4, "总计: " + num.ToString());
                timer.Start();


                //处理一拖拉库的文档
                foreach (string[] file in list)
                {
                    file[0] = "";
                    foreach (string filelin in file) //处理单个文档
                    {
                        string fileline = filelin;
                        if(fileline.Length == 0)
                        {
                            continue;
                        }
                        //转义那些字符
                        foreach (var esc1 in ESC)
                        {
                            if(fileline.IndexOf(esc1.Key) != -1)
                            {
                                fileline = fileline.Replace(esc1.Key, esc1.Value);
                            }
                        }
                        string[] text = fileline.Split('\t');
                        output.Add(long.Parse(text[0].Replace("ML","")),text);
                        num1++;
                    }
                }

                timer.Stop();
                UIupdate(this.label3, "进度: " + num1.ToString());
                processbar(-1);
                output = output.OrderBy(x => x.ToString()).ToDictionary(o => o.Key,p => p.Value);
                long max = output.Keys.Max();
                long min = output.Keys.Min();

                //到这里就完成了
                SaveFileDialog dig = new SaveFileDialog();
                dig.Filter = "文本文档(*.txt)|*.txt";
                dig.Title = "保存文档";
                dig.DefaultExt = "*.txt";
                dig.FileName = max.ToString() + " - " + min.ToString() + ".txt";
                dig.CheckPathExists = true;
                dig.AddExtension = true;
                dig.AutoUpgradeEnabled = true;
                dig.OverwritePrompt = true;
                dig.RestoreDirectory = true;
                dig.InitialDirectory = System.Windows.Forms.Application.StartupPath;

                time = 0;
                timer.Start();
                num1 = 0;
                string text1 = "";
                foreach (string[] text2 in output.Values)
                {
                    num1++;
                    string text3 = "";
                    foreach (string text4 in text2)
                    {
                        text3 = text3 + "\t" + text4;
                    }
                    text3 = text3.Substring(1);
                    text1 = text1 + "\r\n" + text3;
                }
                text1 = text1.Substring(1);

                this.Invoke(new MethodInvoker( () =>
                {
                    if (dig.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dig.FileName, text1);
                    };
                }));

                processbar(100);
                output.Clear();
                timer.Dispose();
                enablebutton();

            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(ex.ToString(), "Oops! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                enablebutton();
                return;
            }
            finally
            {
                list.Clear();
                //GC.Collect();
            }

            void enablebutton()
            {
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
                if (ttttt > 60)
                {
                    mm = (ttttt / 60).ToString();
                }
                if(long.Parse(mm) > 60)
                {
                    hh = (long.Parse(mm) / 60).ToString();
                }
                ss = (ttttt - (long.Parse(mm) * 60) + (long.Parse(hh) * 3600) ).ToString();
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

        public void processbar(int num)
        {
            if (this.IsHandleCreated == false)
            {
                if (num == -1)
                {
                    this.progressBar1.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    this.progressBar1.Style = ProgressBarStyle.Blocks;
                    this.progressBar1.Value = num;
                }
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (num == -1)
                    {
                        this.progressBar1.Style = ProgressBarStyle.Marquee;
                    }
                    else
                    {
                        this.progressBar1.Style = ProgressBarStyle.Blocks;
                        this.progressBar1.Value = num;
                    }
                }));
            }
        }

    }
}
