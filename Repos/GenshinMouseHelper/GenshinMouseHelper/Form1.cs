using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GenshinMouseHelper.Commands.DLL;

namespace GenshinMouseHelper
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Form1()
        {
            this.hookproc = LLMouseProc;

            InitializeComponent();
        }

        /// <summary>
        /// 暂存数字
        /// </summary>
        public int charindex = 1;
        /// <summary>
        /// 我能输入吗?
        /// </summary>
        public bool CanIInput = true;
        /// <summary>
        /// 是否运行
        /// </summary>
        public bool IsRun = false;
        /// <summary>
        /// 自动运行
        /// </summary>
        public bool AutoRun = false;

        public IntPtr LLMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0) //不建议处理 <0 的事件, 会出问题
            {
                //把数据赋值给结构体
                tagMSLLHOOKSTRUCT tag = Marshal.PtrToStructure<tagMSLLHOOKSTRUCT>(lParam);
                short wheel = 0;
                //如果响应的是滚轮事件
                if ((int)wParam == WM_Mouse.WM_MOUSEWHEEL) //WM_Mouse.WM_MOUSEWHEEL = 0x020A
                {
                    wheel = (short)(tag.mouseData >> 16); //数据在 HIWORD, 即左半, 得把左半的字节搬到右半覆盖掉, 使用 short 保留符号
                }
                string button = "";
                //判断按下的是什么按键
                switch ((int)wParam)
                {
                    case 0x020A: //滚轮
                        button = "Wheel";
                        break;
                    case 0x020B: //侧键按下
                        short xbottondown = (short)((int)tag.mouseData >> 16);
                        if (xbottondown == 0x0001)
                        {
                            button = "MouseXBotton1";
                        }
                        else if (xbottondown == 0x0002)
                        {
                            button = "MouseXBotton2";
                        }
                        break;
                    default:
                        button = "";
                        break;
                }

                //打印
                //string text = "\tButton: " + button + "\tWheel: " + wheel;
                //Console.WriteLine(text);

                SetKey(button, wheel);
            }
            //记得处理完逻辑代码, 就得把消息传递给其他进程
            return CallNextHookEx(llmouseproc, nCode, wParam, lParam);
        }

        //把写好的回调函数, 赋值到 HookProc 这种委托类型 的变量里
        public HookProc hookproc;

        //静态保存回调函数的句柄, 不然会被 GC 吃掉
        public static IntPtr llmouseproc;

        public tagINPUT[] inputs = new tagINPUT[2];
        public int taginputsize = Marshal.SizeOf(typeof(tagINPUT));

        /// <summary>
        /// 触发键位
        /// </summary>
        public void SetKey(string button, int wheel)
        {
            //Console.WriteLine(GetForegroundProcName());

            if (button == "MouseXBotton2")
            {
                if (this.CanIInput == false)
                {
                    return;
                }
                else
                {
                    this.CanIInput = false;
                    this.timer1.Enabled = true;
                }
                
                //排除无关程序
                string procname = GetForegroundProcName().ToLower();
                if (procname != "yuanshen" && procname != "genshinimpact")
                {
                    return;
                }

                this.charindex--;
                if (this.charindex < 1)
                {
                    this.charindex = 4;
                }
                switch (this.charindex)
                {
                    case 1:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D1;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D1;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 2:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D2;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D2;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 3:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D3;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D3;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 4:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D4;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D4;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                }
                SendInput(2, this.inputs, this.taginputsize);
            }
            else if (button == "MouseXBotton1")
            {
                if (this.CanIInput == false)
                {
                    return;
                }
                else
                {
                    this.CanIInput = false;
                    this.timer1.Enabled = true;
                }

                //排除无关程序
                string procname = GetForegroundProcName().ToLower();
                if (procname != "yuanshen" && procname != "genshinimpact")
                {
                    return;
                }

                this.charindex++;
                if (this.charindex > 4)
                {
                    this.charindex = 1;
                }
                switch (this.charindex)
                {
                    case 1:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D1;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D1;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 2:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D2;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D2;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 3:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D3;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D3;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                    case 4:
                        this.inputs[0].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[0].ki.wVk = (ushort)Keys.D4;
                        this.inputs[1].type = SendInputType.INPUT_KEYBOARD;
                        this.inputs[1].ki.wVk = (ushort)Keys.D4;
                        this.inputs[1].ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;
                        break;
                }
                SendInput(2, this.inputs, this.taginputsize);
            }
        }

        //获取前台程序名称
        public string GetForegroundProcName()
        {
            string name = "";
            int PID = 0;
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd != IntPtr.Zero && GetWindowThreadProcessId(hwnd, out PID) == true)
            {
                if (PID > 0)
                {
                    Process proc = Process.GetProcessById(PID);
                    name = proc.ProcessName;
                }
            }

            return name;
        }

        //开始
        private void BtnRun_Click(object sender, EventArgs e)
        {
            this.BtnRun.Enabled = false;
            this.开启ToolStripMenuItem.Enabled = false;
            //已关闭
            if(this.IsRun == false)
            {
                llmouseproc = SetWindowsHookExA(14, this.hookproc, IntPtr.Zero, 0);
                Console.WriteLine("已启用");
                this.Text = "已启用|" + Assembly.GetExecutingAssembly().GetName().Name;
                this.BtnRun.Text = "停止";
                this.开启ToolStripMenuItem.Text = "停止";
                this.IsRun = true;
            }
            //正在运行
            else
            {
                UnhookWindowsHookEx(llmouseproc);
                Console.WriteLine("已关闭");
                this.Text = "已关闭|" + Assembly.GetExecutingAssembly().GetName().Name;
                this.BtnRun.Text = "开启";
                this.开启ToolStripMenuItem.Text = "开启";
                this.IsRun = false;
            }
            this.BtnRun.Enabled = true;
            this.开启ToolStripMenuItem.Enabled = true;
            this.BtnRun.Focus();
        }

        //退出
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.IsRun == true)
            {
                //UnhookWindowsHookEx(llmouseproc);
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                e.Cancel = true;
            }
        }

        //退出按钮
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (this.IsRun == true)
            {
                UnhookWindowsHookEx(llmouseproc);
                this.IsRun = false;
            }
            this.Close();
        }

        //加载时
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.Text = "已关闭|" + Assembly.GetExecutingAssembly().GetName().Name;

            if (this.AutoRun == true)
            {
                BtnRun_Click(null, null);
                this.WindowState = FormWindowState.Minimized;
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            if (this.AutoRun == true)
            {
                this.AutoRun = false;
                this.Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.CanIInput = true;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnExit_Click(null, null);
        }

        private void 开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnRun_Click(null, null);
        }

        private void genshinMouseHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        //系统托盘
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            genshinMouseHelperToolStripMenuItem_Click(null, null);
        }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            //this.contextMenuStrip1.Visible = true;
        }

    }
}
