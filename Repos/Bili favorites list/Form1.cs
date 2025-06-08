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
using System.IO;
using System.IO.Compression;


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
            public static int 队列 = 1;
            public static long ML;
            public static bool 错误弹窗的状态;
            public static int 错误计数;
            public static int 步幅;
            public static bool 运行状态 = false;
            public static String 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            public static DateTime 编译时间 = System.IO.File.GetLastWriteTime(typeof(全局变量).Assembly.Location);
            public static String[] 配置文档;
            public static String 标题栏高度;
        }

        //开始
        private void button1_Click(object sender, EventArgs e)
        {
            全局变量.编辑 = this.textBox2.Text;
            全局变量.起始 = this.textBox3.Text;
            全局变量.终止 = this.textBox4.Text;
            try
            {
                全局变量.步幅 = int.Parse(this.textBox7.Text);
                if (long.Parse(全局变量.起始) < 0 || 全局变量.起始 == "")
                {
                    全局变量.起始 = "0";
                    this.textBox3.Text = "0";
                }
                if (全局变量.步幅 < 10 || 全局变量.步幅.ToString() == "")
                {
                    全局变量.步幅 = 10;
                    this.textBox7.Text = "10";
                }
                if (全局变量.终止 == "" || long.Parse(全局变量.终止) < 0)
                {
                    全局变量.终止 = "92233720368547758070";
                    this.textBox4.Text = "9223372036854775807";
                }
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

        /// <summary>
        /// 顾名思义, 写进输出稳定里
        /// </summary>
        /// <param name="ML">收藏夹ID</param>
        /// <param name="UID">用户DI</param>
        /// <param name="name">用户昵称</param>
        /// <param name="title">收藏夹标题</param>
        /// <param name="num">收藏夹数量</param>
        /// <param name="intro">收藏夹简介</param>
        /// <param name="ctime">创建时间</param>
        public void 写进Output(String ML, String UID, String name, String title, String num, String intro, String ctime)
        {
            if (全局变量.运行状态 == true)
            {
                全局变量.内容 = 全局变量.内容 + "\r\n#\tML" + ML + "\t" + UID + "\t"  + name + "\t"  + title + "\t"  +
                    num + "\t" + intro + "\t"  + ctime;
            }
        }
        
        private String 时间戳(int time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var output = startTime.AddSeconds(time);
            return output.ToString();
        }

        private async void HTTPGET()
        {
            if(全局变量.运行状态 == false)
            {
                return;
            }
            Console.WriteLine("开始运行: " + 全局变量.链接);
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            this.toolStripStatusLabel6.Text = 全局变量.链接;
            try
            {
                HttpResponseMessage httpbody = await httpclient.GetAsync(全局变量.链接);
                if (全局变量.运行状态 != true)
                {
                    Console.WriteLine("检测到已暂停了诶");
                    return;
                }
                var CODE = httpbody.StatusCode;
                String 获取状态 = "等待";
                if (CODE.ToString() == "OK")
                {
                    获取状态 = "成功";
                }
                this.toolStripStatusLabel2.Text = 获取状态 + " (" + ((int)CODE).ToString() + ")";
                //String 输出 = await httpbody.Content.ReadAsStringAsync();
                String 输出;

                //解压缩
                if (httpbody.Content.Headers.ContentEncoding.FirstOrDefault() == "gzip") {
                    Stream ms = await httpbody.Content.ReadAsStreamAsync();
                    GZipStream gzips = new GZipStream(ms, CompressionMode.Decompress);
                    StreamReader sr = new StreamReader(gzips);
                    输出 = await sr.ReadToEndAsync();
                    //ms.Dispose();
                    gzips.Dispose();
                    sr.Dispose();
                }
                else
                {
                    输出 = await httpbody.Content.ReadAsStringAsync();
                }

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
                    this.toolStripStatusLabel3.Text = "ML" + 全局变量.ML;
                    this.toolStripStatusLabel4.Text = "UID0";
                    this.toolStripStatusLabel5.Text = "默认收藏夹"; 
                    写进Output(全局变量.ML.ToString(), "无", "无", "无", "0", "", "-");
                    Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1,全局变量.ML.ToString(),
                        "无", "无", "无", "0", "", "-");

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
                    if (全局变量.ML - long.Parse(全局变量.起始) >= 全局变量.步幅)
                    {
                        this.textBox3.Text = 全局变量.ML.ToString();
                        输出内容(true,true);
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                    }
                    else if (全局变量.ML < 0 || 全局变量.ML == long.MaxValue)
                    {
                        this.textBox3.Text = "0";
                        输出内容(true);
                        this.button1.Enabled = true;
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.终止 = "9223372036854775807";
                        全局变量.内容 = "";
                        this.textBox4.Text = 全局变量.终止;
                        return;
                    }
                    else if (全局变量.ML >= long.Parse(全局变量.终止))
                    {
                        this.textBox3.Text = 全局变量.ML.ToString();
                        输出内容(true);
                        this.button1.Enabled = true;
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.终止 = (long.Parse(this.textBox3.Text) + 全局变量.步幅).ToString();
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
                string num = (string)info["media_count"];
                string name = (string)upper["name"];
                this.toolStripStatusLabel3.Text = "ML" + 全局变量.ML;
                this.toolStripStatusLabel4.Text = "UID" + uid;
                this.toolStripStatusLabel5.Text = title;
                写进Output(全局变量.ML.ToString(),
                        "UID" + uid, name, title, num, intro, 时间戳(ctime));
                Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1, 全局变量.ML.ToString(),
                        "UID" + uid, name, title, num, intro, 时间戳(ctime));
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
                if (全局变量.ML - long.Parse(全局变量.起始) >= 全局变量.步幅)
                {
                    this.textBox3.Text = 全局变量.ML.ToString();
                    输出内容(true, true);
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                }
                else if (全局变量.ML < 0 || 全局变量.ML == long.MaxValue)
                {
                    this.textBox3.Text = "0";
                    输出内容(true);
                    this.button1.Enabled = true;
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.终止 = "9223372036854775807";
                    全局变量.内容 = "";
                    this.textBox4.Text = 全局变量.终止;
                    return;
                }
                else if (全局变量.ML >= long.Parse(全局变量.终止))
                {
                    this.textBox3.Text = 全局变量.ML.ToString();
                    输出内容(true);
                    this.button1.Enabled = true;
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.终止 = (long.Parse(this.textBox3.Text) + 全局变量.步幅).ToString();
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


        

        async public Task http(string url,string ml)
        {
            HttpClient client = new HttpClient();
            //我觉得压缩比较好
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            var res = await client.GetAsync(url);
            if (全局变量.运行状态 != true)
            {
                Console.WriteLine("检测到已暂停了诶");
                return;
            }
            var CODE = res.StatusCode;
            String 获取状态 = "等待";
            if (CODE.ToString() == "OK")
            {
                获取状态 = "成功";
            }
            UIupdate(this.toolStripStatusLabel2, 获取状态 + " (" + ((int)CODE).ToString() + ")");
            //String 输出 = await res.Content.ReadAsStringAsync();

            //解压缩
            Stream ms = await res.Content.ReadAsStreamAsync();
            GZipStream gzips = new GZipStream(ms, CompressionMode.Decompress);
            StreamReader sr = new StreamReader(gzips);
            string 输出 = sr.ReadToEnd();
            //ms.Dispose();
            gzips.Dispose();
            sr.Dispose();

            UIupdate(this.textBox1.Text,输出);
            JObject jo = JObject.Parse(输出);
            //Console.WriteLine("jo: " + jo);
            var null1 = 输出.IndexOf("\"data\":null");
            var null2 = 输出.IndexOf("\"info\":null,");
            var null3 = 输出.IndexOf("\"code\":-400");
            if (null3 != -1)
            {
                //内存回收
                client.Dispose();
                res.Dispose();
                GC.Collect();
                Console.WriteLine("错误");
                HTTPGET();
                return;
            } //请求错误
            if (null1 != -1 || null2 != -1) //权限不够
            {
                Console.WriteLine("错误");
                UIupdate(this.toolStripStatusLabel3.Text,"ML" + ml);
                UIupdate(this.toolStripStatusLabel4.Text,"UID0");
                UIupdate(this.toolStripStatusLabel5.Text,"默认收藏夹");
                写进Output(ml, "无", "无", "无", "0", "", "-");
                Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1, 全局变量.ML.ToString(),
                    "无", "无", "无", "0", "", "-");

                if (UIget(textBox6) != "")
                {
                    await Task.Delay(int.Parse( UIget(textBox6) ));
                }
                else
                {
                    await Task.Delay(1);
                }
                //内存回收
                client.Dispose();
                res.Dispose();
                GC.Collect();
                HTTPGET();
                return;
            }

            //能运行在这里说明正常
            JObject info = (JObject)jo["data"]["info"];
            string title = (string)info["title"];
            string intro = (string)info["intro"];
            int ctime = (int)info["ctime"];
            JObject upper = (JObject)info["upper"];
            string uid = (string)info["mid"];
            string num = (string)info["media_count"];
            string name = (string)upper["name"];
            UIupdate(this.toolStripStatusLabel3.Text, "ML" + 全局变量.ML);
            UIupdate(this.toolStripStatusLabel4.Text, "UID" + uid);
            UIupdate(this.toolStripStatusLabel5.Text, title);
            写进Output(ml,
                    "UID" + uid, name, title, num, intro, 时间戳(ctime));
            Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1, 全局变量.ML.ToString(),
                    "UID" + uid, name, title, num, intro, 时间戳(ctime));
            if (UIget(textBox6) != "")
            {
                await Task.Delay(int.Parse( UIget(textBox6 ) ));
            }
            else
            {
                await Task.Delay(1);
            }
        }





        //修改UI文本
        public void UIupdate(object obj,string text)
        {
            Control control = null;
            if (obj is ToolStripStatusLabel)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    ToolStripStatusLabel status = obj as ToolStripStatusLabel;
                    status.Text = text;
                }));
                return;
            }
            else
            {
                control = obj as Control;
            }

            if(this.IsHandleCreated == false)
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
        //获取UI文本
        public string UIget(Control control)
        {
            if(this.IsHandleCreated == false)
            {
                return control.Text;
            }
            else
            {
                string t = "";
                this.Invoke(new MethodInvoker(() =>
                {
                    t = control.Text;
                }));
                return t;
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
            全局变量.ML = long.Parse(Bili_favorites_list.Output.让我看看.listView1.Items[Bili_favorites_list.Output.让我看看.listView1.Items.Count - 1].SubItems[0].Text) + 1;
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
            if (System.IO.File.Exists(@"./Setting.ini") == false)
            {
                String[] 配置文档 = { "版本=" + 全局变量.版本, 
                    "编辑=" + textBox2.Text, 
                    "起始=" + textBox3.Text,
                    "终止=" + textBox4.Text, 
                    "后缀=" + textBox5.Text, 
                    "延时=" + textBox6.Text,
                    "步幅=" + textBox7.Text, 
                    "步幅=" + textBox7.Text, 
                    "队列=" + this.numericUpDown1.Value.ToString(), 
                    "[By:yuhang0000]" };
                System.IO.File.WriteAllLines(@"./Setting.ini", 配置文档);
                全局变量.配置文档 = 配置文档;
                this.textBox1.Text = "就绪。";
            }
            else
            {
                try //我爱死 try 和 catch 这两个方法啦，爱用🤍
                {
                    全局变量.配置文档 = System.IO.File.ReadAllLines(@"./Setting.ini");
                    this.textBox2.Text = 全局变量.配置文档[1].Replace("编辑=", "");
                    this.textBox3.Text = 全局变量.配置文档[2].Replace("起始=", "");
                    this.textBox4.Text = 全局变量.配置文档[3].Replace("终止=", "");
                    this.textBox5.Text = 全局变量.配置文档[4].Replace("后缀=", "");
                    this.textBox6.Text = 全局变量.配置文档[5].Replace("延时=", "");
                    this.textBox7.Text = 全局变量.配置文档[6].Replace("步幅=", "");
                    this.numericUpDown1.Value = decimal.Parse(全局变量.配置文档[7].Replace("队列=", ""));
                    this.textBox1.Text = "就绪。";
                }
                catch (Exception)
                {
                    System.Media.SystemSounds.Beep.Play();
                    toolStripStatusLabel6.Text = "配置文档好像坏欸。";
                    textBox1.Text = "就绪。\r\n额，配置文档好像坏欸。";
                }
            }
            全局变量.ML = long.Parse(this.textBox3.Text);
            全局变量.标题栏高度 = (this.Height - this.ClientSize.Height).ToString();
            //Console.WriteLine("当前标题栏高度: " + 全局变量.标题栏高度);
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
            Console.WriteLine("保存配置文档");
            String[] 配置文档 = { 
                "版本=" + 全局变量.版本, 
                "编辑=" + textBox2.Text, 
                "起始=" + 全局变量.ML,
                "终止=" + textBox4.Text, 
                "后缀=" + textBox5.Text, 
                "延时=" + textBox6.Text,
                "步幅=" + textBox7.Text, 
                "队列=" + this.numericUpDown1.Value.ToString(), 
                "", 
                "[By:yuhang0000]" };
            System.IO.File.WriteAllLines(@"./Setting.ini", 配置文档);
        }

        private void 输出内容(bool close = false, bool bizui = false)
        {
            //Console.WriteLine(全局变量.内容);
            if (全局变量.内容 == "" && close == false)
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("保存失败，内容是空滴。", "失败哩  ╥﹏╥...");
                return;
            }
            else if (全局变量.内容.Contains("\r\n\r\n") == false && close == true)
            {
                保存配置文档();
                return;
            }
            else if (close == true && bizui == false)
            {
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                全局变量.运行状态 = false;
                toolStripStatusLabel1.Text = "暂停"; 
                保存配置文档();
            }
            else if (close == true && bizui == true)
            {
                保存配置文档();
            }
            System.IO.Directory.CreateDirectory(Application.StartupPath + @"\Output\");
            //String date = DateTime.Now.ToString("yyyy-MM-dd") +"   "+ DateTime.Now.Hour.ToString() +"-"+
            //DateTime.Now.Minute.ToString() +"-"+ DateTime.Now.Second.ToString();
            String num;
            if (全局变量.起始 == (全局变量.ML - 1).ToString() )
            {
                num = (全局变量.ML - 1).ToString();
            }
            else
            {
                if (long.Parse(全局变量.起始) > (全局变量.ML - 1) )
                {
                    num = 全局变量.ML.ToString();
                }
                else
                {
                    num = 全局变量.起始 + " - " + (全局变量.ML - 1).ToString();
                }
                
            }
            String path = Application.StartupPath + @"/Output/" + num + ".txt";
            Console.WriteLine("尝试导出文件: " + path);
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

        //关鱼
        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bili favorites list\nBy：yuhang0000\nBuild Time：" + 全局变量.编译时间 + "\n版本：" + 
                全局变量.版本 + "\n\n引用的第三方库: \n" + "# Newtonsoft.Json v13.0.3\n" + 
                "   ( https://www.newtonsoft.com/json )", "关于");
        }

        //谁会在 "延时" 里写 "-1" 呀
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBox6.Text) < 1 || textBox6.Text == "")
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox6.Text = "1";
                }
                else if(int.Parse(textBox6.Text) > 9999)
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox6.Text = "9999";
                }
                else if (textBox6.Text[0].ToString() == "0" && textBox6.Text != "0")
                {
                    textBox6.Text = textBox6.Text.Substring(1);
                }
            }
            catch
            {
                System.Media.SystemSounds.Beep.Play();
                textBox6.Text = "1";
            }
        }

        //9223372036854775807 + 1 = -9223372036854775807
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (long.Parse(textBox3.Text) > long.MaxValue && textBox3.Text != "9223372036854775807")
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox3.Text = "9223372036854775807";
                }
                if (long.Parse(textBox3.Text) < 0 || textBox3.Text == "")
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox3.Text = "0";
                }
                else if (textBox3.Text[0].ToString() == "0" && textBox3.Text != "0")
                {
                    textBox3.Text = textBox3.Text.Substring(1);
                }
            }
            catch
            {
                System.Media.SystemSounds.Beep.Play();
                textBox3.Text = "0";
            }
        }

        //long.MaxValue = 9223372036854775807
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /*
                if (textBox4.Text[0].ToString() == "-")
                {
                    textBox4.Text = textBox4.Text.Substring(1);
                }   */
                if (long.Parse(textBox4.Text) < 0 || textBox4.Text == "")
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox4.Text = "9223372036854775807";
                }
                else if (textBox4.Text.Length > 19)
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox4.Text = "9223372036854775807";
                }
                else if (textBox4.Text[0].ToString() == "0" && textBox4.Text != "0")
                {
                    textBox4.Text = textBox4.Text.Substring(1);
                }
            }
            catch
            {
                System.Media.SystemSounds.Beep.Play();
                textBox4.Text = "9223372036854775807";
            }
        }

        //16位数，这已经够大了
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (long.Parse(textBox7.Text) < 10 || textBox7.Text == "")
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox7.Text = "10";
                }
                else if (textBox7.Text.Length > 16)
                {
                    System.Media.SystemSounds.Beep.Play();
                    textBox7.Text = "9999999999999999";
                }
                else if (textBox7.Text[0].ToString() == "0" && textBox7.Text != "0")
                {
                    textBox7.Text = textBox7.Text.Substring(1);
                }
            }
            catch
            {
                System.Media.SystemSounds.Beep.Play();
                textBox7.Text = "10";
            }
        }

        //合并文件
        private void button5_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + @"\Output\";
            if (Directory.Exists(path) == false)
            {
                path = Application.StartupPath;
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = path;
            dialog.Description = "选择文件夹所在位置: ";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.SelectedPath;
                Console.WriteLine(path);
                DirectoryInfo dir = new DirectoryInfo(path);
                if (Directory.Exists(path) == false)
                {
                    MessageBox.Show("找不到该路径:\r\n " + path, "错误    Σ(っ °Д °;)っ");
                    return;
                }
                FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    Console.WriteLine(fsinfo.FullName);
                }
            }
        }
    }
}
