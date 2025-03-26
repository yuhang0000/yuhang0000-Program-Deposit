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
            Console.WriteLine("loadding...");
            try
            {
                WlanClient wlan = new WlanClient();
                //循环扫描
                foreach (WlanClient.WlanInterface wlanInterface in wlan.Interfaces) //枚举所有网络通讯设备接口?
                {
                    Wlan.WlanAvailableNetwork[] networks = wlanInterface.GetAvailableNetworkList(0); //获取 WiFi 列表
                    foreach (Wlan.WlanAvailableNetwork network in networks) //遍历 WIFI
                    {
                        String[] strings = new String[2];
                        //Console.WriteLine(network.profileName);
                        //Console.WriteLine(network.wlanSignalQuality);
                        Console.WriteLine(network.dot11DefaultCipherAlgorithm);
                        Console.WriteLine("############################################");
                        //String ssid = Encoding.ASCII.GetString(network.dot11Ssid.SSID, 0 ,(int) network.dot11Ssid.SSIDLength);
                        //官方示例这里用的是 ASCII, 但不支援中文字集, 改用 UTF8
                        //另外 emoji 会显示为 ?? (头一次知道可以设置 emoji 🤔)
                        String ssid = Encoding.UTF8.GetString(network.dot11Ssid.SSID, 0 ,(int) network.dot11Ssid.SSIDLength);
                        //Console.WriteLine(ssid);
                        strings[0] = network.wlanSignalQuality.ToString();
                        //strings[1] = network.profileName.ToString();
                        strings[1] = ssid;
                        Array.Resize(ref wifilist, wifilist.Length + 1);
                        wifilist[wifilist.Length - 1] = string.Join(",",strings);
                    }
                }
                //把结果打印出来
                foreach (String list in wifilist)
                {
                    String[] lists = list.Split(',');
                    ListViewItem listItem = new ListViewItem();
                    listItem.Text = this.listView1.Items.Count.ToString();
                    listItem.SubItems.Add(lists[0]);
                    listItem.SubItems.Add(lists[1]);
                    this.listView1.Items.Add(listItem);
                }
            }
            catch(Exception ex)
            {
                SystemSounds.Hand.Play();
                ListViewItem listItem = new ListViewItem();
                listItem.Text = "Oops";
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
            int w = this.listView1.Width - this.columnHeader3.Width;
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
                this.columnHeader3.Width = w;
            }
            else
            {
                this.columnHeader3.Width = 44;
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
    }
}
