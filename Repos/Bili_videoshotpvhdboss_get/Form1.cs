using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections;
using static System.Windows.Forms.LinkLabel;
using System.Media;

namespace Bili_videoshotpvhdboss_get
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //哇塞，变量名可以用中文欸，太好了！
        public static class 全局变量
        {
            public static String Buildtime = "2024-4-10";
            public static String 代码串;
            public static String 初始代码串;
            public static int 位1;
            public static int 位2;
            public static int 位3;
            public static int 位4;
            public static int 位5;
            public static int 位6;
            public static int 终位1;
            public static int 终位2;
            public static int 终位3;
            public static int 终位4;
            public static int 终位5;
            public static int 终位6;
            public static String 数值;
            public static String 变化码起始;
            public static String 变化码终止;
            public static String 变化码;
            public static String 后缀;
            public static String 前缀;
            public static String 获取状态;
            public static String 当前时间和日期;
            public static bool Timer1的状态;
            public static int 计数;
            public static string 递增列表 = "0123456789abcdefghijklmnopqrstuvwxyz";
            public static String[] 这是递增列表 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
        }

        //程序刚打开就运行的地方
        private void Form1_Load(object sender, EventArgs e)
        {
            //我太懒了，版本号自己打印
            label6.Text = "v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //创建文件夹
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(@".\Done\", ""));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(@".\Logs\", ""));
        }

        //开始运行
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.toolStripStatusLabel4.Text) != 0 && this.toolStripStatusLabel2.Text != "成功 (200)")
            {
                //先暂停
                //this.timer1.Enabled = false;
                this.toolStripStatusLabel1.Text = "暂停";
                全局变量.Timer1的状态 = false;
                timer1.Stop();
                timer2.Stop();
                //this.button2.Enabled = true;
                //this.button3.Enabled = false;

                SystemSounds.Beep.Play();
                if (MessageBox.Show("上一批的任务还没有完成捏，是否重新运行新任务？", "提示    (´･ω･`)?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    //如果处于激活状态下，继续运行
                    if (this.button3.Enabled == true)
                    {
                        //this.timer1.Enabled = true;
                        this.toolStripStatusLabel1.Text = "进行中...";
                        全局变量.Timer1的状态 = true;
                        timer1.Start();
                        timer2.Start();
                        this.button3.Enabled = true;
                        this.button2.Enabled = false;
                    }
                    return;
                }
            }
            全局变量.初始代码串 = textBox2.Text+textBox5.Text+"_"+textBox3.Text+textBox7.Text;
            //Console.WriteLine(全局变量.初始代码串);
            this.toolStripStatusLabel1.Text = "进行中";
            this.textBox1.Text = "进行中...";
            this.toolStripStatusLabel2.Text = "等待 (000)";
            全局变量.数值 = textBox5.Text;
            全局变量.前缀 = textBox2.Text;
            全局变量.后缀 = textBox7.Text;
            全局变量.变化码 = textBox5.Text;
            全局变量.变化码起始 = textBox5.Text;
            全局变量.变化码终止 = textBox4.Text;
            全局变量.计数 = 0;
            全局变量.代码串 = 全局变量.初始代码串;
            全局变量.当前时间和日期 = DateTime.Now.ToString("yyyy-MM-dd") +"   "+ DateTime.Now.Hour.ToString() +"-"+ DateTime.Now.Minute.ToString() +"-"+ DateTime.Now.Second.ToString();
            //Console.WriteLine(全局变量.当前时间和日期);

            //设置循环时间
            this.timer1.Interval = int.Parse(textBox6.Text);

            //截取字符
            全局变量.位6 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(0, 1));
            全局变量.位5 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(1, 1));
            全局变量.位4 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(2, 1));
            全局变量.位3 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(3, 1));
            全局变量.位2 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(4, 1));
            全局变量.位1 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(5, 1));
            //全局变量.终位1 = int.Parse(textBox4.Text.Substring(0, 1));
            //全局变量.终位2 = int.Parse(textBox4.Text.Substring(1, 1));
            //全局变量.终位3 = int.Parse(textBox4.Text.Substring(2, 1));
            //全局变量.终位4 = int.Parse(textBox4.Text.Substring(3, 1));
            //全局变量.终位5 = int.Parse(textBox4.Text.Substring(4, 1));
            //全局变量.终位6 = int.Parse(textBox4.Text.Substring(5, 1));


            //Console.WriteLine(全局变量.递增列表.IndexOf("o"));

            /*
            Console.WriteLine(全局变量.位1);
            Console.WriteLine(全局变量.位2);
            Console.WriteLine(全局变量.位3);
            Console.WriteLine(全局变量.位4);
            Console.WriteLine(全局变量.位5);
            Console.WriteLine(全局变量.位6);
            Console.WriteLine(全局变量.终位1);
            Console.WriteLine(全局变量.终位2);
            Console.WriteLine(全局变量.终位3);
            Console.WriteLine(全局变量.终位4);
            Console.WriteLine(全局变量.终位5);
            Console.WriteLine(全局变量.终位6);
            */

            /*
            var client = new HttpClient();
            var 输出 = await client.GetAsync(全局变量.初始代码串);
            //var 输出 = await client.GetAsync("https://bimp.hdslb.com/videoshotpvhdboss/1482069818_vifmja-0001.jpg");
            //Console.WriteLine(输出);
            this.textBox1.Text = 输出.ToString();
            //状态码
            //Console.WriteLine((int)输出.StatusCode);
            if (((int)输出.StatusCode).ToString() == "200")
            {
                全局变量.获取状态 = "成功";
            }
            else if (((int)输出.StatusCode).ToString() == "404")
            {
                全局变量.获取状态 = "失败";
            }
            this.toolStripStatusLabel2.Text = 全局变量.获取状态 +  " (" + ((int)输出.StatusCode).ToString() + ")";
            */

            this.timer1.Enabled = true;
            this.timer2.Enabled = true;
            this.button3.Enabled = true;
            this.button2.Enabled = false;
            全局变量.Timer1的状态 = true;
        }

        
        

        class test
        {
            private static readonly HttpClient client = new HttpClient();
            static async Task Main(string[] args)
            {
                //发送Get请求
                var responseString = await client.GetStringAsync("https://bimp.hdslb.com/videoshotpvhdboss/1482069818_vifmja-0001.jpg");
                Console.WriteLine(responseString);
            }
        }


        //循环器
        private async void timer1_Tick(object sender, EventArgs e)
        {
            //计数
            全局变量.计数 = 全局变量.计数 + 1;
            this.toolStripStatusLabel4.Text = 全局变量.计数.ToString();
            //int abc = 12;
            //this.toolStripStatusLabel4.Text = 全局变量.这是递增列表[abc].ToString();
            //this.toolStripStatusLabel3.Text = 全局变量.代码串;

            //打印到状态栏
            this.toolStripStatusLabel3.Text = 全局变量.前缀 + 全局变量.数值 +"_"+ 全局变量.这是递增列表[全局变量.位6].ToString() + 全局变量.这是递增列表[全局变量.位5].ToString() + 全局变量.这是递增列表[全局变量.位4].ToString() + 全局变量.这是递增列表[全局变量.位3].ToString() + 全局变量.这是递增列表[全局变量.位2].ToString() + 全局变量.这是递增列表[全局变量.位1].ToString() + 全局变量.后缀;


            //网络爬取
            timer1.Stop();
            var client = new HttpClient();
            //var 输出 = await client.GetAsync(全局变量.初始代码串);
            var 输出 = await client.GetAsync(this.toolStripStatusLabel3.Text);
            //Console.WriteLine(输出);
            this.textBox1.Text = 输出.ToString();
            //状态码
            //Console.WriteLine((int)输出.StatusCode);
            if (((int)输出.StatusCode).ToString() == "200")
            {
                全局变量.获取状态 = "成功";

                this.button2.Enabled = true;
                this.button3.Enabled = false;
                this.toolStripStatusLabel1.Text = "成功";
                System.IO.File.WriteAllLines(@".\Done\"+ DateTime.Now.ToString("yyyy-MM-dd") +"   "+ DateTime.Now.Hour.ToString() +"-"+ DateTime.Now.Minute.ToString() +"-"+ DateTime.Now.Second.ToString() +".txt", toolStripStatusLabel3.Text.Split());

                SystemSounds.Beep.Play();
                if (MessageBox.Show(this.toolStripStatusLabel3.Text, "成功    q(≧▽≦q)",MessageBoxButtons.OK) == DialogResult.OK)
                {
                    //复制到剪切板
                    Clipboard.SetDataObject(this.toolStripStatusLabel3.Text);
                }
            }
            else if (((int)输出.StatusCode).ToString() == "404")
            {
                全局变量.获取状态 = "失败";
                if (全局变量.Timer1的状态 != false)
                {
                    timer1.Start();
                }
            }
            this.toolStripStatusLabel2.Text = 全局变量.获取状态 +  " (" + ((int)输出.StatusCode).ToString() + ")";
            //内存回收
            client.Dispose();
            输出.Dispose();
            输出 = null;
            client = null;  
            GC.Collect();
            


            //网址遍历
            全局变量.位1 = 全局变量.位1 + 1;
            if (全局变量.位1 > 35)
            {
                全局变量.位1 = 0;
                全局变量.位2 = 全局变量.位2 + 1;
            }
            if (全局变量.位2 > 35)
            {
                全局变量.位2 = 0;
                全局变量.位3 = 全局变量.位3 + 1;
            }
            if (全局变量.位3 > 35)
            {
                全局变量.位3 = 0;
                全局变量.位4 = 全局变量.位4 + 1;
            }
            if (全局变量.位4 > 35)
            {
                全局变量.位4 = 0;
                全局变量.位5 = 全局变量.位5 + 1;
            }
            if (全局变量.位5 > 35)
            {
                全局变量.位5 = 0;
                全局变量.位6 = 全局变量.位6 + 1;
            }
            if (全局变量.位6 > 35)
            {
                全局变量.位6 = 0;

                //this.timer1.Enabled = false;
                全局变量.Timer1的状态 = false;
                timer1.Stop();
                timer2.Stop();
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.toolStripStatusLabel1.Text = "失败";
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("我尽力了QwQ", "失败    ╥﹏╥...");
            }
            
        }

        //继续
        private void button2_Click(object sender, EventArgs e)
        {
            //this.timer1.Enabled = true;
            this.toolStripStatusLabel1.Text = "进行中...";
            全局变量.Timer1的状态 = true;
            timer1.Start(); 
            timer2.Start();
            this.button3.Enabled = true;
            this.button2.Enabled = false;
        }
        //暂停
        private void button3_Click(object sender, EventArgs e)
        {
            //this.timer1.Enabled = false;
            this.toolStripStatusLabel1.Text = "暂停";
            全局变量.Timer1的状态 = false;
            timer1.Stop();  
            timer2.Stop();
            this.button2.Enabled = true;
            this.button3.Enabled = false;
        }

        //写日志
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                System.IO.File.WriteAllLines(@".\Logs\"+ 全局变量.当前时间和日期 +".txt", toolStripStatusLabel3.Text.Split());
            }
        }

        //关于
        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bili_videoshotpvhdboss_get\nBy：yuhang0000\nBuild Time：" + 全局变量.Buildtime + "\n版本号：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() , "关于");
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            if (this.toolStripStatusLabel3.Text != "https://bimp.hdslb.com/videoshotpvhdboss/...")
            { 
                Clipboard.SetDataObject(this.toolStripStatusLabel3.Text);
            }
        }
    }
}
