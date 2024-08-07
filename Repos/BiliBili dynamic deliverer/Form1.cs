using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 全局变量集;
using Command;


namespace BiliBili_dynamic_deliverer
{
    public partial class Form1 : Form
    {

        //全局访问 Form1 实例
        public static Form1 让我看看 { get; private set; }

        public Form1()
        {
            InitializeComponent();
            让我看看 = this;
        }


        //设置
        private setting 设置窗口;
        private void button1_Click(object sender, EventArgs e)
        {
            if (设置窗口 == null || 设置窗口.IsHandleCreated == false)
            {
                设置窗口 = new setting(); // 创建新的实例
                设置窗口.FormClosed += (s, args) => 设置窗口 = null;
                //s 指 "设置窗口" 这个实例，args 是指 "FormClosed" 这个事件
                设置窗口.Show();
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                设置窗口.WindowState = FormWindowState.Normal;
                设置窗口.Activate();
                设置窗口.Focus();
            }
        }

        //程序加载时运行
        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = "v"+ 全局变量.版本;
            this.Text = "BiliBili dynamic deliverer "+ label4.Text;
            textBox2.Text = "##### " + this.Text + " #####";
            textBox2.Text = 指令.时间() + "尝试读取配置文档";
            if (File.Exists(@"./Setting.ini") == false)
            {
                textBox2.Text = 指令.时间() + "配置文档不存在，尝试创建新的配置文档";
                指令.写入配置文档();
                textBox2.Text = 指令.时间() + "配置文档创建完成";
                textBox2.Text = 指令.时间() + "就绪。";
            }
            else
            {
                try
                {
                    指令.读取配置文档();
                }
                catch
                { 

                }
            }
        }

        //关于
        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BiliBili dynamic deliverer\nBy：yuhang0000\nBuild Time：" + 全局变量.编译时间 + "\n版本：" + 全局变量.版本, "关于");
        }

        //checkBox4 检查是否选中
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == false)
            {
                checkBox3.Enabled = false;
                checkBox5.Enabled = false;
            }
            else
            {
                checkBox3.Enabled = true;
                checkBox5.Enabled = true;
            }
        }

        //自适应窗口大小
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.label4.Top = this.Height - 500 + 400;
            this.groupBox1.Width = this.Width - 1130 + 721;
            this.groupBox1.Height = this.Height - 500 + 421; 
        }

        //抓取信息
        private output 输出窗口;
        private void button8_Click(object sender, EventArgs e)
        {
            if (输出窗口 == null || 输出窗口.IsHandleCreated == false)
            {
                输出窗口 = new output(); // 创建新的实例
                输出窗口.FormClosed += (s, args) => 输出窗口 = null;
                输出窗口.Show();
            }
            else
            {
                输出窗口.WindowState = FormWindowState.Normal;
                输出窗口.Activate();
                输出窗口.Focus();
            }
        }
    }
}
