using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHttpClient
{
    public partial class Form1 : Form
    {
        public int num = 100000;
        public int max = 100;
        public string urlleft = "";
        public string urlright = "";
        public bool retry = true;
        public List<double> list = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        async Task runs(string url)
        {
            try
            {
                int lll = await http(url);
                num++;
                update(gettext() + "\t成功\t" + lll);

            }
            catch {
                if (retry == true)
                {
                    await runs(url);
                }
                else
                {
                    num++;
                    update(gettext() + "\t失败\t" + "0");
                }
                return;
            }
        }

        async void run()
        {
            DateTime start = DateTime.Now;

            //string temp1 = urlleft + num.ToString() + urlright;
            //int tab = temp1.Length / 8 / 2;

            update("开始于: " + start.ToString() + "\tURL\t\t\t\t\t\t状态\t长度\t时间");
            for (int i = 0; i < max; i++)
            {
                DateTime ss = DateTime.Now;
                string url = urlleft + num + urlright;
                update(gettext() + "\r\n" + url);
                await runs(url);
                DateTime ee = DateTime.Now;
                TimeSpan ssee = ee - ss;
                update(gettext() + "\t" + ssee.TotalSeconds + "s");
                list.Add(ssee.TotalSeconds);

                title(i);
            }
            DateTime end = DateTime.Now;
            update(gettext() + "\r\n开始于: " + start.ToString() + "\t结束于: " + end.ToString());

            TimeSpan ts = end - start;
            update(gettext() + "\t总花费: " + ts.TotalSeconds + "s");

            update(gettext() + "\r\n平均: " + list.Average().ToString() + "s");

            update(gettext() + "\t\t最大: " + list.Max().ToString() + "s\t\t\t最小: " + list.Min().ToString() + "s");

            if (this.IsHandleCreated == false)
            {
                this.button2.Visible = true;
            }
            else
            {
                this.Invoke(new MethodInvoker( () =>
                {
                    this.button2.Visible = true;
                }));
            }
        }

        public void update(string ttt)
        {
            if (this.IsHandleCreated == false)
            {
                this.textBox1.Text = ttt;
                this.textBox1.SelectionStart = this.textBox1.TextLength;
                this.textBox1.ScrollToCaret();
            }
            else
            {
                this.Invoke(new MethodInvoker( () =>
                {
                    this.textBox1.Text = ttt;
                    this.textBox1.SelectionStart = this.textBox1.TextLength;
                    this.textBox1.ScrollToCaret();
                }));
            }
        }

        public void title(int tt)
        {
            int ttt = tt + 1;
            if (this.IsHandleCreated == false)
            {
                this.Text = "TestHttpClient (" + ttt.ToString() + "\\" + max.ToString() + ")";
            }
            else
            {
                this.Invoke(new MethodInvoker( () =>
                {
                    this.Text = "TestHttpClient (" + ttt.ToString() + "\\" + max.ToString() + ")";
                }));
            }
        }

        public string gettext()
        {
            string ttt = "";
            if (this.IsHandleCreated == false)
            {
                return this.textBox1.Text;
            }
            else
            {
                this.Invoke(new MethodInvoker( () =>
                {
                    ttt = this.textBox1.Text;
                }));
                return ttt;
            }
        }

        async Task<int> http(string url)
        {
            HttpClient client = new HttpClient();
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();
            string data = await res.Content.ReadAsStringAsync();
            client.Dispose();
            res.Dispose();
            return data.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Visible = true;
            this.textBox2.Visible = false;
            this.textBox3.Visible = false;
            this.textBox4.Visible = false;
            this.textBox5.Visible = false;
            this.button1.Visible = false;
            this.checkBox1.Visible = false;

            retry = this.checkBox1.Checked;
            urlleft = this.textBox2.Text;
            urlright = this.textBox3.Text;
            num = int.Parse(this.textBox4.Text);
            max = int.Parse(this.textBox5.Text);
            this.Text = "TextHttpClient " + "(0\\" + max + ")";
            run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            list.Clear();
            this.textBox1.Text = "";
            this.textBox1.Visible = false;
            this.textBox2.Visible = true;
            this.textBox3.Visible = true;
            this.textBox4.Visible = true;
            this.textBox5.Visible = true;
            this.button1.Visible = true;
            this.button2.Visible = false;
            this.checkBox1.Visible = true;
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.textBox1.Focus();
                button1_Click(null,null);
            }
        }
    }
}
