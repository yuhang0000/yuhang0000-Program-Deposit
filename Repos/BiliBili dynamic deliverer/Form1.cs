using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiliBili_dynamic_deliverer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //按钮1 鼠标左键单击
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "BiliBili dynamic deliverer v"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
