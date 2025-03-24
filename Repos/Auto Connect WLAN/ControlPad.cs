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
    public partial class ControlPad : UserControl
    {
        public ControlPad()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form connect = new Connect();
            connect.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Auto_Connect_WLAN.Form1.让我看看.Close();
        }
    }
}
