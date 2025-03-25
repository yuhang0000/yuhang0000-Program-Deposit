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
                        Console.WriteLine(network.profileName);
                        Console.WriteLine(network.wlanSignalQuality);
                        String q = network.wlanSignalQuality.ToString();
                        String n = network.profileName.ToString();
                        Array.Resize(ref wifilist, wifilist.Length + 1);
                        wifilist[wifilist.Length - 1] = q + "," + n;
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
            this.listView1.Height = this.Height - 93;
            this.listView2.Height = this.listView1.Height;
            int top = (this.Height / 2) - ((this.controlPad1.Height * 54) / 100);
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
    }
}
