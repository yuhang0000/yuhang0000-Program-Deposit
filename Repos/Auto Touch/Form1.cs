using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Touch
{
    public partial class Form1 : Form
    {
        //全局访问 Form1 实例
        public static Form1 让我看看 { get; private set; }

        public Form1(String[] args = null)
        {
            if (args != null && args.Length > 0)
            {
                String command = "";
                foreach (var item in args)
                {
                    command = command + item.ToString();
                }

                //OMG，居然可以在方法里再嵌套个新方法
                void err()
                {
                    String name = "Auto Touch.exe";
                    try
                    {
                        name = Application.ExecutablePath;
                        name = name.Replace(Application.StartupPath + "\\", "");
                    }
                    catch
                    {
                        name = "Auto Touch.exe";
                    }
                    SystemSounds.Hand.Play();
                    MessageBox.Show("参数错误: \r\n\"" + name + " " + command + "\"", "Oops! ");
                    help();
                }

                try
                {
                    if (args[0] == "ver" || args[0] == "version")
                    {
                        about();
                    }
                    else if (args[0] == "help" || args[0] == "/?" || args[0] == "-h")
                    {
                        help();
                    }
                    else
                    {
                        全局变量.XXX = int.Parse(args[0]);
                        全局变量.YYY = int.Parse(args[1]);
                        run();
                    }
                    System.Environment.Exit(0);
                }
                catch
                {
                    err();
                    System.Environment.Exit(0);
                }
            }
            else
            {
                //help() ;
            }
            InitializeComponent();
            让我看看 = this;
        }

        [DllImport("user32")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32")]
        public static extern int GetCursorPos(out System.Drawing.Point lpPoint);
        [DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static class 全局变量
        {
            public static String 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            public static DateTime 编译时间 = System.IO.File.GetLastWriteTime(typeof(全局变量).Assembly.Location);
            public static int XXX = 0;
            public static int YYY = 0;
        }

        public void help()
        {
            MessageBox.Show("Auto Touch.exe : \r\n\r\n\thelp\t\t获取帮助。\r\n\tver\t\t检查版本信息。\r\n\t" +
                "X[int] Y[int]\t点击 (X,Y) 坐标位置。", "帮助: ");
        }

        public void about()
        {
            MessageBox.Show("Auto Touch\r\nBy: yuhang0000\r\nBuild Time: " + 全局变量.编译时间
                + "\r\n版本: " + 全局变量.版本, "关于: ");
        }

        //一打开就运行
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "v"+全局变量.版本;
            toolStripStatusLabel3.Text = 全局变量.编译时间.ToShortDateString().ToString();
        }

        public void run()
        {
            System.Drawing.Point mp = new System.Drawing.Point();
            GetCursorPos(out mp);
            int XX = mp.X; 
            int YY = mp.Y;
            SetCursorPos(全局变量.XXX, 全局变量.YYY);
            mouse_event(0x0002 | 0x0004, 0,0,0,0);
            SetCursorPos(XX, YY);
            System.Environment.Exit(0);
        }

        //捕捉光标位置
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            捕捉 pos = new 捕捉();
            pos.Show();
            pos.Activate();
            pos.Focus();
            SelectNextControl(ActiveControl, true, true, true, true);
        }

        //关鱼
        private void statusStrip1_Click(object sender, EventArgs e)
        {
            about();
        }

        //帮助
        private void button1_Click(object sender, EventArgs e)
        {
            help();
        }

        //开始运行
        private void button3_Click(object sender, EventArgs e)
        {
            String xy = textBox1.Text;
            try
            {
                xy = xy.Substring(0, xy.Length - 1);
                xy = xy.Substring(1, xy.Length - 1);
                全局变量.XXX = int.Parse( xy.Substring(0,xy.IndexOf(",")) );
                全局变量.YYY = int.Parse( xy.Substring(xy.IndexOf(",") + 1) );
                Console.WriteLine(全局变量.XXX + "," + 全局变量.YYY);
                run();
            }
            catch
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("输入有误: \r\n" + textBox1.Text ,"Oops!");
            }
        }

        //文本纠正, 为了防呆而防呆, 不要键入中文标点符号!!!
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int aaa = textBox1.SelectionStart;
            if(textBox1.Text.IndexOf(" ") > 0)
            {
                aaa--;
            }
            textBox1.Text = textBox1.Text.Replace(" ","");
            textBox1.Text = textBox1.Text.Replace("（", "(");
            textBox1.Text = textBox1.Text.Replace("）",")");
            textBox1.Text = textBox1.Text.Replace("，",",");
            textBox1.SelectionStart = aaa;
        }

        //Exit
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
