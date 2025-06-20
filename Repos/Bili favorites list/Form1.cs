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
using System.Security.Cryptography;
using System.Xml.Linq;
using static Bili_favorites_list.Output;
using static Bili_favorites_list.报错;
using System.Text.RegularExpressions;
using System.Net;


namespace Bili_favorites_list
{
    public partial class Form1 : Form
    {
        public static Form1 form1 { get; private set; }

        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

        public static class 全局变量
        {
            public static String 编辑;
            public static String 起始;
            public static String 终止;
            public static String 后缀;
            public static String 链接;
            public static StringBuilder 内容 = new StringBuilder();
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
            public static HttpClient client;
        }

        //开始
        private void button1_Click(object sender, EventArgs e)
        {
            GC.Collect();
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
                //this.textBox1.Text = "输入数据有误诶。";
                UIprint("输入数据有误诶。");
                return;
            }
            全局变量.后缀 = this.textBox5.Text;
            全局变量.队列 = (int)this.numericUpDown1.Value;
            全局变量.ML = long.Parse(this.textBox3.Text);
            重置Output();
            全局变量.链接 = 全局变量.编辑 + 全局变量.起始 + 全局变量.后缀;
            this.toolStripStatusLabel1.Text = "进行中";
            全局变量.运行状态 = true;
            this.button1.Enabled = false;
            this.button2.Enabled = true;
            //焦点转至 "暂停"
            SelectNextControl(ActiveControl, false, true, true, true);
            //HTTPGET();
            //Output.让我看看.listView1.Items.Clear();
            Output.让我看看.listView1.VirtualListSize = 0;
            Output.lists.Clear();

            //尝试共用 HttpClient
            全局变量.client = new HttpClient();
            //我觉得压缩比较好 //定义爆头
            全局变量.client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            全局变量.client.DefaultRequestHeaders.Add("Accept", "*/*");
            全局变量.client.DefaultRequestHeaders.Add("Accept-Language", "zh,en-US;q=0.9,en;q=0.8,en-GB;q=0.7");
            全局变量.client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            全局变量.client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            //全局变量.client.DefaultRequestHeaders.Add("User-Agent", "");
            全局变量.client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
            //全局变量.client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "Windows");
            全局变量.client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            全局变量.client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            全局变量.client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");

            run_loop();
        }

