using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWindowsHook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = Application.ProductName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Run();
        }

        // 定义一个委托类型, 给 WH_MOUSE_LL 回调函数用的
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32")]
        // 设置消息钩子
        public static extern IntPtr SetWindowsHookExA(int idHook, HookProc lpfn, IntPtr hmod, int dwThreadId);

        [DllImport("User32")]
        // 移除消息钩子
        public static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("User32")]
        // 继续运行下一个钩子 (其实是把钩子消息传递给下一个程序)
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        // POINT 结构体
        public struct tagPOINT
        {
            public int X;
            public int Y;
        }

        // MSLLHOOKSTRUCT 结构体
        public struct tagMSLLHOOKSTRUCT
        {
            // 光标的 XY 坐标
            public tagPOINT pt;
            // 鼠标额外数据: 滚轮信息或者侧键状态
            public int mouseData;
            // 事件注入的标志
            public int flags;
            // 此消息的时间戳
            public int time;
            // 与消息关联的其他信息
            public uint dwExtraInfo;
        }

        /// <summary>
        /// WM_Mouse消息
        /// <para>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-lbuttondown">WM_LBUTTONDOWN消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-lbuttonup">WM_LBUTTONUP消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-mousemove">WM_MOUSEMOVE消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-mousewheel">WM_MOUSEWHEEL消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-rbuttondown">WM_RBUTTONDOWN消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-rbuttonup">WM_RBUTTONUP消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-mbuttondown">WM_MBUTTONDOWN消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-mbuttonup">WM_MBUTTONUP消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-xbuttondown">WM_XBUTTONDOWN消息</a><br/>
        /// <a href="https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-xbuttonup">WM_XBUTTONUP消息</a><br/>
        /// </para>
        /// </summary>
        public static class WM_Mouse
        {
            /// <summary>
            /// 无
            /// </summary>
            public static int NONE = 0x0000;
            /// <summary>
            /// 鼠标左键按下
            /// </summary>
            public static int WM_LBUTTONDOWN = 0x0201;
            /// <summary>
            /// 鼠标左键松开
            /// </summary>
            public static int WM_LBUTTONUP = 0x0202;
            /// <summary>
            /// 鼠标移动
            /// </summary>
            public static int WM_MOUSEMOVE = 0x0200;
            /// <summary>
            /// 鼠标滚轮
            /// </summary>
            public static int WM_MOUSEWHEEL = 0x020A;
            /// <summary>
            /// 鼠标右键按下
            /// </summary>
            public static int WM_RBUTTONDOWN = 0x0204;
            /// <summary>
            /// 鼠标右键松开
            /// </summary>
            public static int WM_RBUTTONUP = 0x0205;
            /// <summary>
            /// 鼠标中键按下
            /// </summary>
            public static int WM_MBUTTONDOWN = 0x0207;
            /// <summary>
            /// 鼠标中键放开
            /// </summary>
            public static int WM_MBUTTONUP = 0x0208;
            /// <summary>
            /// 鼠标侧键按下
            /// </summary>
            public static int WM_XBUTTONDOWN = 0x020B;
            /// <summary>
            /// 鼠标侧键松开
            /// </summary>
            public static int WM_XBUTTONUP = 0x020C;

            /// <summary>
            /// 鼠标左键关闭
            /// </summary>
            public static int MK_LBUTTON = 0x0001;
            /// <summary>
            /// 鼠标右键关闭
            /// </summary>
            public static int MK_RBUTTON = 0x0002;
            /// <summary>
            /// Shift关闭
            /// </summary>
            public static int MK_SHIFT = 0x0004;
            /// <summary>
            /// Ctrl关闭
            /// </summary>
            public static int MK_CONTROL = 0x0008;
            /// <summary>
            /// 鼠标中键关闭
            /// </summary>
            public static int MK_MBUTTON = 0x0010;
            /// <summary>
            /// 鼠标侧键1关闭
            /// </summary>
            public static int MK_XBUTTON1 = 0x0020;
            /// <summary>
            /// 鼠标侧键2关闭
            /// </summary>
            public static int MK_XBUTTON2 = 0x0040;
        }

        /// <summary>
        /// WH_MOUSE_LL 的回调函数, 真正的业务逻辑处理在这
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static IntPtr LLMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
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
                tagPOINT point = tag.pt;
                string button = "";
                //判断按下的是什么按键
                switch ((int)wParam)
                {
                    case 0x020A: //滚轮
                        button = "Wheel";
                        break;
                    case 0x020B: //侧键
                        button = "MouseXButton";
                        break;
                    case 0x0201: //左键
                        button = "MouseLeft";
                        break;
                    case 0x0204: //右键
                        button = "MouseRight";
                        break;
                    case 0x0207: //中键
                        button = "MouseMiddle";
                        break;
                    default:
                        button = "";
                        break;
                }
                string text = "X: " + point.X + "\tY: " + point.Y + "\tTime: " + tag.time + "\tButton: " + button;
                //打印
                Console.WriteLine(text);
            }
            //记得处理完逻辑代码, 就得把消息传递给其他进程
            return CallNextHookEx(llmouseproc, nCode, wParam, lParam);
        }

        //把写好的回调函数, 赋值到 HookProc 这种委托类型 的变量里
        public static HookProc hookproc = LLMouseProc;

        //静态保存回调函数的句柄, 不然会被 GC 吃掉
        public static IntPtr llmouseproc;

        //开始执行
        public static void Run()
        {
            //开始部署消息钩子, 执行这一段函数之后, 就真正开始监听鼠标事件了
            //SetWindowsHookExA: 第一个是消息类型, 第二个是 HookProc 这种委托类型的变量, 回调函数赋值在这里, 第三个和第四个正常不用管
            llmouseproc = SetWindowsHookExA(14, hookproc, IntPtr.Zero, 0); //低级鼠标钩子消息类型, 值为 14
        }

        //退出时, 记得把消息钩子注销掉
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(llmouseproc);
        }

        //退出按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
