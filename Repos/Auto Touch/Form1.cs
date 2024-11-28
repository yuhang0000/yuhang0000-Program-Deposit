using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Touch
{
    public partial class Form1 : Form
    {
        //全局访问 Form1 实例
        public static Form1 让我看看 { get; private set; }

        //启动参数
        public Form1(String[] args = null)
        {
            if (args != null && args.Length > 0)
            {
                String command = "";
                foreach (var item in args)
                {
                    command = command + " " + item.ToString();
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
                    MessageBox.Show("参数错误: \r\n\"" + name + command + "\"", "Oops! ");
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
                        int time = 0;
                        if (args.Length > 2)
                        {
                            time = int.Parse(args[2]);
                        }
                        //Console.WriteLine(args.ToString());
                        run(time);
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
        //计时器相关的
        [DllImport("winmm")]
        static extern uint timeGetTime();

        public static class 全局变量
        {
            public static String 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            public static DateTime 编译时间 = System.IO.File.GetLastWriteTime(typeof(全局变量).Assembly.Location);
            public static int XXX = 0;
            public static int YYY = 0;
            public static int 状态 = 1;
            public static String[] ini = new String[0];
            //public static String[] proj = { };
            public static bool 闪 = false;
            public static int comboboxindex = 0;
        }

        //读取配置文档
        public void readini()
        {
            if (File.Exists(@"./Auto Touch.ini") == true)
            {
                try
                {
                    全局变量.ini = File.ReadAllLines(@"./Auto Touch.ini");
                    //Console.WriteLine(全局变量.ini[0]);
                    this.textBox1.Text = 全局变量.ini[全局变量.ini.Length - 1].Split(';')[1];
                    foreach (String inis in 全局变量.ini)
                    {
                        string[] ini = inis.Split(';');
                        //全局变量.proj.Append(ini[0]);
                        this.comboBox1.Items.Add(ini[0]);
                        //this.comboBox1.SelectedIndex = 1;
                        //this.textBox1.Text = ini[1];
                        //this.textBox2.Text = ini[2];
                    }
                }
                catch (Exception ex)
                {
                    全局变量.ini = new String[0];
                    Console.WriteLine("读取失败: " + ex.Message);
                }
            }
        }

        public void help()
        {
            MessageBox.Show("Auto Touch.exe : \r\n\r\n\thelp\t\t获取帮助。\r\n\tver\t\t检查版本信息。\r\n\t" +
                "X[int] Y[int]\t点击 (X,Y) 坐标位置。\r\n\tTime[int]\t\t延时运行, 单位 ms 。\r\n\r\n示例: " +
                "\tAuto Touch.exe 1920 1080 1000\r\n\t( 等待1秒后, 点击 (1920,1080) 所处位置。)", "帮助: ");
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
            this.Width = 312;
            readini();
        }

        async public void run(int time = 0)
        {
            time = (int)timeGetTime() + time;
            while (time > (int)timeGetTime()) {
                //time = time - 1000;
                if (全局变量.状态 == 2)
                {
                    this.toolStripStatusLabel5.Text = (time - (int)timeGetTime()).ToString() + "ms";
                    await Task.Delay(10);
                }
                else if (全局变量.状态 == 1)
                {
                    Thread.Sleep(10);
                }
                if (全局变量.状态 == 0)
                {
                    try
                    {
                        this.button3.Enabled = true;
                        this.button3.Focus();
                        this.textBox1.Enabled = true;
                        //SelectNextControl(ActiveControl, false, true, true, true);
                        //SelectNextControl(ActiveControl, false, true, true, true);
                        this.button2.Enabled = true;
                        this.button3.Text = "开始";
                    }
                    catch { }
                    return;
                }
            }
            System.Drawing.Point mp = new System.Drawing.Point();
            GetCursorPos(out mp);
            int XX = mp.X;
            int YY = mp.Y;
            SetCursorPos(全局变量.XXX, 全局变量.YYY);
            mouse_event(0x0002 | 0x0004, 0, 0, 0, 0);
            SetCursorPos(XX, YY);
            System.Environment.Exit(0);
        }

        //捕捉光标位置
        private void button2_Click(object sender, EventArgs e)
        {
            this.button3.Focus();
            this.WindowState = FormWindowState.Minimized;
            捕捉 pos = new 捕捉();
            pos.Show();
            pos.Activate();
            pos.Focus();
            //SelectNextControl(ActiveControl, true, true, true, true);
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
            if (this.button3.Text == "开始")
            {
                this.button3.Text = "停止";
                this.button2.Enabled = false;
                this.textBox1.Enabled = false;
                全局变量.状态 = 2;
                String xy = textBox1.Text;
                try
                {
                    xy = xy.Substring(0, xy.Length - 1);
                    xy = xy.Substring(1, xy.Length - 1);
                    全局变量.XXX = int.Parse(xy.Substring(0, xy.IndexOf(",")));
                    全局变量.YYY = int.Parse(xy.Substring(xy.IndexOf(",") + 1));
                    Console.WriteLine(全局变量.XXX + "," + 全局变量.YYY);
                    //给定时间用这个
                    if (this.textBox2.Text.IndexOf(":") > -1)
                    {

                    }
                    //倒计时用这个
                    else
                    {
                        run(int.Parse(this.textBox2.Text));
                    }
                }
                catch
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("输入有误: \r\n" + textBox1.Text, "Oops!");
                    全局变量.状态 = 0;
                    this.button2.Enabled = true;
                    this.textBox1.Enabled = true;
                    this.button3.Text = "开始";
                }
            }
            else
            {
                全局变量.状态 = 0;
                this.button3.Enabled = false;
                //this.button3.Text = "开始";
            }
        }

        //文本纠正, 为了防呆而防呆, 不要键入中文标点符号!!!
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int aaa = textBox1.SelectionStart;
            if (textBox1.Text.IndexOf(" ") > 0)
            {
                aaa--;
            }
            textBox1.Text = textBox1.Text.Replace(" ", "");
            textBox1.Text = textBox1.Text.Replace("（", "(");
            textBox1.Text = textBox1.Text.Replace("）", ")");
            textBox1.Text = textBox1.Text.Replace("，", ",");
            textBox1.SelectionStart = aaa;
        }

        //Exit
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //高级选项
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.Width < 444)
            {
                this.Width = 588;
                this.toolStripStatusLabel4.Visible = true;
                this.toolStripStatusLabel5.Visible = true;
                this.comboBox1.Enabled = true;
                this.textBox2.Enabled = true;
                this.button6.Enabled = true;
                if(this.comboBox1.SelectedIndex != 0)
                {
                    this.button7.Enabled = true;
                }
                //this.button8.Enabled = true;
                this.button4.TabStop = true;
            }
            else
            {
                this.Width = 312;
                this.toolStripStatusLabel4.Visible = false;
                this.toolStripStatusLabel5.Visible = false;
                this.comboBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.button6.Enabled = false;
                this.button7.Enabled = false;
                //this.button8.Enabled = false;
                this.button4.TabStop = false;
            }
        }

        //时间，仅允许数字，和 ":"
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //我的天呐，不能用双引号
            if (e.KeyChar != '\b' && e.KeyChar != ':' && Char.IsDigit(e.KeyChar) == false)
            {
                e.Handled = true;
                SystemSounds.Beep.Play();
            }
        }

        //重命名
        public void rename(String text = null)
        {
            if(text == "新建" || text == "" || text == " " || this.textBox1.Text == "" || 
                this.textBox1.Text == " " || this.textBox2.Text == "" || this.textBox2.Text == " ")
            {
                return;
            }
            //if (this.comboBox1.SelectedIndex >= 0 && 全局变量.comboboxindex > 0)
            if (全局变量.comboboxindex > 0)
            {
                String[] ini = 全局变量.ini[全局变量.comboboxindex - 1].Split(';');
                ini[0] = this.comboBox1.Text;
                ini[1] = this.textBox1.Text;
                ini[2] = this.textBox2.Text;
                全局变量.ini[全局变量.comboboxindex - 1] = String.Join(";", ini);
                this.comboBox1.Items[全局变量.comboboxindex] = this.comboBox1.Text;
                if (text == null)
                {
                    this.comboBox1.SelectedIndex = 全局变量.comboboxindex;
                    this.comboBox1.Text = ini[0];
                }
            }
        }

        //选择框被更改
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(comboBox1.Text);
            全局变量.comboboxindex = comboBox1.SelectedIndex;
            if (comboBox1.SelectedIndex <= 0)
            {
                this.button6.Text = "保存";
                this.button7.Enabled = false;
                return;
            }
            else
            {
                this.button7.Enabled = true;
                this.button6.Text = "新建";
                String[] ini = 全局变量.ini[this.comboBox1.SelectedIndex - 1].Split(';');
                this.comboBox1.Text = ini[0];
                this.textBox1.Text = ini[1];
                this.textBox2.Text = ini[2];
                //rename();
            }
        }

        async public void 闪烁(Control aaa)
        {
            SystemSounds.Beep.Play();
            int top = aaa.Top;
            aaa.Focus();
            //SelectNextControl(ActiveControl, false, true, true, true);
            if (全局变量.闪 == false)
            {
                全局变量.闪 = true;
                aaa.Top = 65536;
                await Task.Delay(100);
                aaa.Top = top;
                await Task.Delay(100);
                aaa.Top = 65536;
                await Task.Delay(100);
                aaa.Top = top;
                await Task.Delay(100);
                aaa.Top = 65536;
                await Task.Delay(100);
                aaa.Top = top;
                await Task.Delay(100);
                aaa.Top = 65536;
                await Task.Delay(100);
                aaa.Top = top;
                await Task.Delay(100);
                aaa.Top = 65536;
                await Task.Delay(100);
                aaa.Top = top;
                await Task.Delay(100);
                全局变量.闪 = false;
            }
        }

        //保存 & 新建
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.button6.Text == "新建")
            {
                rename();
                this.comboBox1.Focus();
                this.comboBox1.SelectedIndex = 0;
                this.comboBox1.SelectAll();
            }
            else
            {
                if(this.comboBox1.Text == "新建" || this.comboBox1.Text == "" || this.comboBox1.Text == " ")
                {
                    闪烁(this.comboBox1);
                    return;
                }
                if(this.textBox1.Text == "" || this.textBox1.Text == " ")
                {
                    闪烁(this.textBox1); 
                    return;
                }
                if(this.textBox2.Text == "" || this.textBox2.Text == " ")
                {
                    闪烁(this.textBox2); 
                    return;
                }
                Array.Resize(ref 全局变量.ini,全局变量.ini.Length + 1);
                全局变量.ini[全局变量.ini.Length - 1] = this.comboBox1.Text + ";" + this.textBox1.Text + ";" + 
                    this.textBox2.Text;
                this.comboBox1.Items.Add(this.comboBox1.Text);
                this.comboBox1.SelectedIndex = 全局变量.ini.Length;
            }
        }

        //不要在文本框输入 ";"
        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            if(this.comboBox1.Text.IndexOf(";") != -1)
            {
                int aaa = this.comboBox1.SelectionStart;
                SystemSounds.Beep.Play();
                this.comboBox1.Text = this.comboBox1.Text.Replace(";","") ;
                this.comboBox1.SelectionStart = aaa - 1;
            }
        }

        //关闭前先保存
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                rename();
                File.WriteAllLines(@"./Auto Touch.ini", 全局变量.ini);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败哩: \r\n" + ex.Message, "(＃°Д°) !!!  失败");
            }
        }

        //删除
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                return;
            }
            if(MessageBox.Show("欸，要删除 " + 全局变量.ini[this.comboBox1.SelectedIndex - 1].Split(';')[0] +
                " 吗? ", "删除",MessageBoxButtons.OKCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)
                == DialogResult.OK){
                int index = 全局变量.comboboxindex;
                if(index == 1)
                {
                    this.button2.Focus();
                }
                this.comboBox1.SelectedIndex = 全局变量.comboboxindex - 1;
                this.comboBox1.Items.RemoveAt(index);
                string[] ini = { };
                Array.Resize(ref ini, 全局变量.ini.Length - 1);
                int a = 0;
                int b = 0;
                while(a < 全局变量.ini.Length)
                {
                    if(a != (index - 1))
                    {
                        ini[b] = 全局变量.ini[a];
                        b++;
                    }
                    a++;
                }
                全局变量.ini = ini;
            }
        }

        //失去焦点来重命名
        private void comboBox1_Leave(object sender, EventArgs e)
        {
            rename();
        }

        //改变选项来重命名
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            rename(this.comboBox1.Text);
        }
    }
}