        public static class httpheader
        {
            public static int t = 0;
            public static string[] uas = {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36 Edg/123.0.0.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/536.31 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/536.31",
            "Mozilla/5.0 (X11; Linux aarch64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.4928.34 OPR/103.0.4928.34",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.117",
            "Mozilla/5.0 (Windows NT 11.0) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/118.0.2",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.5993.117",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/121.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.4944.36 OPR/104.0.4944.36",
            "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Version/17.1 Safari/17.1",
            "Mozilla/5.0 (X11; Linux armv8l) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.4944.36 OPR/104.0.4944.36",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/119.0",
            "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.2151.97 Edg/119.0.2151.97",
            "Mozilla/5.0 (Linux; Android 10.0; Pixel 8) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.199 Mobile Safari/537.36",
            "Mozilla/5.0 (iPad; CPU OS 17_0_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.7 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 17_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.7 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Linux; Android 12.0; Samsung Galaxy S23) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.199 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 12.0; Pixel 7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.149 Mobile Safari/537.36",
            "Mozilla/5.0 (iPad; CPU OS 17_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.7 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 17_1_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.6 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 17_1_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.8 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Linux; Android 12.0; OPPO Find X6 Pro) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.199 Mobile Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 17_1_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.1 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/117.0",
            "Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Version/17.2 Safari/17.2",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 14_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.4970.60 OPR/105.0.4970.60",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.4998.70 OPR/106.0.4998.70",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_5) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/121.0",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Version/17.1 Safari/17.1",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/119.0",
            "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5845.187",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.5938.149",
            };
            public static string[] Platform = {
                "Macintosh",
                "Windows",
                "Android",
                "Linux",
            };
            public static string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0";
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
                //全局变量.内容 = 全局变量.内容 + "\r\n#\tML" + ML + "\t" + UID + "\t"  + name + "\t"  + title + "\t"  +
                //    num + "\t" + intro + "\t"  + ctime;
                全局变量.内容.Append("\r\n#\tML" + ML + "\t" + UID + "\t"  + name + "\t"  + title + "\t"  +
                    num + "\t" + intro + "\t"  + ctime);
            }
        }
        public void 重置Output()
        {
            全局变量.内容.Clear();
            全局变量.内容.Append("#####  " + DateTime.Now.ToString() + "  #####\r\n");
        }
        
        private String 时间戳(int time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var output = startTime.AddSeconds(time);
            return output.ToString();
        }

        public void oops(string code,string text)
        {
            if(code != null)
            {
                text = "错误("+ code + "): " + text;
            }
            if (全局变量.错误弹窗的状态 == false)
            {
                全局变量.错误弹窗的状态 = true;
                报错 dig = new 报错();
                dig.Show();
            }
            报错.列表更新(DateTime.Now.ToString(), text);
        }

        //废弃了
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

                //this.textBox1.Text = 输出;
                UIprint(输出);

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
                    Bili_favorites_list.Output.列表更新(全局变量.ML.ToString(), "无", "无", "无", "0", "", "-");

                    /*if (textBox6.Text != "")
                    {
                        await Task.Delay(int.Parse(textBox6.Text));
                    }
                    */
                    await Task.Delay( (int)numericUpDown2.Value );
                    /* 
                    else if (int.Parse(textBox6.Text) >= 10000)
                    {
                        await Task.Delay(9999);
                    }
                    */
                    /*else
                    {
                        await Task.Delay(1);
                    }*/
                    全局变量.ML++;
                    if (全局变量.ML - long.Parse(全局变量.起始) >= 全局变量.步幅)
                    {
                        this.textBox3.Text = 全局变量.ML.ToString();
                        输出内容(true,true);
                        全局变量.起始 = this.textBox3.Text;
                        //全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                        重置Output();
                    }
                    else if (全局变量.ML < 0 || 全局变量.ML == long.MaxValue)
                    {
                        this.textBox3.Text = "0";
                        输出内容(true);
                        this.button1.Enabled = true;
                        全局变量.起始 = this.textBox3.Text;
                        全局变量.终止 = "9223372036854775807";
                        //全局变量.内容 = "";
                        全局变量.内容.Clear();
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
                        //全局变量.内容 = "";
                        全局变量.内容.Clear();
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
                Bili_favorites_list.Output.列表更新(全局变量.ML.ToString(), "UID" + uid, name, title, num, intro, 时间戳(ctime));
                /*if (textBox6.Text != "")
                {
                    await Task.Delay(int.Parse(textBox6.Text));
                }*/
                await Task.Delay( (int)numericUpDown2.Value );
                /* 
                else if (int.Parse(textBox6.Text) >= 10000)
                {
                    await Task.Delay(9999);
                }
                */
                /*else
                {
                    await Task.Delay(1);
                }
                */
                全局变量.ML++;
                if (全局变量.ML - long.Parse(全局变量.起始) >= 全局变量.步幅)
                {
                    this.textBox3.Text = 全局变量.ML.ToString();
                    输出内容(true, true);
                    全局变量.起始 = this.textBox3.Text;
                    //全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                    重置Output();
                }
                else if (全局变量.ML < 0 || 全局变量.ML == long.MaxValue)
                {
                    this.textBox3.Text = "0";
                    输出内容(true);
                    this.button1.Enabled = true;
                    全局变量.起始 = this.textBox3.Text;
                    全局变量.终止 = "9223372036854775807";
                    //全局变量.内容 = "";
                    全局变量.内容.Clear();
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
                    //全局变量.内容 = "";
                    全局变量.内容.Clear();
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
                        Bili_favorites_list.报错.列表更新(报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_favorites_list.报错.列表更新(报错时间, "异常错误");
                    }
                }
                else
                {
                    if (错了呀.Length > 0)
                    {
                        Bili_favorites_list.报错.列表更新(报错时间, 错了呀[0]);
                    }
                    else
                    {
                        Bili_favorites_list.报错.列表更新(报错时间, "异常错误");
                    }
                }

                //报错了就重试喽...
                if (全局变量.运行状态 != false)
                {
                    /*if (textBox6.Text != "")
                    {
                        await Task.Delay(int.Parse(textBox6.Text));
                    }
                    */
                    await Task.Delay( (int)numericUpDown2.Value );
                    /* 
                    else if (int.Parse(textBox6.Text) >= 10000)
                    {
                        await Task.Delay(9999);
                    }
                    */
                    /*else
                    {
                        await Task.Delay(1);
                    }*/
                    HTTPGET();
                    return;
                }
            }
        }

        //主循环运行的地方
        async public void run_loop()
        {
            while (全局变量.运行状态 != false)
            {
                await run();
            }
        }

        //真正运行的地方
        async public Task run()
        {
            int toover = 0; //0 正常; 1 步进; 2 截止
            List<Task<(bool, string[])>> list = new List<Task<(bool, string[])>>();
            int 队列 = 全局变量.队列;
            if (全局变量.ML + 队列 > long.Parse(全局变量.终止) ) //是否戒指
            {
                队列 = (int)((全局变量.ML + 队列) - long.Parse(全局变量.终止));
                队列 = 全局变量.队列 - 队列;
                toover = 2;
            }
            if (全局变量.ML + 队列 > long.Parse(全局变量.起始) + (long)全局变量.步幅 ) //是否步进
            {
                队列 = (int)((全局变量.ML + 队列) - (long.Parse(全局变量.起始) + (long)全局变量.步幅) );
                队列 = 全局变量.队列 - 队列;
                toover = 1;
            }

            for(int i = 0; i < 队列; i++)
            {
                string ml = (全局变量.ML + i).ToString();
                /*int ii = i;
                //添加任务
                list.Add( Task.Run<(bool, string[])> ( async () =>
                {
                    int delay = ( (int)(UIget<decimal>(numericUpDown2) ) / (全局变量.队列 + 0) );
                    if(delay < 1)
                    {
                        delay = 1;
                    }
                    //Console.WriteLine( delay * (ii + 1) );
                    await Task.Delay( delay * (ii + 1) );
                    return await http(全局变量.编辑 + ml + 全局变量.后缀, ml);
                }) );*/
                //添加任务
                list.Add(http(全局变量.编辑 + ml + 全局变量.后缀, ml));
            }
            await Task.WhenAll(list);
            
            if (全局变量.运行状态 != true) //强制终止运行
            {
                if (this.IsHandleCreated == false)
                {
                    this.button3.Enabled = true;
                }
                else
                {
                    this.Invoke(new MethodInvoker( () =>
                    {
                        this.button3.Enabled = true;
                    }));
                }
                return;
            }

            //提取
            foreach (var task in list)
            {
                var(status,output) = task.Result;
                if(status == false) //返回错误就重试
                {
                    oops(output[0], output[1]);
                    await Task.Delay(60000);
                    if (全局变量.运行状态 != true) //强制终止运行
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
                        return;
                    }
                    //await run();
                    return;
                }
                if(output == null)
                {
                    continue;
                }
                写进Output(output[0], output[1], output[2], output[3], output[4], output[5], output[6]);
                Bili_favorites_list.Output.列表更新(output[0], output[1], output[2], output[3], output[4], output[5], output[6]);
            }

            //到戒指上限了就停止 //0 正常; 1 步进; 2 截止
            if (toover == 1)
            {
                全局变量.ML = long.Parse(全局变量.起始) + (long)全局变量.步幅;
                输出内容(true,true);
                //全局变量.内容 = "#####  " + DateTime.Now.ToString() + "  #####\r\n";
                重置Output();
                全局变量.起始 = 全局变量.ML.ToString();
                UIupdate(textBox3, 全局变量.起始 );
                全局变量.ML = 全局变量.ML - 全局变量.队列;
                if (UIget<bool>(this.checkBox2) == true) //尽可能地减小内存开销
                {
                    Output.lists.Clear();
                    //Output.让我看看.listView1.Clear();
                    Output.让我看看.listView1.VirtualListSize = 0;
                }
                GC.Collect();
            }
            if (toover == 2)
            {
                全局变量.运行状态 = false;
                UIupdate(textBox3,UIget<string>(textBox4));
                UIupdate(textBox4, (long.Parse(UIget<string>(textBox4)) + long.Parse(UIget<string>(textBox7)) ).ToString());
                全局变量.ML = long.Parse(全局变量.终止);
                输出内容(true);
                //全局变量.内容 = "";
                全局变量.内容.Clear();
                if (this.IsHandleCreated == false)
                {
                    this.button1.Enabled = true;
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                    //全局变量.ML = Output.lists[Output.lists.Count - 1].SubItems[0].Text;
                }
                else
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        this.button1.Enabled = true;
                        this.button2.Enabled = false;
                        this.button3.Enabled = false;
                    }));
                }
                return;
            }
            else
            {
                全局变量.ML = 全局变量.ML + 全局变量.队列;
                //run();
            }
        }
        
        //网络请求的地方
        async public Task<(bool,string[])> http(string url,string ml)
        {
            HttpResponseMessage res = null;

            Random random = new Random();
            全局变量.client.DefaultRequestHeaders.Remove("Sec-Ch-Ua-Platform");
            全局变量.client.DefaultRequestHeaders.Remove("User-Agent");
            全局变量.client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", httpheader.Platform[random.Next(httpheader.Platform.Length)]);
            全局变量.client.DefaultRequestHeaders.Add("User-Agent", httpheader.uas[random.Next(httpheader.uas.Length)]);
            random = null;

            await tryget(); //麻烦，尝试自动重试

            async Task tryget() //失败自动重试
            {
                try
                {
                    res = await 全局变量.client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    oops( null, ex.ToString().Split(new[] { Environment.NewLine } , StringSplitOptions.None)[0] );
                    //await Task.Delay( (int)UIget<decimal>(numericUpDown2) );
                    if (全局变量.运行状态 != true)
                    {
                        Console.WriteLine("检测到已暂停了诶");
                        return;
                    }
                    await Task.Delay( 1000 );
                    await tryget();
                    return;
                }
            }

            if (全局变量.运行状态 != true)
            {
                Console.WriteLine("检测到已暂停了诶");
                return (true, null);
            }
            var CODE = res.StatusCode;
            String 获取状态 = "等待";
            if (CODE.ToString() == "OK")
            {
                获取状态 = "成功";
            }
            else
            {
                获取状态 = "失败";
            }
            UIupdate(this.toolStripStatusLabel2, 获取状态 + " (" + ((int)CODE).ToString() + ")");
            UIupdate(this.toolStripStatusLabel6, url);
            //String 输出 = await res.Content.ReadAsStringAsync();
            String 输出;
            //解压缩
            if (res.Content.Headers.ContentEncoding.FirstOrDefault() == "gzip")
            {
                Stream ms = await res.Content.ReadAsStreamAsync();
                GZipStream gzips = new GZipStream(ms, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gzips);
                输出 = sr.ReadToEnd();
                //ms.Dispose();
                gzips.Dispose();
                sr.Dispose();
            }
            else
            {
                输出 = await res.Content.ReadAsStringAsync();
            }
            //UIupdate(this.textBox1,输出);
            输出 = Regex.Replace(输出,@"[\u02b0-\u036f\u0900-\u10ff\u2700-\u27bf\u1200-\u1dff]","");
            //过滤掉不需要的字符，因为 wine 不兼容

            UIprint(输出);
            //风控检查
            if ( ((int)CODE).ToString() == "412")
            {
                return (false, new string[] { ((int)CODE).ToString(), "由于触发哔哩哔哩安全风控策略，该次访问请求被拒绝" });
            }
            else if( ((int)CODE).ToString() != "200")
            {
                string code1 = ((int)CODE).ToString();
                string msg1 = "我也不知道是啥问题, 反正报错哩";
                try
                {
                    JObject jo1 = JObject.Parse(输出);
                    code1 = (string)jo1["code"];
                    msg1 = (string)jo1["message"];
                    jo1 = null;
                }
                catch { }
                res.Dispose();
                输出 = null;
                return (false, new string[] { code1, msg1 });
            } //传输层方面错误

            JObject jo = JObject.Parse(输出);
            //Console.WriteLine("jo: " + jo);
            string code = (string)jo["code"];
            string msg = (string)jo["message"];
            switch (code)
            {
                case "0": //正常放行
                    break;
                case "-403": //权限不够
                    await returnnull();
                    return (true, new string[] { ml, "无", "无", "无", "0", "", "-" });
                default: //既不是 0 又不是 403 说明是其他错误
                    return (false, new string[] { code, msg });
            } //业务层方面错误
            var null1 = 输出.IndexOf("\"data\":null");
            var null2 = 输出.IndexOf("\"info\":null,");
            if (null1 != -1 || null2 != -1) //权限不够, 组织访问
            {
                await returnnull();
                return (true, new string[] { ml, "无", "无", "无", "0", "", "-" } );
            } //时不时返回个 null 我也是服了
            async Task returnnull()
            {
                Console.WriteLine("错误: 权限不够, 阻止访问 " + url);
                UIupdate(this.toolStripStatusLabel3, "ML" + ml);
                UIupdate(this.toolStripStatusLabel4, "UID0");
                UIupdate(this.toolStripStatusLabel5, "默认收藏夹");
                await Task.Delay((int)UIget<decimal>(numericUpDown2));
                //内存回收
                res.Dispose();
                jo = null;
                输出 = null;
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
            UIupdate(this.toolStripStatusLabel3, "ML" + 全局变量.ML);
            UIupdate(this.toolStripStatusLabel4, "UID" + uid);
            UIupdate(this.toolStripStatusLabel5, title);
            /*写进Output(ml,
                    "UID" + uid, name, title, num, intro, 时间戳(ctime));
            Bili_favorites_list.Output.列表更新(Bili_favorites_list.Output.让我看看.listView1, ml,
                    "UID" + uid, name, title, num, intro, 时间戳(ctime));*/
            /*if (UIget(textBox6) != "")
            {
                await Task.Delay(int.Parse( UIget(textBox6 ) ));
            }
            else
            {
                await Task.Delay(1);
            }*/

            //转义字符
            foreach (var esc in 合并.ESC)
            {
                title = title.Replace(esc.Key,esc.Value);
                intro = intro.Replace(esc.Key,esc.Value);
                name = name.Replace(esc.Key,esc.Value);
            }

            //垃圾回收
            //client.Dispose();
            res.Dispose();
            //GC.Collect();
            输出 = null;
            jo = null;
            info = null;
            upper = null;
            await Task.Delay( (int)UIget<decimal>(numericUpDown2) );
            return (true, new string[] { ml, "UID" + uid, name, title, num, intro, 时间戳(ctime) });
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
        public T UIget<T>(Control control)
        {
            if(this.IsHandleCreated == false)
            {
                if(control is System.Windows.Forms.NumericUpDown numup) { 
                    return (T)(object)numup.Value;
                }
                else if (control is System.Windows.Forms.CheckBox checkbox)
                {
                    return (T)(object)checkbox.Checked;
                }
                else
                {
                    return (T)(object)control.Text;
                }
            }
            else
            {
                object t = null;
                this.Invoke(new MethodInvoker(() =>
                {
                    if (control is System.Windows.Forms.NumericUpDown numup)
                    {
                        t = (T)(object)numup.Value;
                    }
                    else if (control is System.Windows.Forms.CheckBox checkbox)
                    {
                        t = (T)(object)checkbox.Checked;
                    }
                    else
                    {
                        t = control.Text;
                    }
                }));
                return (T)(object)t;
            }
        }

        //修改textbox1
        public static string print2textbox1 = "";
        public void UIprint(string text)
        {
            print2textbox1 = text;
            if (this.WindowState != FormWindowState.Minimized)
            {
                UIupdate(this.textBox1, print2textbox1);
            }
        }

        //恢复时, 从列表上获取最后一个序号
        public long mlgetinoutput()
        {
            long num;
            if (Output.lists.Count > 0){
                num = long.Parse(Output.lists[Output.lists.Count - 1].SubItems[0].Text) + 1;
            }
            else
            {
                num = long.Parse(全局变量.起始);
            }
            return num;
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
            //this.button3.Enabled = true;
            全局变量.运行状态 = false;
            SelectNextControl(ActiveControl, false, true, true, true);
            toolStripStatusLabel1.Text = "暂停";
        }

        //继续
        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
            this.button2.Enabled = true;
            全局变量.ML = mlgetinoutput();
            全局变量.运行状态 = true;
            SelectNextControl(ActiveControl, false, true, true, true);
            toolStripStatusLabel1.Text = "进行中";

            run_loop();
            //HTTPGET();
        }

        //打开就运行
        private void Form1_Load(object sender, EventArgs e)
        {
            this.label6.Text = "v" + 全局变量.版本;
            Output output = new Output();
            output.Show();
            string path = Application.StartupPath + "/Setting.ini";
            //检查配置文件在不在喽
            if (System.IO.File.Exists(path) == false)
            {
                String[] 配置文档 = { "版本=" + 全局变量.版本, 
                    "编辑=" + textBox2.Text, 
                    "起始=" + textBox3.Text,
                    "终止=" + textBox4.Text, 
                    "后缀=" + textBox5.Text, 
                    "延时=" + numericUpDown2.Value.ToString(),
                    "步幅=" + textBox7.Text, 
                    "步幅=" + textBox7.Text, 
                    "队列=" + this.numericUpDown1.Value.ToString(), 
                    "列表自动更新=" + this.checkBox1.Checked.ToString(), 
                    "减小内存开销=" + this.checkBox2.Checked.ToString(), 
                    "[By:yuhang0000]" };
                System.IO.File.WriteAllLines(path, 配置文档);
                全局变量.配置文档 = 配置文档;
                //this.textBox1.Text = "就绪。";
                UIprint("就绪。");
            }
            else
            {
                try //我爱死 try 和 catch 这两个方法啦，爱用🤍
                {
                    全局变量.配置文档 = System.IO.File.ReadAllLines(path);
                    this.textBox2.Text = 全局变量.配置文档[1].Replace("编辑=", "");
                    this.textBox3.Text = 全局变量.配置文档[2].Replace("起始=", "");
                    this.textBox4.Text = 全局变量.配置文档[3].Replace("终止=", "");
                    this.textBox5.Text = 全局变量.配置文档[4].Replace("后缀=", "");
                    this.numericUpDown2.Value = decimal.Parse(全局变量.配置文档[5].Replace("延时=", ""));
                    this.textBox7.Text = 全局变量.配置文档[6].Replace("步幅=", "");
                    this.numericUpDown1.Value = decimal.Parse(全局变量.配置文档[7].Replace("队列=", ""));
                    this.checkBox1.Checked = bool.Parse(全局变量.配置文档[8].Replace("列表自动更新=", ""));
                    this.checkBox2.Checked = bool.Parse(全局变量.配置文档[9].Replace("减小内存开销=", ""));
                    //this.textBox1.Text = "就绪。";
                    UIprint("就绪。");
                }
                catch (Exception)
                {
                    System.Media.SystemSounds.Beep.Play();
                    toolStripStatusLabel6.Text = "配置文档好像坏欸。";
                    //textBox1.Text = "就绪。\r\n额，配置文档好像坏欸。";
                    UIprint("就绪。\r\n额，配置文档好像坏欸。");
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
            string ml;
            if (Output.lists.Count > 0)
            {
                ml = (long.Parse(Output.lists[Output.lists.Count - 1].SubItems[0].Text)
                    + 1).ToString();
            }
            else
            {
                ml = UIget<string>(textBox3);
            }
            String[] 配置文档 = { 
                "版本=" + 全局变量.版本, 
                "编辑=" + textBox2.Text, 
                "起始=" + ml,
                "终止=" + textBox4.Text, 
                "后缀=" + textBox5.Text, 
                "延时=" + numericUpDown2.Value.ToString(),
                "步幅=" + textBox7.Text, 
                "队列=" + this.numericUpDown1.Value.ToString(), 
                "列表自动更新=" + this.checkBox1.Checked.ToString(), 
                "减小内存开销=" + this.checkBox2.Checked.ToString(), 
                "", 
                "[By:yuhang0000]" };
            System.IO.File.WriteAllLines( Application.StartupPath + "/Setting.ini", 配置文档);
        }

        private void 输出内容(bool close = false, bool bizui = false)
        {
            //Console.WriteLine(全局变量.内容);
            if (全局变量.内容.Length == 0 && close == false)
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("保存失败，内容是空滴。", "失败哩  ╥﹏╥...");
                return;
            }
            else if (全局变量.内容.ToString().Contains("\r\n\r\n") == false && close == true)
            {
                保存配置文档();
                return;
            } //只保存配置文档, 不输出
            else if (close == true && bizui == false)
            {
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                全局变量.运行状态 = false;
                toolStripStatusLabel1.Text = "暂停"; 
                保存配置文档();
            } //退出或者已经完成了
            else if (close == true && bizui == true)
            {
                保存配置文档();
            } //自动保存
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
                /*string text = 全局变量.内容.ToString();
                System.IO.File.WriteAllText(path, text);
                text = null; //释放掉 */
                System.IO.File.WriteAllText(path, 全局变量.内容.ToString());
            }
            catch
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("保存失败，无法写入文件，在:\r\n" + path, "失败哩  ╥﹏╥...");
                return;
            }
            GC.Collect();
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
        /*private void textBox6_TextChanged(object sender, EventArgs e)
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
        }*/

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
            /*string path = Application.StartupPath + @"\Output\";
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
            */
            合并 dig = new 合并();
            dig.Show();
            this.button5.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(Output.lists.Count > 0 && this.checkBox1.Checked == true)
            {
                Output.让我看看.listView1.VirtualListSize = Output.lists.Count;
            }
        }

        //恢复焦点时
        private void Form1_Activated(object sender, EventArgs e)
        {
            UIupdate(textBox1, print2textbox1);
        }
    }
}
