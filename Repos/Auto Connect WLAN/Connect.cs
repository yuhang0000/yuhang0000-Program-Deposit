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
    public partial class Connect : Form
    {
        public Connect()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.textBox1.PasswordChar = '\0';
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.textBox1.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Connect_Load(object sender, EventArgs e)
        {
            this.label2.Text = Auto_Connect_WLAN.Form1.让我看看.listView1.SelectedItems[0].SubItems[3].Text;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
