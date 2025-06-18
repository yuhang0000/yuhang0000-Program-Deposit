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
using static System.Net.WebRequestMethods;

namespace Bili_favorites_list
{
    public partial class 报错 : Form
    {
        //全局访问介个窗口的实例
        public static 报错 让我看看 { get; private set; }

        public static List<ListViewItem> lists = new List<ListViewItem>();

        public 报错()
        {
            InitializeComponent();
            让我看看 = this;
        }

        //错误弹窗关闭后
        private void 报错_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bili_favorites_list.Form1.全局变量.错误弹窗的状态 = false;
            Bili_favorites_list.Form1.全局变量.错误计数 = 0;
            lists.Clear();
        }

        public static Dictionary<string, string> errlist = new Dictionary<string, string>()
        {
            {"System.ArgumentException", ""},
            {"System.Net.Http.HttpRequestException", ""},
            {"System.Net.WebException", ""},
            {"System.Threading.Tasks.TaskCanceledException", "---> 大概是连接超时?"},
            {"System.InvalidOperationException", ""},
            {"System.Net.Sockets.SocketException", ""},
            {"System.IO.IOException", ""},
            {"System.Security.Authentication.AuthenticationException", ""},
            {"System.UriFormatException", ""},
        };

        public static void 列表更新(String 时间, String 错误)
        {
            //错误信息筛选
            /*
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
            if (错误.Contains("System.IO.IOException: ") == true)
            {
                错误 = 错误.Replace("System.IO.IOException: ", "");
            }
            if (错误.Contains("System.Security.Authentication.AuthenticationException: ") == true)
            {
                错误 = 错误.Replace("System.Security.Authentication.AuthenticationException: ", "");
            }
            if (错误.Contains("System.UriFormatException: ") == true)
            {
                错误 = 错误.Replace("System.UriFormatException: ", "");
            }*/
            
            foreach (var list2 in errlist)
            {
                if (错误.Contains(list2.Key + ": ") == true)
                {
                    错误 = 错误.Replace(list2.Key + ": ", "");
                    错误 = 错误 + list2.Value;
                }
            }

            //更新列表数据
            void update()
            {
                //listView.BeginUpdate();
                ListViewItem 列表 = new ListViewItem();
                列表.Text = (Bili_favorites_list.Form1.全局变量.错误计数++).ToString();
                列表.SubItems.Add(时间);
                列表.SubItems.Add(错误);
                //listView.Items.Add(列表);
                lists.Add(列表);
                if (让我看看.WindowState != FormWindowState.Minimized)
                {
                    让我看看.listView1.VirtualListSize = lists.Count;
                    //自动滚动到底部
                    让我看看.listView1.Items[让我看看.listView1.Items.Count - 1].EnsureVisible();
                }
                //listView.EndUpdate();
            }

            if (让我看看.IsHandleCreated == false)
            {
                update();
            }
            else
            {
                让我看看.Invoke( new MethodInvoker( () =>
                {
                    update() ;
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            列表更新(DateTime.Now.ToString(), "caooo");
        }

        //窗口自适应
        private void 报错_Resize(object sender, EventArgs e)
        {
            this.columnHeader3.Width = (this.Width - 1060) + 787;
            //this.listView1.Width = (this.Width - 1000) + 1042;
        }

        //一打开介个窗口就运行
        private void 报错_Load(object sender, EventArgs e)
        {
            //一上来就显示在荧幕左下角
            this.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //string text;
            //if(this.listView1.Items.Count > 0 && this.listView1.SelectedIndices.Count > 0)
            if(lists.Count > 0 && this.listView1.SelectedIndices.Count > 0)
            {
                //text = this.listView1.SelectedItems[0].SubItems[2].Text;
                //MessageBox.Show(text);
                Clipboard.SetText(lists[this.listView1.SelectedIndices[0]].SubItems[2].Text);
            }
        }

        public void listView1_RetrieveVirtualItemEventHandler(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (lists.Count > 0)
            {
                e.Item = lists[e.ItemIndex];
            }
        }

        private void 报错_Activated(object sender, EventArgs e)
        {
            if(lists.Count > 0 && this.listView1 != null)
            {
                this.listView1.VirtualListSize = lists.Count;
                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
            }
        }
    }
}
