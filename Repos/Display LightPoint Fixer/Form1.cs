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

namespace Display_LightPoint_Fixer
{
    public partial class win1 : Form
    {
        public win1()
        {
            InitializeComponent();
            SetWindowLong(this.Handle, GWL_EXSTYLE, WS_EX_LAYERED);
            SetLayeredWindowAttributes(this.Handle, 255, 255, LWA_COLORKEY);
            TransparencyKey = BackColor;//背景透明(鼠标穿透)
        }
        private const uint WS_EX_LAYERED = 0x80000;
        private const int GWL_EXSTYLE = -20;
        private const int LWA_COLORKEY = 1;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

        
        //程序启动时自动运行代码
        private void Form1_Load(object sender, EventArgs e)
        {

            int xWidth1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
            int yHeight1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;//高度
            //Console.WriteLine(xWidth1 + "px * " + yHeight1 + "px");
            this.Width = xWidth1;
            this.Height = yHeight1;
            this.Left = 0;
            this.Top = 0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true; this.ResumeLayout(false);
            int xWidth2 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            int yHeight2 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            this.Width = xWidth2;
            this.Height = yHeight2;
            this.ResumeLayout(false);
            OpenFileDialog open = new OpenFileDialog();
            DialogResult result = open.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(open.FileName);
            }

        }

        //按钮1 点击
        private void button1_Click(object sender, EventArgs e)
        {
            
            int xWidth1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            int yHeight1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                //public static float dpiX = dpiX;
                //public static float dpiY = dpiY;
                //Console.WriteLine(dpiX + "px * " + dpiY + "px");
                string dpiX1 = Convert.ToString(dpiX);
                label1.Text = dpiX1;
            }
            
            label1.Text = ("你这电脑的分辨率是：" + "\n" + xWidth1 + "px x " + yHeight1 + "px" + "\nDPI：" + label1.Text);

        }
        //按钮2 点击
        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        //按钮3 点击
        private void button3_Click(object sender, EventArgs e)
        {
            int xWidth1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            int yHeight1 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                //public static float dpiX = dpiX;
                //public static float dpiY = dpiY;
                //Console.WriteLine(dpiX + "px * " + dpiY + "px");
                string dpiX1 = Convert.ToString(dpiX);
                label1.Text = dpiX1;
            }

            label1.Text = ("你这电脑的分辨率是：" + "\n" + xWidth1 + "px x " + yHeight1 + "px" + "\nDPI：" + label1.Text);
            int xWidth2 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            int yHeight2 = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            this.Width = xWidth1;
            this.Height = yHeight1;
            this.Left = 0;
            this.Top = 0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true; this.ResumeLayout(false);
            //this.Width = xWidth2;
            //this.Height = yHeight2;
            //this.TransparencyKey = Color.Blue;
            //this.BackColor = Color.Blue;
            button1.Visible = false;
            button3.Visible = false;
            label1.Visible = false;
            this.ResumeLayout(false);
            button4.Enabled = true;
            //pictureBox1.Image = 
        }

        //窗口按钮按下事件
        private void win1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F4)&&(e.Alt==true))  //屏蔽ALT+F4
            {
                e.Handled = true;
            }
        }

        //按钮4 点击
        private void button4_Click(object sender, EventArgs e)
        {
            this.TransparencyKey = BackColor;//背景透明(鼠标穿透)
        }
    }
}
