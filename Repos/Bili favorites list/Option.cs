using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bili_favorites_list
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
        }

        private void Option_Load(object sender, EventArgs e)
        {
            this.checkBox1.Checked = Form1.httpheader.useproxy;
            this.textBox2.Text = Form1.httpheader.proxyurl;
            this.textBox3.Text = Form1.httpheader.proxyuser;
            this.textBox4.Text = Form1.httpheader.proxypass;
            foreach(string ua in Form1.httpheader.uas)
            {
                this.textBox1.AppendText(ua + "\r\n");
            }
        }

        private void Option_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.checkBox1.Checked == true)
                {
                    new Uri(this.textBox2.Text);
                }
            }
            catch(Exception ex)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(ex.Message,"Oops! ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            Form1.httpheader.useproxy = this.checkBox1.Checked;
            Form1.httpheader.proxyurl = this.textBox2.Text;
            Form1.httpheader.proxyuser = this.textBox3.Text;
            Form1.httpheader.proxypass = this.textBox4.Text;
            Form1.httpheader.uas = this.textBox1.Text.Trim().Split(new string[] { "\r\n" } ,StringSplitOptions.RemoveEmptyEntries);
            this.Dispose();
        }

        private void Option_Resize(object sender, EventArgs e)
        {
            this.textBox1.Width = this.Width - 71;
            this.textBox1.Height = this.Height - 218;
            this.textBox2.Width = this.Width - 145;
            this.textBox3.Width = this.textBox2.Width;
            this.textBox4.Width = this.textBox2.Width;
            this.groupBox1.Width = this.Width - 45;
        }
    }
}
