using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Principal;


namespace Off_your_display
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const uint SDC_APPLY = 0x00000080;
        private const uint SDC_TOPOLOGY_INTERNAL = 0x00000001;
        private const uint SDC_TOPOLOGY_CLONE = 0x00000002;
        private const uint SDC_TOPOLOGY_EXTERNAL = 0x00000008;
        private const uint SDC_TOPOLOGY_EXTEND = 0x00000004;
        //仅电脑屏幕
        private const uint smode = SDC_APPLY | SDC_TOPOLOGY_INTERNAL;
        //屏幕扩展
        //private const uint smode = SDC_APPLY | SDC_TOPOLOGY_EXTEND;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeArrayElements, IntPtr modeArray, uint flags);

        private void Form1_Load(object sender, EventArgs e)
        {
            SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, smode);
            Environment.Exit(0);
        }
    }
}
