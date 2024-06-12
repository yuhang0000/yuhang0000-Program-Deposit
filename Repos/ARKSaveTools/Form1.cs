using Command_list;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Command_list.Command;
using static System.Net.Mime.MediaTypeNames;

namespace ARKSaveTools
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

        //选择指定文件夹路径
        private void OpenFilePath(object sender, EventArgs e)
        {
            //获取该按钮Name
            Button button = (Button)sender;
            //Console.WriteLine(button.Name.Substring(6));
            string Path = (Controls.Find("TextBox" + button.Name.Substring(6), false)[0]).Text;
            //打开对话框，并返回指定路径
            (Controls.Find("TextBox" + button.Name.Substring(6), false)[0]).Text = Command_list.Command.选择文件夹位置(Path);
        }

        //程序刚打开就运行的地方
        private void Form1_Load(object sender, EventArgs e)
        {
            //让它自己打印版本号
            this.Text = "逆天的方舟生存存档管理工具 (ARKSaveTools) v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //读取配置文档
            if (File.Exists("./Setting.ini") == false)
            {
                Console.WriteLine("配置文档不存在！！！");
                File.WriteAllText("./Setting.ini", "版本号="+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\r\n" + "存档路径=" + textBox1.Text + "\r\n" + "救档路径=" + textBox2.Text + "\r\n" + "备份1=" + textBox3.Text + "\r\n" + "备份2=" + textBox4.Text + "\r\n" + "备份3=" + textBox5.Text + "\r\n\n[By:yuhang0000]");
            }
        }

        //备份存档
        private void button6_Click(object sender, EventArgs e)
        {
            Command.俺想要UI更新 = text => toolStripStatusLabel2.Text = text;
            // 开始遍历
            全局变量.文件数量 = 0;
            //目标文件夹
            toolStripStatusLabel2.Text = "开始备份";
            string ToPath;
            全局变量.当前操作是啥 = "备份";
            if (comboBox1.Text == "备份1")
            {
                ToPath = textBox3.Text;
            }
            else if (comboBox1.Text == "备份2")
            {
                ToPath = textBox4.Text;
            }
            else if (comboBox1.Text == "备份3")
            {
                ToPath = textBox5.Text;
            }
            else if (comboBox1.Text == "救档")
            {
                ToPath = textBox2.Text;
            }
            else
            {
                MessageBox.Show("OMG\r\n我找不到这个存档欸: \r\n" + comboBox1.Text, "错误");
                return;
            }
            Command.遍历文件(this.textBox1.Text, ToPath);
        }

        //还原存档
        private void button7_Click(object sender, EventArgs e)
        {
            Command.俺想要UI更新 = text => toolStripStatusLabel2.Text = text;
            // 开始遍历
            全局变量.文件数量 = 0;
            //目标文件夹
            toolStripStatusLabel2.Text = "开始恢复";
            string ToPath;
            全局变量.当前操作是啥 = "恢复";
            if (comboBox1.Text == "备份1")
            {
                ToPath = textBox3.Text;
            }
            else if (comboBox1.Text == "备份2")
            {
                ToPath = textBox4.Text;
            }
            else if (comboBox1.Text == "备份3")
            {
                ToPath = textBox5.Text;
            }
            else if (comboBox1.Text == "救档")
            {
                ToPath = textBox2.Text;
            }
            else
            {
                MessageBox.Show("OMG\r\n我找不到这个存档欸: \r\n" + comboBox1.Text, "错误");
                return;
            }
            Command.遍历文件(ToPath , this.textBox1.Text);
        }

        //窗口自适应大小
        private void statusStrip1_Resize(object sender, EventArgs e)
        {
            textBox1.Width = 677 + (this.Width - 838);
            button1.Left = 767 + (this.Width - 838);
            textBox2.Width = 677 + (this.Width - 838);
            button2.Left = 767 + (this.Width - 838);
            textBox3.Width = 677 + (this.Width - 838);
            button3.Left = 767 + (this.Width - 838);
            textBox4.Width = 677 + (this.Width - 838);
            button4.Left = 767 + (this.Width - 838);
            textBox5.Width = 677 + (this.Width - 838);
            button5.Left = 767 + (this.Width - 838);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("逆天的方舟生存存档管理工具。\nBy：yuhang0000\nBuild Time：" + 全局变量.Buildtime + "\n版本号：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), "关于");
        }
    }
}
