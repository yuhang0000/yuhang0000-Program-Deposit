using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Bili_favorites_list
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static class 全局变量
        {
            public static String 编辑;
            public static String 起始;
            public static String 终止;
            public static String 后缀;
            public static String 链接;
            public static String 内容 = "";
            public static long ML;
            public static bool 错误弹窗的状态;
            public static int 错误计数;
            public static bool 运行状态 = false;
            public static String 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            public static DateTime 编译时间 = System.IO.File.GetLastWriteTime(typeof(全局变量).Assembly.Location);
            public static String[] 配置文档;
        }

        //开始
        private void button1_Click(object sender, EventArgs e)
        {
            全局变量.编辑 = this.textBox2.Text;
            全局变量.起始 = this.textBox3.Text;
            全局变量.终止 = this.textBox4.Text;
            try
            {
                if (long.Parse(this.textBox3.Text) > long.Parse(this.textBox4.Text))
                {
                    全局变量.起始 = this.textBox4.Text;
                    全局变量.终止 = this.textBox3.Text;
                    this.textBox4.Text = 全局变量.终止;
                    this.textBox3.Text = 全局变量.起始;
                }
            }
            catch
            {
                System.Media.SystemSounds.Hand.Play();
                this.textBox1.Text = "输入数据有误诶。";
                return;
            }
            全局变量.后缀 = this.textBox5.Text;
            全局变量.ML = long.Parse(this.textBox3.Text);
            全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
            全局变量.链接 = 全局变量.编辑 + 全局变量.起始 + 全局变量.后缀;
            this.toolStripStatusLabel1.Text = "进行中";
            全局变量.运行状态 = true;
            this.button1.Enabled = false;
            this.button2.Enabled = true;
            //焦点转至 "暂停"
            SelectNextControl(ActiveControl, false, true, true, true);
            HTTPGET();
        }

        private void 写进Output(String ML, String UID, String name, String title, String intro, String ctime)
        {
            全局变量.内容 = 全局变量.内容 + "\r\n#  ML" + ML + "\t" + UID + "\t"  + name + "\t"  + title + "\t"  + 
                intro + "\t"  + ctime;
        }
        
        private String 时间戳(int time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var output = startTime.AddSeconds(time);
            return output.ToString();
        }

        private async void HTTPGET()
        {
            int ppp = 100000;
            if(全局变量.运行状态 == false)
            {
                return;
            }
            Console.WriteLine("开始运行: " + 全局变量.链接);
            var httpclient = new HttpClient();
            this.toolStripStatusLabel6.Text = 全局变量.链接;
            try
            {
                HttpResponseMessage httpbody = await httpclient.GetAsync(全局变量.链接);
                var CODE = httpbody.StatusCode;
                String 获取状态 = "等待";
                if (CODE.ToString() == "OK")
                {
                    获取状态 = "成功";
                }
                this.toolStripStatusLabel2.Text = 获取状态 + " (" + ((int)CODE).ToString() + ")";
                String 输出 = await httpbody.Content.ReadAsStringAsync();
                this.textBox1.Text = 输出;

                JObject jo = JObject.Parse(输出);
                //Console.WriteLine("jo: " + jo);
                var null1 = 输出.IndexOf("\"data\":null");
                var null2 = 输出.IndexOf("\"info\":null,");
                var null3 = 输出.IndexOf("\"code\":-400");
                if (null3 >= 0)
                {
                    //内存回收
                    httpclient.Dispose();
                    httpclient = null;
                    GC.Collect();
                    Console.WriteLine("错误");
                    HTTPGET();
                    return;
                }
                if (null1 >= 0 || null2 >= 0)
                {
                    Console.WriteLine("错误");
                    this.toolStripStatusLabel3.Text = "UID0";
                    this.toolStripStatusLabel4.Text = "ML0";
                    this.toolStripStatusLabel5.Text = "默认收藏夹"; 
                    写进Output(全局变量.ML.ToString(), "无", "无", "无", "", "-");
                    Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1,全局变量.ML.ToString(),
                        "无", "无", "无", "", "-");

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
                    全局变量.ML++;
                    if (全局变量.ML - long.Parse(全局变量.起始) >= ppp)
                    {
                        this.textBox3.Text = 全局变量.ML.ToString();
                        输出内容(true,true);
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                    }
                    else if (全局变量.ML >= long.Parse(全局变量.终止))
                    {
                        this.textBox3.Text = 全局变量.ML.ToString();
                        输出内容(true);
                        this.button1.Enabled = true;
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.终止 = (long.Parse(this.textBox3.Text) + ppp).ToString();
                        全局变量.内容 = "";
                        this.textBox4.Text = 全局变量.终止;
                    }
                    全局变量.链接 = 全局变量.编辑 + 全局变量.ML + 全局变量.后缀;

                    //内存回收
                    httpclient.Dispose();
                    httpclient = null;
                    GC.Collect();
                    HTTPGET();
                    return;
                }
                JObject info = (JObject)jo["data"]["info"];
                string title = (string)info["title"];
                string intro = (string)info["intro"];
                int ctime = (int)info["ctime"];
                JObject upper = (JObject)info["upper"];
                string uid = (string)info["mid"];
                string name = (string)upper["name"];
                this.toolStripStatusLabel3.Text = "UID" + uid;
                this.toolStripStatusLabel4.Text = "ML" + 全局变量.ML;
                this.toolStripStatusLabel5.Text = title;
                写进Output(全局变量.ML.ToString(),
                        "UID" + uid, name, title, intro, 时间戳(ctime));
                Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1, 全局变量.ML.ToString(),
                        "UID" + uid, name, title, intro, 时间戳(ctime));
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
                全局变量.ML++;
                if (全局变量.ML - long.Parse(全局变量.起始) >= ppp)
                {
                    this.textBox3.Text = 全局变量.ML.ToString();
                    输出内容(true, true);
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                }
                else if (全局变量.ML >= long.Parse(全局变量.终止))
                {
                    this.textBox3.Text = 全局变量.ML.ToString();
                    输出内容(true);
                    this.button1.Enabled = true;
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.终止 = (long.Parse(this.textBox3.Text) + ppp).ToString();
                    全局变量.内容 = "";
                    this.textBox4.Text = 全局变量.终止;
                }
                全局变量.链接 = 全局变量.编辑 + 全局变量.ML + 全局变量.后缀;
                HTTPGET();
            }
            catch (Exception 错了)
            {
                //内存回收
                httpclient.Dispose();
                httpclient = null;
                GC.Collect();


                string[] 错了呀 = 错了.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string 报错时间 = DateTime.Now.ToString();
                this.toolStripStatusLabel2.Text = "连接失败";

                if (全局变量.错误弹窗的状态 == false)
                {
                    全局变量.错误弹窗的状态 = true;
                    报错 报错 = new 报错();
                    报错.Show();

                    if (错了呀.Length > 0)
                    {
                        Bili_favorites_list.报错.列表更新(Bili_favorites_list.报错.让我看看.listView1, 报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_favorites_list.报错.列表更新(Bili_favorites_list.报错.让我看看.listView1, 报错时间, "异常错误");
                    }
                }
                else
                {
                    if (错了呀.Length > 0)
                    {
                        Bili_favorites_list.报错.列表更新(Bili_favorites_list.报错.让我看看.listView1, 报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_favorites_list.报错.列表更新(Bili_favorites_list.报错.让我看看.listView1, 报错时间, "异常错误");
                    }
                }

                //报错了就重试喽...
                if (全局变量.运行状态 != false)
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

        //调整窗口大小
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.label6.Top = this.Height - 495 + 397;
            this.groupBox1.Width = this.Width - 1130 + 816;
            this.groupBox1.Height = this.Height - 495 + 423;
        }

        //暂停
        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = false;
            this.button3.Enabled = true;
            全局变量.运行状态 = false;
            SelectNextControl(ActiveControl, false, true, true, true);
            toolStripStatusLabel1.Text = "暂停";
        }

        //继续
        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
            this.button2.Enabled = true;
            全局变量.运行状态 = true;
            SelectNextControl(ActiveControl, false, true, true, true);
            toolStripStatusLabel1.Text = "进行中";
            HTTPGET();
        }

        //打开就运行
        private void Form1_Load(object sender, EventArgs e)
        {
            this.label6.Text = "v" + 全局变量.版本;
            Output output = new Output();
            output.Show();
            //检查配置文件在不在喽
            if (System.IO.File.Exists(@".\Setting.ini") == false)
            {
                String[] 配置文档 = { "版本=" + 全局变量.版本, "编辑=" + textBox2.Text, "起始=" + textBox3.Text,
                    "终止=" + textBox4.Text, "后缀=" + textBox5.Text, "延时=" + textBox6.Text, "", "[By:yuhang0000]" };
                System.IO.File.WriteAllLines(@".\Setting.ini", 配置文档);
                全局变量.配置文档 = 配置文档;
                this.textBox1.Text = "就绪。";
            }
            else
            {
                try //我爱死 try 和 catch 这两个方法啦，爱用🤍
                {
                    全局变量.配置文档 = System.IO.File.ReadAllLines(@".\Setting.ini");
                    this.textBox2.Text = 全局变量.配置文档[1].Replace("编辑=", "");
                    this.textBox3.Text = 全局变量.配置文档[2].Replace("起始=", "");
                    this.textBox4.Text = 全局变量.配置文档[3].Replace("终止=", "");
                    this.textBox5.Text = 全局变量.配置文档[4].Replace("后缀=", "");
                    this.textBox6.Text = 全局变量.配置文档[5].Replace("延时=", "");
                    this.textBox1.Text = "就绪。";
                }
                catch (Exception)
                {
                    System.Media.SystemSounds.Beep.Play();
                    toolStripStatusLabel3.Text = "配置文档好像坏欸。";
                    textBox1.Text = "就绪。\r\n额，配置文档好像坏欸。";
                }
            }
            全局变量.ML = long.Parse(this.textBox3.Text);
        }

        //复制状态栏链接
        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
        {
            if (this.toolStripStatusLabel6.Text != "https://api.bilibili.com/x/v3/fav/resource/list?media_id=...")
            {
                Clipboard.SetDataObject(this.toolStripStatusLabel6.Text);
            }
        }

        //导出
        private void button4_Click(object sender, EventArgs e)
        {
            输出内容();
        }

        private void 保存配置文档()
        {
            String[] 配置文档 = { "版本=" + 全局变量.版本, "编辑=" + textBox2.Text, "起始=" + 全局变量.ML,
                    "终止=" + textBox4.Text, "后缀=" + textBox5.Text, "延时=" + textBox6.Text, "", "[By:yuhang0000]" };
            System.IO.File.WriteAllLines(@".\Setting.ini", 配置文档);
        }

        private void 输出内容(bool close = false, bool bizui = false)
        {
            Console.WriteLine(全局变量.内容);
            if (全局变量.内容 == "" && close == false)
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("保存失败，内容是空滴。", "失败哩  ╥﹏╥...");
                return;
            }
            else if (全局变量.内容 == "" && close == true)
            {
                保存配置文档();
                return;
            }
            else if (close == true && bizui == false)
            {
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                全局变量.运行状态 = false;
                toolStripStatusLabel1.Text = "暂停"; 保存配置文档();
            }
            else if (close == true && bizui == true)
            {
                保存配置文档();
            }
            System.IO.Directory.CreateDirectory(Application.StartupPath + @"\Output\");
            //String date = DateTime.Now.ToString("yyyy-MM-dd") +"   "+ DateTime.Now.Hour.ToString() +"-"+
            //DateTime.Now.Minute.ToString() +"-"+ DateTime.Now.Second.ToString();
            String num;
            if (全局变量.起始 == (全局变量.ML - 1).ToString())
            {
                num = (全局变量.ML - 1).ToString();
            }
            else
            {
                num = 全局变量.起始 + " - " +(全局变量.ML - 1).ToString();
            }
            String path = Application.StartupPath + @"\Output\" + num + ".txt";
            try
            {
                System.IO.File.WriteAllText(path, 全局变量.内容);
            }
            catch
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("保存失败，无法写入文件，在:\r\n" + path, "失败哩  ╥﹏╥...");
                return;
            }
            if (bizui == false)
            { 
                System.Media.SystemSounds.Beep.Play();
                if (MessageBox.Show("数据保存在: \r\n" + path, "成功!!!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        Process.Start("notepad", path);
                    }
                    catch (Exception ex)
                    {
                        System.Media.SystemSounds.Hand.Play();
                        MessageBox.Show(ex.Message.ToString() + "\r\n" + "notepad \"" +
                        path + "\"", "错误    Σ(っ °Д °;)っ");
                    }
                }
            }
        }

        //关闭前先保存
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            输出内容(true);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bili favorites list\nBy：yuhang0000\nBuild Time：" + 全局变量.编译时间 + "\n版本：" + 
                全局变量.版本, "关于");
        }
    }
}
