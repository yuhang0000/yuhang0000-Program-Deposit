using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_run_power_saver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
        //鼠标监听
        [System.Runtime.InteropServices.DllImport("user32.dll")] //导入user32.dll函数库
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);//获取鼠标坐标

        public class mouse2333
        {
            public static int mpX1;
            public static int mpY1;
        }
        */
        public class freetimesss
        {
            public static int aaa;
        }


        //导入dll文件 键盘监听
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINOUTINFO
        {
            //设置结构体块容量
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            //抓获时间
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINOUTINFO Plii);//获取鼠标和键盘的空闲时间
        public static long GetLastInputTime()
        {
            LASTINOUTINFO vLastInputinfo = new LASTINOUTINFO();
            vLastInputinfo.cbSize = Marshal.SizeOf(vLastInputinfo);
            if (!GetLastInputInfo(ref vLastInputinfo))
            {
                return 0;
            }
            else
            {
                long count = Environment.TickCount - (long)vLastInputinfo.dwTime;
                //刷新周期
                long icount = count / 50;
                return icount;
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            //打开自动运行
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/c power cfg /s 4804a71f-bacf-49e7-94c5-8d106f6b6796";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();

            /*
            //一上来就给你获取初始鼠标坐标
            System.Drawing.Point mp = new System.Drawing.Point();
            GetCursorPos(out mp);
            mouse2333.mpX1 = mp.X;
            mouse2333.mpY1 = mp.Y;
            */

            //我太懒了，版本号自己打印
            toolStripStatusLabel2.Text = "v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        //循环检测
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            /*
            //检测鼠标移动
            System.Drawing.Point mp = new System.Drawing.Point();
            GetCursorPos(out mp);
            int mousex = mp.X;  //鼠标当前X坐标
            int mousey = mp.Y;  //鼠标当前Y坐标
            //Console.WriteLine(mp.X + "xxx" + mp.Y);
            if (mp.X != mouse2333.mpX1 && mp.Y != mouse2333.mpY1)
            {
                //Console.WriteLine("我TM要886");
                //System.Environment.Exit(0);
            }
            mouse2333.mpX1 = mousex;
            mouse2333.mpY1 = mousey;
            */

            //检测键盘事件
            /*
            if (GetAsyncKeyState(13) !=0)
            {
                MessageBox.Show("你按下了回车");
            }
            */

            //在这里！！！！！！！
            double freeTime = GetLastInputTime();
            /*
            if (freeTime == 0 && freetimesss.aaa == 0)
            {
                freetimesss.aaa = 1;
            }
            else if(freeTime == 0)
            {
                System.Environment.Exit(0);
            }
            Console.WriteLine(GetLastInputTime());
            */
            if (freeTime == 0)
            {
                System.Environment.Exit(0);
            }
            Console.WriteLine(GetLastInputTime());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Environment.Exit(0);
        }

    }
}