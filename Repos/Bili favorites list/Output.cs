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
using static Bili_favorites_list.Form1;

namespace Bili_favorites_list
{
    public partial class Output : Form
    {
        //全局访问介个窗口的实例
        public static Output 让我看看 { get; private set; }

        public Output()
        {
            InitializeComponent();
            让我看看 = this;
        }

        //输出弹窗关闭后
        private void Output_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        public static void 列表更新(ListView listView, String ML, String UID, String name, String title, String num, String intro,String ctime)
        {
            if (全局变量.运行状态 != true)
            {
                return;
            }
            void update() //单独封装一个方法
            {
                //更新列表数据
                listView.BeginUpdate();
                ListViewItem 列表 = new ListViewItem();
                列表.Text = ML;
                列表.SubItems.Add(UID);
                列表.SubItems.Add(name);
                列表.SubItems.Add(title);
                列表.SubItems.Add(num);
                列表.SubItems.Add(intro);
                列表.SubItems.Add(ctime);
                listView.Items.Add(列表);
                //自动滚动到底部
                listView.Items[listView.Items.Count - 1].EnsureVisible();
                listView.EndUpdate();
            }

            if (让我看看.IsHandleCreated == false)
            {
                update();
            }
            else
            {
                让我看看.Invoke(new MethodInvoker( ()=>
                {
                    update();
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            列表更新(this.listView1,"0000","UID000","介素昵称","介素标题","0","介素简介",DateTime.Now.ToString());
        }

        //窗口自适应
        private void Output_Resize(object sender, EventArgs e)
        {
            //this.columnHeader3.Width = (this.Width - 1060) + 797;
            //this.listView1.Width = (this.Width - 1000) + 1042;
        }

        //一打开介个窗口就运行
        private void Output_Load(object sender, EventArgs e)
        {
            //一上来就显示在荧幕左下角
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.Items.Count > 0 && this.listView1.SelectedIndices.Count > 0)
            {
                Clipboard.SetText(this.listView1.SelectedItems[0].SubItems[0].Text);
            }
        }
    }
}
