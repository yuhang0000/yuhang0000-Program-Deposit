using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Bili_videoshotpvhdboss_get
{
    public partial class 报错 : Form
    {
        //全局访问介个窗口的实例
        public static 报错 让我看看 { get; private set; }

        public 报错()
        {
            InitializeComponent();
            让我看看 = this;
        }

        //错误弹窗关闭后
        private void 报错_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bili_videoshotpvhdboss_get.Form1.全局变量.错误弹窗的状态 = false;
            Bili_videoshotpvhdboss_get.Form1.全局变量.错误计数 = 0;
        }

        public static void 列表更新(ListView listView, String 时间, String 错误)
        {
            //错误信息筛选
            if(错误.Contains("System.Net.Http.HttpRequestException: ") == true)
            {
                错误 = 错误.Replace("System.Net.Http.HttpRequestException: ","");
            }
            if (错误.Contains("System.Net.WebException: ") == true)
            {
                错误 = 错误.Replace("System.Net.WebException: ", "");
            }
            if (错误.Contains("System.Threading.Tasks.TaskCanceledException: ") == true)
            {
                错误 = 错误.Replace("System.Threading.Tasks.TaskCanceledException: ", "");
                错误 = 错误 + "---> 大概是连接超时?";
            }
            if (错误.Contains("System.InvalidOperationException: ") == true)
            {
                错误 = 错误.Replace("System.InvalidOperationException: ", "");
            }
            if (错误.Contains("System.Net.Sockets.SocketException: ") == true)
            {
                错误 = 错误.Replace("System.Net.Sockets.SocketException: ", "");
            }

            //更新列表数据
            listView.BeginUpdate();
            ListViewItem 列表 = new ListViewItem();
            列表.Text = (Bili_videoshotpvhdboss_get.Form1.全局变量.错误计数++).ToString();
            列表.SubItems.Add(时间);
            列表.SubItems.Add(错误);
            listView.Items.Add(列表);
            //自动滚动到底部
            listView.Items[listView.Items.Count - 1].EnsureVisible();
            listView.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            列表更新(this.listView1, DateTime.Now.ToString(), "caooo");
        }

        //窗口自适应
        private void 报错_Resize(object sender, EventArgs e)
        {
            this.columnHeader3.Width = (this.Width - 1060) + 797;
            //this.listView1.Width = (this.Width - 1000) + 1042;
        }

        //一打开介个窗口就运行
        private void 报错_Load(object sender, EventArgs e)
        {
            //一上来就显示在荧幕左下角
            this.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }
    }
}
