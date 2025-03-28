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
            //Console.WriteLine(Auto_Connect_WLAN.Form1.让我看看.listView1.SelectedItems[0].SubItems[3].Text);
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

        private void button7_Click(object sender, EventArgs e)
        {
            //ListViewItem list = new ListViewItem();
            //list.Text = "111";
            //list.SubItems.Add("222");
            //list.SubItems.Add("333");
            //Auto_Connect_WLAN.Form1.让我看看.listView1.Items.Add;
            Auto_Connect_WLAN.Form1.让我看看.sreach();
        }
    }
}
