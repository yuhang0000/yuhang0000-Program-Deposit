using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Click_Messsge_List_on_NTQQ
{
    public partial class img : Form
    {
        public static img 让我看看 { get; private set; }
        public img()
        {
            InitializeComponent();
            让我看看 = this;
        }

        public async void toimg(Bitmap bitmap = null)
        {
            if(bitmap != null)
            {
                await Task.Run(() => { 
                    this.pictureBox1.Image = bitmap;
                    bitmap = null;
                });
            }
        }

        private void img_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.pictureBox1.Dispose();
            this.Dispose();
            GC.Collect();
        }
    }
}
