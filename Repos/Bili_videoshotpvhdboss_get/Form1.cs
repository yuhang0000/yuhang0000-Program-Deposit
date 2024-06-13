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
            public static String Buildtime = "2024-6-13";
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
            public static bool 网络循环操作的状态;
            public static bool 错误弹窗的状态;
            public static int 错误计数 = 0;
            public static int 计数;
            public static String[] 这是配置文档诶;
            public static string 纯数字数列 = "0123456789";
            public static string 递增列表 = "0123456789abcdefghijklmnopqrstuvwxyz";
            public static String[] 这是递增列表 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
        }

        //程序刚打开就运行的地方
        private void Form1_Load(object sender, EventArgs e)
        {
            //我太懒了，版本号自己打印
            string 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label6.Text = "v"+ 版本;
            //创建文件夹
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(@".\Done\", ""));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(@".\Logs\", ""));
            //检查配置文件在不在喽
            if (System.IO.File.Exists(@".\Setting.ini") == false)
            {
                string[] 这是配置文档诶 = {"版本=" + 版本, "编辑=" + textBox2.Text, "数值=" + textBox5.Text, "起始=" + textBox3.Text, "终止=" + textBox4.Text, "末尾=" + textBox7.Text, "周期=" + textBox6.Text, "上一次的进度=", "", "[By:yuhang0000]" };
                System.IO.File.WriteAllLines(@".\Setting.ini", 这是配置文档诶);
            }
            else 
            {
                try //我爱死 try 和 catch 这两个方法啦，爱用🤍
                { 
                    全局变量.这是配置文档诶 = System.IO.File.ReadAllLines(@".\Setting.ini");
                    this.textBox2.Text = 全局变量.这是配置文档诶[1].Replace("编辑=", "");
                    this.textBox5.Text = 全局变量.这是配置文档诶[2].Replace("数值=", "");
                    this.textBox3.Text = 全局变量.这是配置文档诶[3].Replace("起始=", "");
                    this.textBox4.Text = 全局变量.这是配置文档诶[4].Replace("终止=", "");
                    this.textBox7.Text = 全局变量.这是配置文档诶[5].Replace("末尾=", "");
                    this.textBox6.Text = 全局变量.这是配置文档诶[6].Replace("周期=", "");
                    if(全局变量.这是配置文档诶[7].Replace("上一次的进度=", "") != "")
                    {
                        this.textBox3.Text = 全局变量.这是配置文档诶[7].Replace("上一次的进度=", "");
                    }
                }
                catch(Exception)
                {
                    System.Media.SystemSounds.Beep.Play();
                    toolStripStatusLabel3.Text = "配置文档好像坏欸。";
                    textBox1.Text = "就绪。\r\n额，配置文档好像坏欸。";
                }
            }
        }

        //开始运行
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.toolStripStatusLabel4.Text) != 0 && this.toolStripStatusLabel2.Text != "成功 (200)")
            {
                //先暂停
                //this.timer1.Enabled = false;
                this.toolStripStatusLabel1.Text = "暂停";
                全局变量.网络循环操作的状态 = false;
                //timer1.Stop();
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
                        全局变量.网络循环操作的状态 = true;
                        //timer1.Start();
                        HTTPGET();
                        timer2.Start();
                        this.button3.Enabled = true;
                        this.button2.Enabled = false;
                    }
                    return;
                }
            }
            if(this.textBox5.Text == "")
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("数值部分不能为空诶。", "错误    (  OдO)!!!");
                return;
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
            //this.timer1.Interval = int.Parse(textBox6.Text);

            //截取字符
            try
            { 
            全局变量.位6 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(0, 1));
            全局变量.位5 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(1, 1));
            全局变量.位4 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(2, 1));
            全局变量.位3 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(3, 1));
            全局变量.位2 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(4, 1));
            全局变量.位1 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(5, 1));
            }
            catch (Exception)
            {
                System.Media.SystemSounds.Hand.Play();
                this.toolStripStatusLabel4.Text = "0";
                this.toolStripStatusLabel3.Text = "起始字段输入有误。";
                this.toolStripStatusLabel2.Text = "错误 (000)";
                this.toolStripStatusLabel1.Text = "已终止";
                this.textBox1.Text = "已终止\r\n起始字段输入有误吼，是六位数欸。\r\n又或者在里面输入了奇怪的东西?";
                return;
            }
            try 
            {
                全局变量.终位6 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(0, 1));
                全局变量.终位5 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(1, 1));
                全局变量.终位4 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(2, 1));
                全局变量.终位3 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(3, 1));
                全局变量.终位2 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(4, 1));
                全局变量.终位1 = 全局变量.递增列表.IndexOf(textBox4.Text.Substring(5, 1));
            }
            catch (Exception)
            {
                System.Media.SystemSounds.Hand.Play();
                this.toolStripStatusLabel4.Text = "0";
                this.toolStripStatusLabel3.Text = "终止字段输入有误。";
                this.toolStripStatusLabel2.Text = "错误 (000)";
                this.toolStripStatusLabel1.Text = "已终止";
                this.textBox1.Text = "已终止\r\n终止字段输入有误吼，是六位数欸。\r\n又或者在里面输入了奇怪的东西?";
                return;
            }


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

            //this.timer1.Enabled = true;
            this.timer2.Enabled = true;
            this.button3.Enabled = true;
            this.button2.Enabled = false;
            全局变量.网络循环操作的状态 = true;
            HTTPGET();
            //把焦点给button3
            SelectNextControl(ActiveControl, false, true, true, true);
        }

        

        //这个坑爹Timer我是不会再用了。
        //private void timer1_Tick(object sender, EventArgs e)
        //循环网络操作
        private async void HTTPGET()
        {
            //计数
            全局变量.计数 = 全局变量.计数 + 1;
            this.toolStripStatusLabel4.Text = 全局变量.计数.ToString();
            //int abc = 12;
            //this.toolStripStatusLabel4.Text = 全局变量.这是递增列表[abc].ToString();
            //this.toolStripStatusLabel3.Text = 全局变量.代码串;

            //打印到状态栏
            try
            { 
            this.toolStripStatusLabel3.Text = 全局变量.前缀 + 全局变量.数值 +"_"+ 全局变量.这是递增列表[全局变量.位6].ToString() + 全局变量.这是递增列表[全局变量.位5].ToString() + 全局变量.这是递增列表[全局变量.位4].ToString() + 全局变量.这是递增列表[全局变量.位3].ToString() + 全局变量.这是递增列表[全局变量.位2].ToString() + 全局变量.这是递增列表[全局变量.位1].ToString() + 全局变量.后缀;
            }
            catch (Exception)
            {
                System.Media.SystemSounds.Hand.Play();
                this.toolStripStatusLabel4.Text = "0";
                this.toolStripStatusLabel3.Text = "起始字段输入有误。";
                this.toolStripStatusLabel2.Text = "错误 (000)";
                this.toolStripStatusLabel1.Text = "已终止";
                this.textBox1.Text = "已终止\r\n起始字段输入有误吼，是六位数欸。\r\n又或者在里面输入了奇怪的东西?";
                return;
            }

            //网络爬取
            //timer1.Stop();
            var client = new HttpClient();
            //var 输出 = await client.GetAsync(全局变量.初始代码串);
            try //Try 是个好东西
            { 
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
                if (全局变量.网络循环操作的状态 != false)
                {
                    //timer1.Start();
                    if(textBox6.Text != "")
                    {
                    await Task.Delay(int.Parse(textBox6.Text));
                    }
                    /*
                    else if (int.Parse(textBox6.Text) >= 10000)
                    {
                        await Task.Delay(9999);
                    }
                    */
                    else
                    {
                        await Task.Delay(1);
                    }
                    HTTPGET();
                }
            }
            else
            {
                if (全局变量.网络循环操作的状态 != false)
                {
                    if (textBox6.Text != "")
                    {
                        await Task.Delay(int.Parse(textBox6.Text));
                    }
                    /* 
                    else if (int.Parse(textBox6.Text) >= 10000)
                    {
                        await Task.Delay(9999);
                    }
                    */
                    else
                    {
                        await Task.Delay(1);
                    }
                    this.toolStripStatusLabel2.Text = 全局变量.获取状态 +  " (" + ((int)输出.StatusCode).ToString() + ")";
                    string 报错时间 = DateTime.Now.ToString();
                    if (全局变量.错误弹窗的状态 == false)
                    {
                        全局变量.错误弹窗的状态 = true;
                        报错 报错 = new 报错();
                        报错.Show();
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, "错误，无法访问该网页: " + ((int)输出.StatusCode).ToString() );
                    }
                    else
                    {
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, "错误，无法访问该网页: " + ((int)输出.StatusCode).ToString() );
                    }
                    HTTPGET();
                    return;
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
            string 介素起始 = 全局变量.这是递增列表[全局变量.位6].ToString() + 全局变量.这是递增列表[全局变量.位5].ToString() + 全局变量.这是递增列表[全局变量.位4].ToString() + 全局变量.这是递增列表[全局变量.位3].ToString() + 全局变量.这是递增列表[全局变量.位2].ToString() + 全局变量.这是递增列表[全局变量.位1].ToString();
            string 介素终止  = 全局变量.这是递增列表[全局变量.终位6].ToString() + 全局变量.这是递增列表[全局变量.终位5].ToString() + 全局变量.这是递增列表[全局变量.终位4].ToString() + 全局变量.这是递增列表[全局变量.终位3].ToString() + 全局变量.这是递增列表[全局变量.终位2].ToString() + 全局变量.这是递增列表[全局变量.终位1].ToString();
            
            if (介素起始 != 介素终止)
            {
                全局变量.位1 = 全局变量.位1 + 1;
            }
            if (全局变量.位1 > 35)
            {
                全局变量.位1 = 0;
                if (介素起始 != 介素终止)
                {
                    全局变量.位2 = 全局变量.位2 + 1;
                }
            }
            if (全局变量.位2 > 35)
            {
                全局变量.位2 = 0;
                if (介素起始 != 介素终止)
                {
                    全局变量.位3 = 全局变量.位3 + 1;
                }
            }
            if (全局变量.位3 > 35)
            {
                全局变量.位3 = 0;
                if (介素起始 != 介素终止)
                {
                    全局变量.位4 = 全局变量.位4 + 1;
                }
            }
            if (全局变量.位4 > 35)
            {
                全局变量.位4 = 0;
                if (介素起始 != 介素终止)
                {
                    全局变量.位5 = 全局变量.位5 + 1;
                }
            }
            if (全局变量.位5 > 35)
            {
                全局变量.位5 = 0;
                if (介素起始 != 介素终止)
                {
                    全局变量.位6 = 全局变量.位6 + 1;
                }
            }
            if (全局变量.位6 > 35 ||  介素起始 == 介素终止)
            {
                //全局变量.位6 = 0;

                if(全局变量.网络循环操作的状态 != false)
                {
                    //this.timer1.Enabled = false;
                    全局变量.网络循环操作的状态 = false;
                    //timer1.Stop();
                    timer2.Stop();
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                    this.toolStripStatusLabel1.Text = "失败";
                    System.Media.SystemSounds.Hand.Play();
                    MessageBox.Show("我尽力了QwQ", "失败    ╥﹏╥...");
                }
            }
            
            //Try 报错哩就放在这儿弹个窗口
            }
            catch (Exception 错了)
            {
                //内存回收
                client.Dispose();
                client = null;
                GC.Collect();


                string[] 错了呀 = 错了.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string 报错时间 = DateTime.Now.ToString();

                if (全局变量.错误弹窗的状态 == false)
                {
                    全局变量.错误弹窗的状态 = true;
                    报错 报错 = new 报错();
                    报错.Show();

                    if (错了呀.Length > 0)
                    {
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, "异常错误");
                    }
                }
                else
                {
                    if (错了呀.Length > 0)
                    {
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_videoshotpvhdboss_get.报错.列表更新(Bili_videoshotpvhdboss_get.报错.让我看看.listView1, 报错时间, "异常错误");
                    }
                }

                //报错了就重试喽...
                if (全局变量.网络循环操作的状态 != false)
                {
                    if (textBox6.Text != "")
                    {
                        await Task.Delay(int.Parse(textBox6.Text));
                    }
                    /* 
                    else if (int.Parse(textBox6.Text) >= 10000)
                    {
                        await Task.Delay(9999);
                    }
                    */
                    else
                    {
                        await Task.Delay(1);
                    }
                    HTTPGET();
                    return;
                }
            }
        }

        //继续
        private void button2_Click(object sender, EventArgs e)
        {
            //this.timer1.Enabled = true;
            this.toolStripStatusLabel1.Text = "进行中...";
            全局变量.网络循环操作的状态 = true;
            //timer1.Start(); 
            HTTPGET();
            timer2.Start();
            this.button3.Enabled = true;
            this.button2.Enabled = false;
            //把焦点给button3，妈耶，怎么这么麻烦。。。
            SelectNextControl(ActiveControl, false, true, true, true);
        }
        //暂停
        private void button3_Click(object sender, EventArgs e)
        {
            //this.timer1.Enabled = false;
            this.toolStripStatusLabel1.Text = "暂停";
            全局变量.网络循环操作的状态 = false;
            //timer1.Stop();  
            timer2.Stop();
            this.button2.Enabled = true;
            this.button3.Enabled = false;
        }

        //写日志
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                string 旧文本;
                if (System.IO.File.Exists(@".\Logs\"+ 全局变量.当前时间和日期 +".txt") == true)
                {
                    旧文本 = System.IO.File.ReadAllText(@".\Logs\"+ 全局变量.当前时间和日期 +".txt");
                }
                else
                {
                    旧文本 = "";
                }
                string[] 新文本 = { (旧文本 + DateTime.Now.ToString() + "\t|\t" + toolStripStatusLabel4.Text + "\t|\t" + toolStripStatusLabel3.Text) };
                System.IO.File.WriteAllLines(@".\Logs\"+ 全局变量.当前时间和日期 +".txt", 新文本);
            }
        }

        //关于
        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bili_videoshotpvhdboss_get\nBy：yuhang0000\nBuild Time：" + 全局变量.Buildtime + "\n版本号：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() , "关于");
        }

        //复制状态栏链接
        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            if (this.toolStripStatusLabel3.Text != "https://bimp.hdslb.com/videoshotpvhdboss/...")
            { 
                Clipboard.SetDataObject(this.toolStripStatusLabel3.Text);
            }
        }

        //离开textBox6的焦点
        private void textBox6_Leave(object sender, EventArgs e)
        {
            if(this.textBox6.Text == "")
            {
                this.textBox6.Text = "1";
            }
        }

        //进行textBox6文本操作
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                SystemSounds.Beep.Play();
            }
        }
        //进行textBox6文本操作too
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
            else
            { 
                if(textBox6.Text.Length > 3 && e.KeyChar != (char)8)
                {
                    e.Handled = true;
                    SystemSounds.Beep.Play();
                    this.textBox6.Text = "9999";
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        //窗口调整大小自适应
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.textBox1.Width = this.Width - 507;
            this.textBox1.Height = this.Height - 86;
            this.label6.Top = this.Height - 101;
        }

        //程序关闭后要干嘛勒?当然是要保存配置文档啦!
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string 上一次的进度在哪;
            try
            {
                上一次的进度在哪 = 全局变量.这是递增列表[全局变量.位6].ToString() + 全局变量.这是递增列表[全局变量.位5].ToString() + 全局变量.这是递增列表[全局变量.位4].ToString() + 全局变量.这是递增列表[全局变量.位3].ToString() + 全局变量.这是递增列表[全局变量.位2].ToString() + "0";
                int 位6 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(0, 1));
                int 位5 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(1, 1));
                int 位4 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(2, 1));
                int 位3 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(3, 1));
                int 位2 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(4, 1));
                int 位1 = 全局变量.递增列表.IndexOf(textBox3.Text.Substring(5, 1));
                if (位1 > 全局变量.位1 && 位2 > 全局变量.位2 && 位3 > 全局变量.位3 && 位4 > 全局变量.位4 && 位5 > 全局变量.位5 && 位6 > 全局变量.位6)
                {
                    上一次的进度在哪 = "";
                }
            }
            catch (Exception) 
            {
                上一次的进度在哪 = "";
            }
            string[] 这是配置文档诶 = {"版本=" + 版本, "编辑=" + textBox2.Text, "数值=" + textBox5.Text, "起始=" + textBox3.Text, "终止=" + textBox4.Text, "末尾=" + textBox7.Text, "周期=" + textBox6.Text, "上一次的进度=" + 上一次的进度在哪, "", "[By:yuhang0000]" };
            System.IO.File.WriteAllLines(@".\Setting.ini", 这是配置文档诶);
        }
    }
}
