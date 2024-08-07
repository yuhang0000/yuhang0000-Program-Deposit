using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiliBili_dynamic_deliverer
{
    public partial class setting : Form
    {

        public setting()
        {
            InitializeComponent();
        }

        //一打开就运行
        private void setting_Load(object sender, EventArgs e)
        {
        }

        //一关闭就运行
        private void setting_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        //取消
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //确定
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //打开配置文档
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad", Application.StartupPath + @"\Setting.ini");
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show(ex.Message.ToString() + "\r\n" + "notepad \"" + Application.StartupPath + "\\Setting.ini\"","错误    Σ(っ °Д °;)っ");
            }
        }
    }
}
