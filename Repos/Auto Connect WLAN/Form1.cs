using NativeWifi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Connect_WLAN
{
    public partial class Form1 : Form
    {
        public static Form1 让我看看 { get; private set; }
        public Form1()
        {
            InitializeComponent();
            让我看看 = this;
            this.Text = this.Text + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.toolStripStatusLabel1.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void clearlist()
        {
            this.listView1.Items.Clear();
        }

        async public void sreach()
        {
            this.controlPad1.button7.Enabled = false;
            this.listView1.Items.Clear();
            String[] wifilist = { };
            //Console.WriteLine("loadding...");
            try
            {
                WlanClient wlan = new WlanClient();
                //循环扫描
                foreach (WlanClient.WlanInterface wlanInterface in wlan.Interfaces) //枚举所有网络通讯设备接口?
                {
                    Wlan.WlanAvailableNetwork[] networks = wlanInterface.GetAvailableNetworkList(0); //获取 WiFi 列表
                    foreach (Wlan.WlanAvailableNetwork network in networks) //遍历 WIFI
                    {
                        String[] strings = new String[3];
                        //Console.WriteLine(network.profileName);
                        //Console.WriteLine(network.wlanSignalQuality);
                        //Console.WriteLine(network.dot11DefaultAuthAlgorithm);
                        //Console.WriteLine("############################################");
                        //String ssid = Encoding.ASCII.GetString(network.dot11Ssid.SSID, 0 ,(int) network.dot11Ssid.SSIDLength);
                        //官方示例这里用的是 ASCII, 但不支援中文字集, 改用 UTF8
                        //另外 emoji 会显示为 ?? (头一次知道可以设置 emoji 🤔)
                        String ssid = Encoding.UTF8.GetString(network.dot11Ssid.SSID, 0 ,(int) network.dot11Ssid.SSIDLength);
                        //Console.WriteLine(ssid);
                        strings[0] = network.wlanSignalQuality.ToString();
                        //strings[1] = network.profileName.ToString();
                        strings[1] = network.dot11DefaultAuthAlgorithm.ToString();
                        if(strings[1] == "8")
                        {
                            strings[1] = "WPA3";
                        }
                        if(strings[1] == "9")
                        {
                            strings[1] = "WPA3_SAE";
                        }
                        if(strings[1] == "10")
                        {
                            strings[1] = "OWE";
                        }
                        if(strings[1] == "11")
                        {
                            strings[1] = "WPA_ENT";
                        }
                        strings[2] = ssid;
                        Array.Resize(ref wifilist, wifilist.Length + 1);
                        wifilist[wifilist.Length - 1] = string.Join(",",strings);
                    }
                    networks = null;
                }
                wlan = null;
                this.controlPad1.button1.Enabled = false;
                this.controlPad1.button2.Enabled = false;
                //把结果打印出来
                foreach (String list in wifilist)
                {
                    String[] lists = list.Split(',');
                    ListViewItem listItem = new ListViewItem();
                    listItem.Text = this.listView1.Items.Count.ToString();
                    listItem.SubItems.Add(lists[0]);
                    listItem.SubItems.Add(lists[1]);
                    listItem.SubItems.Add(lists[2]);
                    this.listView1.Items.Add(listItem);
                }
            }
            catch(Exception ex)
            {
                SystemSounds.Hand.Play();
                ListViewItem listItem = new ListViewItem();
                listItem.Text = "Oops";
                listItem.SubItems.Add("-1");
                listItem.SubItems.Add("-1");
                listItem.SubItems.Add(ex.Message);
                this.listView1.Items.Add(listItem);
                //如果窗口没有最小化, 就弹窗提醒
                if(this.WindowState != FormWindowState.Minimized)
                {
                    MessageBox.Show(ex.Message, "Oops! ");
                }
            }
            await Task.Delay(1000);
            this.controlPad1.button7.Enabled = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int w = this.listView1.Width - this.columnHeader4.Width;
            this.listView1.Width = (this.Width /2) - 91;
            this.listView2.Width = this.listView1.Width;
            this.controlPad1.Left = 35 + this.listView1.Width;
            this.listView2.Left = this.controlPad1.Left + 115;
            this.label2.Left = this.listView2.Left + 1;
            this.listView1.Height = this.Height - 122;
            this.listView2.Height = this.listView1.Height;
            int top = (this.Height / 2) - ((this.controlPad1.Height * 60) / 100);
            w = this.listView1.Width - w;
            if(w > 44)
            {
                this.columnHeader4.Width = w;
            }
            else
            {
                this.columnHeader4.Width = 44;
            }
            //Console.WriteLine(top);
            if (top >= 34)
            {
                this.controlPad1.Top = top;
            }
            else
            {
                this.controlPad1.Top = 34;
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.ShowDialog();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                this.controlPad1.button7.Focus();
                if (this.controlPad1.button7.Enabled == true)
                {
                    sreach();
                }
            }
            if(e.KeyCode == Keys.F1)
            {
                toolStripStatusLabel1_Click(null,null);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(columnHeader1.Width != 0)
            {
                toolStripMenuItem2.Checked = true;
            }
            else
            {
                toolStripMenuItem2.Checked = false;
            }
            if(columnHeader2.Width != 0)
            {
                强度ToolStripMenuItem.Checked = true;
            }
            else
            {
                强度ToolStripMenuItem.Checked = false;
            }
            if(columnHeader3.Width != 0)
            {
                加密方式ToolStripMenuItem.Checked = true;
            }
            else
            {
                加密方式ToolStripMenuItem.Checked = false;
            }
            if(columnHeader4.Width != 0)
            {
                sSIDToolStripMenuItem.Checked = true;
            }
            else
            {
                sSIDToolStripMenuItem.Checked = false;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (columnHeader1.Width != 0)
            {
                columnHeader1.Width = 0;
            }
            else
            {
                columnHeader1.Width = 42;
            }
        }

        private void 强度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (columnHeader2.Width != 0)
            {
                columnHeader2.Width = 0;
            }
            else
            {
                columnHeader2.Width = 44;
            }
        }

        private void 加密方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (columnHeader3.Width != 0)
            {
                columnHeader3.Width = 0;
            }
            else
            {
                columnHeader3.Width = 125;
            }
        }

        private void sSIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (columnHeader4.Width != 0)
            {
                columnHeader4.Width = 0;
            }
            else
            {
                columnHeader4.Width = 150;
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                this.controlPad1.button1.Enabled = true;
                this.controlPad1.button2.Enabled = true;
            }
            else if(this.listView1.SelectedItems.Count > 1)
            {
                this.controlPad1.button1.Enabled = false;
                this.controlPad1.button2.Enabled = true;
            }
            else
            {
                this.controlPad1.button1.Enabled = false;
                this.controlPad1.button2.Enabled = false ;
            }
        }
    }
}
