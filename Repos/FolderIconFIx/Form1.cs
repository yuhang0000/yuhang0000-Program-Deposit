using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderIconFIx
{
    public partial class Form1 : Form
    {
        //构造函数
        public Form1()
        {
            InitializeComponent();
            this.Text = Application.ProductName;
            this.StatusTextVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        /// <summary>
        /// 当前模式, false: 搜索; true: 执行
        /// </summary>
        public bool ModeCode = false;
        /// <summary>
        /// 运行状态
        /// </summary>
        public bool StatusCode = false;
        /// <summary>
        /// 暂存 Desktop.ini 路径
        /// </summary>
        public List<string> DesktopPaths = new List<string>();   

        //关于
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            String 康派利 = Application.CompanyName;
            String 奈姆 = Assembly.GetExecutingAssembly().GetName().Name;
            String 版本 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            DateTime 编译时间 = File.GetLastWriteTime(typeof(Form1).Assembly.Location);
            String text = 奈姆 + "\r\nBy: " + 康派利 +"\r\nBuild Time: " + 编译时间 + "\r\n版本号: " + 版本;
            MessageBox.Show(text, "关于");
        }

        //退出
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //打开资料夹选择器
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选定指定资料夹. ";
            dialog.SelectedPath = this.textBox1.Text;
            if(dialog.ShowDialog() == DialogResult.OK && dialog.SelectedPath != null && dialog.SelectedPath.Length > 0)
            {
                this.textBox1.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="target">给定时间对象, 缺省时取当前时间</param>
        /// <returns>long: 时间戳</returns>
        public static long GetTimeStamp(DateTime? target = null)
        {
            DateTime now;
            if (target == null)
            {
                now = DateTime.Now;
            }
            else
            {
                now = (DateTime)target;
            }
            DateTime old = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long timestamp = (long)(now - old).TotalSeconds;

            return timestamp;
        }

        // 搜索 & 开始 & 终止
        private void button3_Click(object sender, EventArgs e)
        {
            if(this.StatusCode == false)
            {
                this.StatusCode = true;
                this.BtnStart.Enabled = false;
                //搜索
                if (this.ModeCode == false)
                {
                    Sreach();
                }
                //开始
                else
                {
                    Start();
                }
            }
            //终止
            else
            {
                this.StatusCode = false;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        public void Sreach()
        {
            if(this.textBox1.Text.Length == 0)
            {
                this.StatusCode = false;
                this.BtnStart.Enabled = true;
                SystemSounds.Hand.Play();
                return;
            }
            string[] Folders;
            try
            {
                long starttime = GetTimeStamp();
                Folders = Directory.GetDirectories(this.textBox1.Text);
                this.textBox2.Clear();
                this.DesktopPaths.Clear();
                foreach(string folder in Folders)
                {
                    if (File.Exists(folder + "\\Desktop.ini") == true)
                    {
                        this.DesktopPaths.Add(folder + "\\Desktop.ini");
                        this.textBox2.AppendText(folder + "\\Desktop.ini" + "\r\n");
                    }
                }
                long lasttime = GetTimeStamp();
                this.textBox2.AppendText("搜索用时: " + (lasttime - starttime).ToString() + "s");
                this.StatusTextNum.Text = "0/" + this.DesktopPaths.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("遍历文件夹时出现了问题, 原因是: \r\n" + ex.ToString(),"Oops",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.StatusCode = false;
                this.BtnStart.Enabled = true;
                SystemSounds.Hand.Play();
            }
            this.StatusCode = false;
            this.ModeCode = true;
            this.BtnStart.Text = "开始";
            this.BtnStart.Enabled = true;
        }
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            this.StatusCode = true;
            this.BtnStart.Enabled = false;

            int finishnum = 0;
            foreach(string folder in this.DesktopPaths)
            {
                try
                {
                    string[] texts = File.ReadAllLines(folder);
                    //遍历每行文本
                    string iconpath = ""; //图标路径
                    string tmp1 = "IconResource=";
                    int index = 0; //暂存索引
                    for(int i = 0; i < texts.Length; i++)
                    {
                        string text = texts[i];
                        if(text.IndexOf(tmp1)  != -1)
                        {
                            iconpath = text.Substring(tmp1.Length, text.IndexOf(","));
                            index = i;
                        }
                    }
                    if(iconpath != "")
                    {
                        string[] tmp2; //吧路径切成腻子, 图标路径
                        string[] tmp3; //吧路径切成腻子, 当前文件夹路径
                        tmp2 = iconpath.Split('\\');
                        tmp3 = folder.Split('\\');
                        if (tmp2[0] !=  tmp3[0] || tmp2[1] == "." || tmp2[1] == "..") //不在同个盘符或者已经设置了相对路径, 就跳过
                        {
                            //跳过
                            //continue;
                        }
                        else
                        {
                            int lastindex = tmp3.Length - 2; //遍历每个腻子, 这个记录, 当遇到比较不同文件夹名称时, 最后的索引, 默认是文件夹所在路径, 去除 Desktop.ini 和盘符的数组长度
                            for(int i = 1; i < tmp3.Length - 1;i++) //跳过第一个和最后一个
                            {
                                if (tmp2[i] == tmp3[i])
                                {
                                    continue;
                                }
                                else
                                {
                                    lastindex = i - 1;
                                }
                            }
                            string newiconpath = "";
                            //往前
                            if(lastindex < tmp2.Length - 2)
                            {
                                for(int i = lastindex; i < tmp2.Length - 1; i++)
                                {
                                    newiconpath = "..\\" +  newiconpath + tmp2[i];
                                }
                            }
                            //往后
                            else
                            {
                                newiconpath = ".\\";
                            }
                        }
                    }

                    finishnum++;
                    this.StatusTextNum.Text = finishnum.ToString() +" /" + this.DesktopPaths.Count.ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("修补文件时出现了问题, 原因是: \r\n" + ex.ToString(), "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.StatusCode = false;
                    this.BtnStart.Enabled = true;
                    SystemSounds.Hand.Play();
                }
            }
            this.StatusCode = false;
            this.BtnStart.Enabled = true;
        }

        //路径文本礦变更时
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.ModeCode = false;
            this.BtnStart.Text = "搜索";
        }
    }
}
