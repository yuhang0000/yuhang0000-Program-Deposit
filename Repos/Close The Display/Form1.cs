using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Close_The_Display
{
    public partial class Form1 : Form
    {

        protected override void WndProc(ref Message m)
        {
            //状态机
            //DateTime now = DateTime.Now;
            //Console.WriteLine(now + "| " + m.Msg.ToString());
            //this.textBox1.AppendText(m.Msg + "\r\n");
            switch (m.Msg)
            {
                case 0x218:
                    Console.WriteLine("接收消息: " + m.WParam.ToString());
                    if(m.WParam.ToInt32() == 0x8013) //已收到电源设置更改事件。
                    {
                        /*int size = Marshal.SizeOf(m.LParam); //获取结构体大小
                        IntPtr freesize = Marshal.AllocHGlobal(size); //分配相等的内存池
                        Marshal.StructureToPtr(m.LParam,freesize,false); //ctrl + C/V
                        byte[] buffer = new byte[size];
                        Marshal.Copy(freesize,buffer,0,size); //新内存池，比特数组，不知道是啥，大小
                        Marshal.FreeHGlobal(freesize);
                        String buffers = "";
                        foreach(byte b in buffer)
                        {
                            buffers = buffers + b.ToString() + " ";
                        }
                        Console.WriteLine(buffers);
                        if (buffer[0].ToString() == "32")
                        {
                            Process.Start("Rundll32.exe user32.dll,LockWorkStation");
                        }*/
                        MyStruct myStruct = (MyStruct)Marshal.PtrToStructure(m.LParam,typeof(MyStruct));
                        Guid guid = Guid.Parse("BA3E0F4D-B817-4094-A2D1-D56379E6A0F3");
                        if (myStruct.PowerSetting == guid)
                        {
                            if (sss.wait == 1)
                            {
                                sss.wait = 0;
                            }
                            else
                            {
                                Process.Start("Rundll32.exe", "user32.dll,LockWorkStation");
                            }
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        //创建结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct MyStruct
        {
            public Guid PowerSetting;
            public uint DataLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] //固定data数量为 1 个
            public byte[] Data;
        }

        [DllImport("user32.dll", SetLastError = true)] //注册
        public static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient,
            [MarshalAs(UnmanagedType.LPStruct)] Guid PowerSettingGuid, uint Flags);
        [DllImport("user32.dll", SetLastError = true)] //注销
        public static extern void UnregisterPowerSettingNotification(IntPtr Handle);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public static class sss
        {
            public static int wait = 1;
            public static IntPtr reback;
        }

        public Form1()
        {
            InitializeComponent();
            Guid guid = Guid.Parse("BA3E0F4D-B817-4094-A2D1-D56379E6A0F3");
            try
            {
                sss.reback = RegisterPowerSettingNotification(this.Handle, guid, 0);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message,"Oops! ");
                UnregisterPowerSettingNotification(sss.reback);
            }
            int a = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, a | 0x00000080); //在 alt + Tab 上颖仓
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterPowerSettingNotification(sss.reback);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sss.wait = 0;
            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
