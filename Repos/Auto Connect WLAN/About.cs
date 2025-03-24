using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            else if(content != "\0")    //其实是要反着来的
            {
                this.Text = content;
                this.label1.Text = title;
            }
            else    //这里写版本号
            {

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
