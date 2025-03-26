using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Connect_WLAN
{
    public partial class About : Form
    {
        public About(String content = "\0" , String title = "\0")
        {
            InitializeComponent();
            if(title == "\0" && content != "\0")
            {
                this.Text = "";
                this.label1.Text = content;
            }
            else if(content != "\0") 
            {
                this.Text = title;
                this.label1.Text = content;
            }
            else    //这里写版本号
            {
                String 康派利 = Application.CompanyName;
                String 奈姆 = Assembly.GetExecutingAssembly().GetName().Name;
                String 版本 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                DateTime 编译时间 = File.GetLastWriteTime(typeof(About).Assembly.Location);
                this.Text = "关于";
                this.label1.Text = 奈姆 + "\r\nBy: " + 康派利 +"\r\nBuild Time: " + 编译时间 + "\r\n版本号: " + 版本;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.Width = 243 - (168 - this.label1.Width);
            this.Height = 262 - (80 - this.label1.Height);
        }
    }
}
