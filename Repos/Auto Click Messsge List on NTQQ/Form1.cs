using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;

namespace Auto_Click_Messsge_List_on_NTQQ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //生成 0 - 100 的图标
            int n = -1;
            while (n < 103)
            {
                if (n == -1)
                {
                    sss.numicon[n + 1] = img("ZZZ");
                }
                else if (n == 102)
                {
                    sss.numicon[n + 1] = img(num: " ");
                }
                else if (n == 101)
                {
                    sss.numicon[n + 1] = img(num: "100", color1: Color.Red);
                }
                else
                {
                    sss.numicon[n + 1] = img(n.ToString());
                }
                n++;
            }
            InitializeComponent();
        }

        //网络通讯
        public async void websocket()
        {
            await Task.Delay(1);
            try
            {
                //sss.socket = new WebSocketServer("ws://127.0.0.1:65533");
                sss.socket = new WebSocketServer("ws://" + sss.ip + ":" + sss.port);
                Console.WriteLine("IP: " + sss.ip + " 端口: " + sss.port);
                textout("IP: " + sss.ip + " 端口: " + sss.port);
                Console.WriteLine("尝试等待连接");
                textout("尝试等待连接");
                sss.socket.Start(SocketWrapper =>
                {
                    bool stop = false;
                    //时钟循环
                    async void send()   //发送
                    {
                        while (stop != true)
                        {
                            try
                            {
                                if (sss.senttext == "")
                                {
                                    await SocketWrapper.Send(DateTime.Now.ToString());
                                    Console.WriteLine(DateTime.Now.ToString());
                                }
                                else
                                {
                                    await SocketWrapper.Send(sss.senttext);
                                    Console.WriteLine(sss.senttext);
                                    textout("发送讯息: " + sss.senttext);
                                    if (textout(zzz: "get") == sss.senttext)
                                    {
                                        sss.senttext = "";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                textout("出现异常: " + ex.Message);
                            }
                            await Task.Delay(1000);
                        }
                    };

                    SocketWrapper.OnOpen = () =>
                    {
                        send();
                        Console.WriteLine("连接成功!");
                        textout("连接成功!");
                    };
                    SocketWrapper.OnClose = () =>
                    {
                        Console.WriteLine("连接断开!");
                        textout("连接断开!");
                        stop = true;
                        SocketWrapper.Close();
                        //tpc();
                    };
                    SocketWrapper.OnMessage = message =>
                    {
                        Console.WriteLine("接收讯息: " + message);
                        textout("接收讯息: " + message);
                        if (message.IndexOf(";") != -1) //读取指令
                        {
                            string[] command = message.Split(';');
                            commands(command);
                        }
                        else if (message.IndexOf(",") != -1)
                        {
                            string[] command = message.Split(',');
                            commands(command);
                        }
                    };

                });
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                if (MessageBox.Show("连接错误: \r\n" + ex.Message + "\r\n\r\n当前地址: " + "ws://" + sss.ip + ":" + sss.port
                     + "\r\n是否重试?", "Oops! ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    websocket();
                    return;
                }
                else
                {
                    Application.Exit();
                }
            }
            
            if(sss.manualpause == true)
            {
                textout("暂停计时");
            }
        }

        [System.Runtime.InteropServices.DllImport("user32")]    //鼠标操作
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo); 
        [DllImport("user32")]   //鼠标移动
        public static extern bool SetCursorPos(int X, int Y); 
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]  //键盘输入
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        //接收指令
        public async void commands(string[] command)
        {
            try
            {
                if (command[0] == "mouse" || command[0] == "Mouse" || command[0] == "m" || command[0] == "M")   //鼠标
                {
                    int[] xy = cursor();
                    SetCursorPos(int.Parse(command[2]), int.Parse(command[3])); //先移动鼠标位置
                    if (command[1] == "move" || command[1] == "Move" || command[1] == "m" || command[1] == "M")//只移动
                    {
                        textout("完成鼠标输入动作, 移动至坐标: （" + command[2] + "," + command[3] + ")");
                        return;
                    }
                    else if (command[1] == "wheel" || command[1] == "w" || command[1] == "W")    //滚轮
                    {
                        mouse_event(0x0800, 0, 0, int.Parse(command[4]), 0);
                        if (int.Parse(command[4]) < 0)
                        {
                            textout("完成鼠标输入动作, 坐标: （" + command[2] + "," + command[3] + ") 向下滚动: " + command[4]);
                        }
                        else
                        {
                            textout("完成鼠标输入动作, 坐标: （" + command[2] + "," + command[3] + ") 向上滚动: " + command[4]);
                        }
                    }
                    else if (command[1] == "click" || command[1] == "c" || command[1] == "C")   //点击
                    {
                        mouse_event(0x0002, 0, 0, 0, 0);
                        if (command.Length > 4)
                        {
                            await Task.Delay(int.Parse(command[4]));
                            textout("完成鼠标输入动作, 鼠标点击, 坐标: （" + command[2] + "," + command[3] + ") 持续: " + command[4]);
                        }
                        else
                        {
                            textout("完成鼠标输入动作, 鼠标点击, 坐标: （" + command[2] + "," + command[3] + ")");
                        }
                        mouse_event(0x0004, 0, 0, 0, 0);
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals(command[1],"right-click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"right_click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"right click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"Secondary Click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"Secondary_Click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"Secondary-Click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"rc") )   //右键
                    {
                        mouse_event(0x0008, 0, 0, 0, 0);
                        if (command.Length > 4)
                        {
                            await Task.Delay(int.Parse(command[4]));
                            textout("完成鼠标输入动作, 鼠标右键, 坐标: （" + command[2] + "," + command[3] + ") 持续: " + command[4]);
                        }
                        else
                        {
                            textout("完成鼠标输入动作, 鼠标右键, 坐标: （" + command[2] + "," + command[3] + ")");
                        }
                        mouse_event(0x0010, 0, 0, 0, 0);
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals(command[1], "middle-click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1], "middle_click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1], "middle click") 
                        || StringComparer.OrdinalIgnoreCase.Equals(command[1],"mc") )   //中键
                    {
                        mouse_event(0x0020, 0, 0, 0, 0);
                        if (command.Length > 4)
                        {
                            await Task.Delay(int.Parse(command[4]));
                            textout("完成鼠标输入动作, 鼠标中键, 坐标: （" + command[2] + "," + command[3] + ") 持续: " + command[4]);
                        }
                        else
                        {
                            textout("完成鼠标输入动作, 鼠标中键, 坐标: （" + command[2] + "," + command[3] + ")");
                        }
                        mouse_event(0x0040, 0, 0, 0, 0);
                    }
                    SetCursorPos(xy[0], xy[1]);    //还原鼠标位置
                }
                else if (command[0] == "key" || command[0] == "Key" || command[0] == "k" || command[0] == "K"
                    || command[0] == "Keycode" || command[0] == "keycode" || command[0] == "KeyCode"
                    || command[0] == "KCode" || command[0] == "Kcode" || command[0] == "kcode"
                    || command[0] == "kc" || command[0] == "Kc" || command[0] == "KC")   //键盘
                {
                    Keys key = Keys.None;
                    if (command[0] == "key" || command[0] == "Key" || command[0] == "k" || command[0] == "K")    //输入的不是键值
                    {
                        if (sss.keys.TryGetValue(command[1], out key) == false)
                        {
                            textout("未知键值");
                            return;
                        }
                        if (key == Keys.None)
                        {
                            textout("未知键值");
                            return;
                        }
                    }
                    else
                    {
                        int areunum;
                        if (int.TryParse(command[1], out areunum) == false) //判断是不是数字
                        {
                            textout("未知键值");
                            return;
                        }
                        key = (Keys)areunum; //键值
                    }
                    uint keyup = 0x0002;    //键盘抬起动作
                    if (command.Length > 2)
                    {
                        keybd_event(key, 0, 0, 0);
                        await Task.Delay(int.Parse(command[2]));
                        keybd_event(key, 0, keyup, 0);
                        textout("完成键盘输入动作, 键值: " + key + " 持续: " + command[2]);
                    }
                    else
                    {
                        keybd_event(key, 0, 0, 0);
                        keybd_event(key, 0, keyup, 0);
                        textout("完成键盘输入动作, 键值: " + key);
                    }
                }
                else if (command[0] == "post" || command[0] == "Post" || command[0] == "p" || command[0] == "P") //响应
                {
                    if (command[1] == "run" || command[1] == "Run")
                    {
                        textout("对方已接收");
                        sss.senttext = "";
                    }
                    else if (command[1] == "Ok" || command[1] == "ok" || command[1] == "OK")
                    {
                        textout("对方处理完成!");
                        await Task.Delay(1000);
                        sss.runing = false;
                    }
                    else if (command[1] == "err" || command[1] == "error")
                    {
                        sss.missnum++;
                        if (sss.missnum > sss.missmax)
                        {
                            textout("对方出现错误, 超过最大重试次数!");
                            sss.missnum = 0;
                        }
                        else
                        {
                            textout("对方出现错误, 第 " + sss.missnum + " 次重试");
                            //textout("对方出现错误");
                            sss.senttext = sss.posttext;
                        }
                    }
                }
                else if (command[0] == "app" || command[0] == "App" || command[0] == "a" || command[0] == "A" ||
                    command[0] == "Application" || command[0] == "application" || command[0] == "exe" || command[0] == "Exe") //打开程序
                {
                    try
                    {
                        if (command.Length > 2)
                        {
                            textout("启动程序: " + command[1] + " " + command[2]);
                            Process.Start(command[1], command[2]);
                        }
                        else
                        {
                            textout("启动程序: " + command[1]);
                            Process.Start(command[1]);
                        }
                    }
                    catch (Exception ex)
                    {
                        SystemSounds.Beep.Play();
                        textout("尝试调起目标程序出现错误: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Beep.Play();
                /*string commandsss = "";
                foreach(string commandss in command)
                {
                    commandsss = commandsss + commandss + ",";
                }
                commandsss = commandsss.Substring(0, commandsss.Length - 1);
                textout("指令错误: \r\n" + ex.Message + "\r\n" + commandsss);*/
                textout("指令错误: " + ex.Message);
            }
        }

        public string textout(string text = "",string zzz = "set")
        {
            //如果超出文本框上限，就把旧讯息给删掉
            if(this.textBox2.Text.Length > 1073741823)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    String cut = "\n";
                    int lll = this.textBox2.Text.Length - this.textBox2.Text.IndexOf(cut) - cut.Length;
                    this.textBox2.Text = this.textBox2.Text.Substring(this.textBox2.Text.IndexOf(cut) + cut.Length, lll);
                }));
            }
            if (zzz == "set")
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (this.textBox2.Text == "")
                    {
                        this.textBox2.Text = DateTime.Now.ToString() + " | " + text;
                    }
                    else
                    {
                        this.textBox2.AppendText("\r\n" + DateTime.Now.ToString() + " | " + text);
                    }
                }));
                return null;
            }
            else if(zzz == "get")
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    text = this.textBox1.Text;
                }));
                return text;
            }
            return null;
        }

        [DllImport("user32")]
        public static extern int GetCursorPos(out System.Drawing.Point lpPoint);

        //获取鼠标位置
        public int[] cursor()
        {
            int[] xy = new int[2];
            Point mp = new Point();
            GetCursorPos(out mp);
            xy[0] = mp.X;
            xy[1] = mp.Y;
            return xy;
        }

        //全局变量
        public static class sss
        {
            public static Icon[] numicon = new Icon[104];
            public static int mx = 0;
            public static int my = 0;
            public static int[] xy;
            public static int press = 102;
            public static int 阈值 = 166;
            public static int 强度 = 3;
            public static int missnum = 0;
            public static int missmax = 5;
            public static WebSocketServer socket;
            public static string appname = "QQ";
            public static string senttext = "";
            public static string posttext = "Chatback;举个栗子";
            public static string ip = "127.0.0.1";
            public static string port = "65533";
            public static bool runing = false;
            public static bool minwindows = false;
            public static bool manualpause = false;
            public static bool emptyicon233 = false;
            //创建个字典来键值衍射
            public static Dictionary<string, Keys> keys = new Dictionary<string, Keys>(StringComparer.OrdinalIgnoreCase) {
                {"esc",Keys.Escape},
                {"退出",Keys.Escape},
                {"f1",Keys.F1},
                {"f2",Keys.F2},
                {"f3",Keys.F3},
                {"f4",Keys.F4},
                {"f5",Keys.F5},
                {"f6",Keys.F6},
                {"f7",Keys.F7},
                {"f8",Keys.F8},
                {"f9",Keys.F9},
                {"f10",Keys.F10},
                {"f11",Keys.F11},
                {"f12",Keys.F12},
                {"pause",Keys.Pause},
                {"暂停",Keys.Pause},
                {"del",Keys.Delete},
                {"删除",Keys.Delete},
                {"home",Keys.Home},
                {"顶部",Keys.Home},
                {"end",Keys.End},
                {"底部",Keys.End},
                {"pgup",Keys.PageUp},
                {"pageup",Keys.PageUp},
                {"上一页",Keys.PageUp},
                {"pgdn",Keys.PageDown},
                {"pgdown",Keys.PageDown},
                {"pagedown",Keys.PageDown},
                {"下一页",Keys.PageDown},
                {"`",Keys.Oem3},
                {"1",Keys.D1},
                {"2",Keys.D2},
                {"3",Keys.D3},
                {"4",Keys.D4},
                {"5",Keys.D5},
                {"6",Keys.D6},
                {"7",Keys.D7},
                {"8",Keys.D8},
                {"9",Keys.D9},
                {"0",Keys.D0},
                {"-",Keys.OemMinus},
                {"=",Keys.Oemplus},
                {"~",Keys.Oem3},
                {"!",Keys.D1},
                {"@",Keys.D2},
                {"#",Keys.D3},
                {"$",Keys.D4},
                {"%",Keys.D5},
                {"^",Keys.D6},
                {"&",Keys.D7},
                {"*",Keys.Multiply},
                {"(",Keys.D9},
                {")",Keys.D0},
                {"_",Keys.OemMinus},
                {"+",Keys.Add},
                {"<==",Keys.Back},
                {"backspace",Keys.Back},
                {"bkspace",Keys.Back},
                {"numlk",Keys.NumLock},
                {"numlock",Keys.NumLock},
                {"小键盘",Keys.NumLock},
                {"小键盘锁",Keys.NumLock},
                {"数字键锁",Keys.NumLock},
                {"tap",Keys.Tab},
                {"Q",Keys.Q},
                {"W",Keys.W},
                {"E",Keys.E},
                {"R",Keys.R},
                {"T",Keys.T},
                {"Y",Keys.Y},
                {"U",Keys.U},
                {"I",Keys.I},
                {"O",Keys.O},
                {"P",Keys.P},
                {"A",Keys.A},
                {"S",Keys.S},
                {"D",Keys.D},
                {"F",Keys.F},
                {"G",Keys.G},
                {"H",Keys.H},
                {"J",Keys.J},
                {"K",Keys.K},
                {"L",Keys.L},
                {"Z",Keys.Z},
                {"X",Keys.X},
                {"C",Keys.C},
                {"V",Keys.V},
                {"B",Keys.B},
                {"N",Keys.N},
                {"M",Keys.M},
                {"[",Keys.Oem4},
                {"{",Keys.Oem4},
                {"]",Keys.Oem6},
                {"}",Keys.Oem6},
                {"\\",Keys.Oem5},
                {"|",Keys.Oem5},
                {"caps",Keys.CapsLock},
                {"capslk",Keys.CapsLock},
                {"capslock",Keys.CapsLock},
                {"大写锁定",Keys.CapsLock},
                {"锁定大写",Keys.CapsLock},
                {"锁定大小写",Keys.CapsLock},
                {"大小写锁定",Keys.CapsLock},
                {";",Keys.Oem1},
                {"；",Keys.Oem1},
                {"分号",Keys.Oem1},
                {":",Keys.Oem1},
                {"'",Keys.Oem7},
                {"\"",Keys.Oem7},
                {",",Keys.Oemcomma},
                {"，",Keys.Oemcomma},
                {"逗号",Keys.Oemcomma},
                {"<",Keys.Oemcomma},
                {".",Keys.OemPeriod},
                {">",Keys.OemPeriod},
                {"/",Keys.Oem2},
                {"?",Keys.Oem2},
                {"enter",Keys.Enter},
                {"确定",Keys.Enter},
                {"return",Keys.Return},
                {"shift",Keys.ShiftKey},
                {"lshift",Keys.LShiftKey},
                {"rshift",Keys.RShiftKey},
                {"ctrl",Keys.ControlKey},
                {"lctrl",Keys.LControlKey},
                {"rctrl",Keys.RControlKey},
                {"win",Keys.LWin},
                {"lwin",Keys.LWin},
                {"rwin",Keys.RWin},
                {"alt",Keys.Alt},
                {"lalt",Keys.Alt},
                {"ralt",Keys.Alt},
                {"space",Keys.Space},
                {"up",Keys.Up},
                {"down",Keys.Down},
                {"left",Keys.Left},
                {"Right",Keys.Right},
                {"⬆️",Keys.Up},
                {"⬇️",Keys.Down},
                {"⬅️",Keys.Left},
                {"➡️",Keys.Right},
                {"↑",Keys.Up},
                {"↓",Keys.Down},
                {"←",Keys.Left},
                {"→",Keys.Right},
                {"上",Keys.Up},
                {"下",Keys.Down},
                {"左",Keys.Left},
                {"右",Keys.Right},
                {"menu",Keys.Menu},
                {"菜单",Keys.Menu},
                //{"▶️",Keys.Play},
                //{"播放",Keys.Play},
                {"▶️",Keys.MediaPlayPause},
                {"播放",Keys.MediaPlayPause},
                {"play",Keys.MediaPlayPause},
                //{"暂停",Keys.MediaPlayPause},
                {"播放暂停",Keys.MediaPlayPause},
                {"暂停播放",Keys.MediaPlayPause},
                {"播放/暂停",Keys.MediaPlayPause},
                {"播放&暂停",Keys.MediaPlayPause},
                {"播放与暂停",Keys.MediaPlayPause},
                {"播放或暂停",Keys.MediaPlayPause},
                {"⏯️",Keys.MediaPlayPause},
                {"stop",Keys.MediaStop},
                {"停止",Keys.MediaStop},
                {"终止",Keys.MediaStop},
                {"⏹️",Keys.MediaStop},
                {"■",Keys.MediaStop},
                {"next",Keys.MediaNextTrack},
                {"下一首",Keys.MediaNextTrack},
                {"下一个",Keys.MediaNextTrack},
                {"⏭️",Keys.MediaNextTrack},
                {"previous",Keys.MediaPreviousTrack},
                {"上一首",Keys.MediaPreviousTrack},
                {"上一个",Keys.MediaPreviousTrack},
                {"⏮️",Keys.MediaPreviousTrack},
                {"V+",Keys.VolumeUp},
                {"音量+",Keys.VolumeUp},
                {"V-",Keys.VolumeDown},
                {"音量-",Keys.VolumeDown},
                {"Volume+",Keys.VolumeUp},
                {"Volume-",Keys.VolumeDown},
                {"mute",Keys.VolumeMute},
                {"🔇",Keys.VolumeMute},
                {"静音",Keys.VolumeMute},
                {"shot",Keys.Snapshot},
                {"snapshot",Keys.Snapshot},
                {"printscreen",Keys.PrintScreen},
                {"print screen",Keys.PrintScreen},
                {"prtsc",Keys.PrintScreen},
                {"截屏",Keys.PrintScreen},
                {"截图",Keys.PrintScreen},
                {"scroll",Keys.Scroll},
                {"scrolllock",Keys.Scroll},
                {"scrlk",Keys.Scroll},
                {"insert",Keys.Insert},
                {"sleep",Keys.Sleep},
                {"zzz",Keys.Sleep},
                {"apps",Keys.Apps},
                {"app",Keys.Apps},
                {"help",Keys.Help},
                {"click",Keys.LButton},
                {"lmouse",Keys.LButton},
                {"mousel",Keys.LButton},
                {"lbutton",Keys.LButton},
                {"左键",Keys.LButton},
                {"rmouse",Keys.RButton},
                {"mouser",Keys.RButton},
                {"rbutton",Keys.RButton},
                {"右键",Keys.RButton},
                {"none",Keys.None},
                {"空",Keys.None},
                {"无",Keys.None},
                {"null",Keys.None},
                {"",Keys.None}
            };
        }

        //生成图像
        public Icon img(String num = "0", int imgsize = 32, float fontsize = 10f, Color? color1 = null)
        {
            Color bcolor = color1 ?? Color.Black;
            if(fontsize == 0)
            {
                fontsize = 1f;
            }
            if(imgsize == 0)
            {
                imgsize = 1;
            }
            Bitmap bitmap = new Bitmap(imgsize,imgsize);
            Graphics g = Graphics.FromImage(bitmap);

            //float textw = num.Length + fontsize;
            //Console.WriteLine(textw);
            g.Clear(Color.Transparent);
            //g.Clear(Color.FromArgb(222,222,222));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font ttt = new Font("Microsoft YaHei UI",fontsize,FontStyle.Regular);
            Brush brush = new SolidBrush(bcolor);
            Console.WriteLine( g.MeasureString(num,ttt) );
            float fontw = g.MeasureString(num,ttt).Width;
            float fonth = g.MeasureString(num,ttt).Height;

            //文字居中
            RectangleF box = new RectangleF( (imgsize - fontw) / 2 ,(imgsize - fonth) / 2, 
                fontw,fonth );
            Console.WriteLine(box.ToString());

            g.DrawString(num, ttt, brush, box);
            Icon ico = Icon.FromHandle(bitmap.GetHicon());
            this.Icon = ico;
            bitmap.Dispose();
            g.Dispose();
            return ico;
        }

        //读取配置文档
        public void readsettingini(string setting)
        {
            try
            {
                if (setting.IndexOf("posttext=") != -1)  //默认发送内容
                {
                    sss.posttext = setting.Replace("posttext=", "");
                }
                if (setting.IndexOf("appname=") != -1)  //进程名称
                {
                    sss.appname = setting.Replace("appname=", "");
                }
                if (setting.IndexOf("delay=") != -1)  //延时
                {
                    this.timer1.Interval = int.Parse(setting.Replace("delay=", ""));
                }
                if (setting.IndexOf("minwindows=") != -1)  //是否最小化
                {
                    sss.minwindows = bool.Parse(setting.Replace("minwindows=", ""));
                }
                if (setting.IndexOf("port=") != -1)  //端口
                {
                    sss.port = setting.Replace("port=", "");
                }
                if (setting.IndexOf("ip=") != -1)  //ip地址
                {
                    sss.ip = setting.Replace("ip=", "");
                }
                if (setting.IndexOf("missmax=") != -1)  //最大允许重试次数
                {
                    sss.missmax = int.Parse(setting.Replace("missmax=", ""));
                }
                if (setting.IndexOf("manualpause=") != -1)  //手动暂停
                {
                    sss.manualpause = bool.Parse(setting.Replace("manualpause=", ""));
                }
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("读取配置文档时发生错误: \r\n" + "当前读取数据: " + setting + "\r\n" + ex.Message,"Oops!");
            }
        }

        //一打开就运行
        private void Form1_Load(object sender, EventArgs e)
        {
            //img();
            if(File.Exists(@"./Setting.ini") == true){
                string[] settings = File.ReadAllLines(@"./Setting.ini");
                /*sss.posttext = settings[0];
                sss.appname = settings[1];
                this.timer1.Interval = int.Parse(settings[2]);
                sss.minwindows = bool.Parse(settings[3]);*/
                foreach(string setting in settings)
                {
                    readsettingini(setting);
                }
            }
            //this.Text = this.Text + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = sss.numicon[102];
            this.notifyIcon1.Icon = sss.numicon[102];
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = 48;
            //是否最小化
            if(sss.minwindows == true)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            this.ShowInTaskbar = false; //不显示在任务栏上
            //看看 WebSocketTest.html 文档在不在
            if(File.Exists(@"./WebSocketTest.html") == true)
            {
                this.测试ToolStripMenuItem.Visible = true;
            }
            //刚打开要不要暂停计时
            if(sss.manualpause == true)
            {
                this.trackBar3.Value = 0;
                this.timer1.Enabled = false;
                this.timer2.Enabled = true;
                this.Icon = sss.numicon[0];
                this.notifyIcon1.Icon = sss.numicon[0];
            }
            sss.xy = cursor();
            sss.mx = sss.xy[0];
            sss.my = sss.xy[1];
            Task.Run(() => { websocket(); });
        }

        //生成
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                img(this.textBox1.Text,this.trackBar2.Value,this.trackBar1.Value);
                this.label1.Text = "大小: " + this.trackBar1.Value.ToString() + " 画布: " + 
                    this.trackBar2.Value.ToString();
            }
        }

        //拖动条变化 调整 Icon 生成尺寸
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                img(this.textBox1.Text, this.trackBar2.Value, this.trackBar1.Value);
                this.label1.Text = "大小: " + this.trackBar1.Value.ToString() + " 画布: " + 
                    this.trackBar2.Value.ToString();
            }
        }

        //1-100 切换Icon
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.Icon = sss.numicon[this.trackBar3.Value];
            this.notifyIcon1.Icon = sss.numicon[this.trackBar3.Value];
            if(this.trackBar3.Value == 0)
            {
                sss.manualpause = true;
                this.timer1.Enabled = false;
                this.timer2.Enabled = true;
                textout("暂停计时");
            }
            else if(this.trackBar3.Value > 0 && sss.manualpause == true)
            {
                sss.manualpause = false;
                this.timer1.Enabled = true;
                this.timer2.Enabled = false;
                textout("恢复计时");
            }
        }

        // 计算空闲时间
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINOUTINFO
        {
            public int cbSize;  //设置结构体块容量
            public uint dwTime; //抓获时间
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINOUTINFO Plii);
        public static long GetLastInputTime()
        {
            LASTINOUTINFO vLastInputinfo = new LASTINOUTINFO();
            vLastInputinfo.cbSize = Marshal.SizeOf(vLastInputinfo);
            GetLastInputInfo(ref vLastInputinfo);
            long count = Environment.TickCount - (long)vLastInputinfo.dwTime;
            count = count / 1;
            return count;
        }

        //计时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(sss.runing == true)
            {
                return;
            }
            //Console.WriteLine(GetLastInputTime());
            if(GetLastInputTime() < this.timer1.Interval)
            {
                //Console.WriteLine(GetLastInputTime().ToString());
                sss.press = 102;
            }
            else
            {
                if (sss.press > 0)
                {
                    sss.press--;
                    if(sss.press == 0)//触发
                    {
                        //this.WindowState = FormWindowState.Minimized;
                        sss.runing = true;
                        run();
                    }
                }
            }
            this.Icon = sss.numicon[sss.press];
            this.notifyIcon1.Icon = sss.numicon[sss.press];
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);   //最大/小化指定窗口
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); //获取窗口所属的进程ID
        public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallback callPtr, int lParam); //枚举所有窗口
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);    //获取窗口标题
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);    //获取窗口是否可视
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);    //指定窗口置于前台
        [DllImport("user32.dll")]
        private static extern bool SetFocus(IntPtr hWnd);   //指定窗口获取焦点

        public void run()
        {
            //IntPtr exe = FindWindow(null, "QQ");
            Process[] exe = Process.GetProcessesByName(sss.appname);

            //if(exe == IntPtr.Zero)
            textout("######## 开始执行 ########");
            this.textBox2.AppendText("\r\n" + DateTime.Now.ToString() + " | 总计: " + exe.Length.ToString());
            if(exe.Length == 0)
            {
                Console.WriteLine("找不到进程: " + sss.appname + ".exe");
                textout("找不到进程: " + sss.appname + ".exe");
                sss.runing = false;
            }
            else
            {
                //ShowWindow(exe,9);
                bool areufind = false;
                foreach (Process proc in exe) //先批量恢复窗口
                {
                    // 枚举所有顶层窗口, 并检查它们是否属于当前进程
                    EnumWindows((hWnd, lParam) =>
                    {
                        // 获取窗口所属的进程ID
                        GetWindowThreadProcessId(hWnd, out int procId);
                        if (procId == proc.Id) {
                            int chars = 256;
                            string areuwindow = "";
                            StringBuilder Buff = new StringBuilder(chars);  //标题容器
                            GetWindowText(hWnd, Buff, chars);   //获取标题
                            if (IsWindowVisible(hWnd) == false) //是不是窗口
                            {
                                areuwindow = "\t*非窗口*";
                            }
                            this.textBox2.AppendText("\r\n" + hWnd + "\t" + Buff + areuwindow);
                            if (Buff.ToString() == sss.appname) { 
                                if(IsWindowVisible(hWnd) == true)
                                {
                                    areufind = true;
                                    ShowWindow(hWnd, 3);
                                    if (SetForegroundWindow(hWnd) == true)  //尝试获取焦点
                                    {
                                        SetFocus(hWnd);
                                    }
                                }
                            }
                        }
                        return true; // 返回 true 继续枚举, false 停止枚举
                    }, 0);

                    //this.textBox2.AppendText("\r\n" + proc.MainWindowTitle + "\t" + proc.MainWindowHandle);
                    //ShowWindow(proc.MainWindowHandle, 9);
                }
                if(areufind == false)
                {
                    textout("找不到进程: " + sss.appname + ".exe");
                    Console.WriteLine("找不到进程: " + sss.appname + ".exe");
                    //MessageBox.Show("找不到进程: QQ.exe","Oops! ");
                    sss.runing = false;
                }
                else //执行
                {
                    //是否最小化
                    if (sss.minwindows == true)
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                    sss.senttext = sss.posttext;
                    //shot();
                }
            }
            textout("######## 执行完毕 ########");
        }

        public void shot()
        {
            sss.senttext = this.textBox1.Text;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.label1.Left = this.Width - 487 + 323;
            this.button1.Left = this.Width - 487 + 209;
            this.button2.Left = this.Width - 487 + 152;
            this.button3.Left = this.Width - 487 + 266;
            this.textBox1.Width = this.Width - 487 + 133;
            this.textBox2.Width = this.Width - 487 + 445;
            this.trackBar1.Width = this.Width - 487 + 445;
            this.trackBar2.Width = this.Width - 487 + 445;
            this.trackBar3.Width = this.Width - 487 + 445;
            this.textBox2.Height = this.Height - 428 + 138;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sss.senttext = this.textBox1.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form form = new Form1_old();
            form.Show();
        }

        //最小化方式关闭
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState= FormWindowState.Minimized;
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = Application.ProductName;
            text = text + "\r\nBy: " + Application.CompanyName;
            text = text + "\r\nBuild Time: " +
                System.IO.File.GetLastWriteTime(typeof(Form1).Assembly.Location); ;
            text = text + "\r\n版本: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            text = text + "\r\n\r\n右键托盘图标退出。";
            text = text + "\r\n\r\n引用的第三方库: ";
            text = text + "\r\n# Fleck v1.2.0 \r\n   ( https://github.com/statianzo/Fleck/ )";
            text = text + "\r\n# Tesseract v5.2.0 \r\n   ( https://github.com/charlesw/tesseract/ )";
            MessageBox.Show(text,"关于");
        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer",Application.StartupPath + @"\WebSocketTest.html");
        }

        //帮助
        public void help()
        {
            string text = "指令: \r\n\r\nmouse,m,1920,1080\t鼠标移动到 (1920, 1080) 的地方";
            text = text + "\r\nmouse,c,1920,1080\t\t鼠标点击 (1920, 1080) 的地方";
            text = text + "\r\nmouse,rc,1920,1080\t鼠标右键 (1920, 1080) 的地方";
            text = text + "\r\nmouse,mc,1920,1080\t鼠标中键 (1920, 1080) 的地方";
            text = text + "\r\nmouse,w,1920,1080,120\t鼠标滚轮向上滚动120";
            text = text + "\r\nmouse,c,1920,1080,1000\t鼠标点击 (1920, 1080) 的地方, 持续 1 秒";
            text = text + "\r\nkeycode,123,1000\t\t键盘输入 F12 , 持续 1 秒";
            text = text + "\r\nkey,F12,2000\t\t键盘输入 F12 , 持续 2 秒";
            text = text + "\r\napp,notepad\t\t打开记事本";
            text = text + "\r\napp,notepad,D:/text.txt\t用记事本打开 \"D:/text.txt\"";
            text = text + "\r\npost,run\t\t\t对方已接收";
            text = text + "\r\npost,ok\t\t\t对方已完成";
            text = text + "\r\npost,err\t\t\t对方已失败";
            MessageBox.Show(text,"一些帮助");
            //text = "配置文档: \r\n\r\n放置在 ./Setting.ini";
            text = "配置文档: \r\n";
            text = text + "\r\nposttext=<string>\t\t默认发送内容";
            text = text + "\r\nappname=<string>\t目标进程名";
            text = text + "\r\ndelay=<int>\t\t定时器速度";
            text = text + "\r\nminwindows=<bool>\t启动时最小化窗口";
            text = text + "\r\nmanualpause=<bool>\t启动时是否暂停计时";
            text = text + "\r\nip=<string>\t\t广播ip地址";
            text = text + "\r\nport=<int>\t\t指定端口";
            text = text + "\r\nmissmax=<int>\t\t允许重试的最大次数";
            text = text + "\r\n\r\n配置文档应放置在: " + Application.StartupPath + "\\Setting.ini";
            MessageBox.Show(text,"一些帮助");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help();
        }

        //这个用来闪烁图标，提醒你已经暂停
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(sss.manualpause == true)
            {
                if (sss.emptyicon233 == true)
                {
                    sss.emptyicon233 = false;
                    this.Icon = sss.numicon[0];
                    this.notifyIcon1.Icon = sss.numicon[0];
                }
                else if (sss.emptyicon233 == false && sss.manualpause == true)
                {
                    sss.emptyicon233 = true;
                    this.Icon = sss.numicon[103];
                    this.notifyIcon1.Icon = sss.numicon[103];
                }
            }
        }
    }
}
