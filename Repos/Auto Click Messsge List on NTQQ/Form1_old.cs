using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Auto_Click_Messsge_List_on_NTQQ
{
    public partial class Form1_old : Form
    {
        public Form1_old()
        {
            //生成 0 - 100 的图标
            int n = -1;
            while (n < 102)
            {
                if (n == -1)
                {
                    sss.numicon[n + 1] = img("ZZZ");
                }
                else if (n == 101)
                {
                    sss.numicon[n + 1] = img(num: "100", color1: Color.Red);
                }
                else
                {
                    sss.numicon[n + 1] = img(n.ToString());
                }
                n++;
            }
            InitializeComponent();
        }

        [DllImport("user32")]
        public static extern int GetCursorPos(out System.Drawing.Point lpPoint);

        //获取鼠标位置
        public int[] cursor()
        {
            int[] xy = new int[2];
            Point mp = new Point();
            GetCursorPos(out mp);
            xy[0] = mp.X;
            xy[1] = mp.Y;
            return xy;
        }

        //全局变量
        public static class sss
        {
            public static Icon[] numicon = new Icon[103];
            public static int mx = 0;
            public static int my = 0;
            public static int[] xy;
            public static int press = 102;
            public static int 阈值 = 166;
            public static int 强度 = 3;
            public static String lang = "chi_sim";
            //public static Rectangle box = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width,
            //        Screen.PrimaryScreen.WorkingArea.Height);
            public static Rectangle box = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width,
                    Screen.PrimaryScreen.WorkingArea.Height);
        }

        //生成图像
        public Icon img(String num = "0", int imgsize = 32, float fontsize = 10f, Color? color1 = null)
        {
            Color bcolor = color1 ?? Color.Black;
            if(fontsize == 0)
            {
                fontsize = 1f;
            }
            if(imgsize == 0)
            {
                imgsize = 1;
            }
            Bitmap bitmap = new Bitmap(imgsize,imgsize);
            Graphics g = Graphics.FromImage(bitmap);

            //float textw = num.Length + fontsize;
            //Console.WriteLine(textw);
            g.Clear(Color.Transparent);
            //g.Clear(Color.FromArgb(222,222,222));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font ttt = new Font("Microsoft YaHei UI",fontsize,FontStyle.Regular);
            Brush brush = new SolidBrush(bcolor);
            Console.WriteLine( g.MeasureString(num,ttt) );
            float fontw = g.MeasureString(num,ttt).Width;
            float fonth = g.MeasureString(num,ttt).Height;

            //文字居中
            RectangleF box = new RectangleF( (imgsize - fontw) / 2 ,(imgsize - fonth) / 2, 
                fontw,fonth );
            Console.WriteLine(box.ToString());

            g.DrawString(num, ttt, brush, box);
            Icon ico = Icon.FromHandle(bitmap.GetHicon());
            this.Icon = ico;
            bitmap.Dispose();
            g.Dispose();
            return ico;
        }

        //一打开就运行
        private void Form1_Load(object sender, EventArgs e)
        {
            //img();
            this.Icon = sss.numicon[102];
            this.notifyIcon1.Icon = sss.numicon[102];
            sss.xy = cursor();
            sss.mx = sss.xy[0];
            sss.my = sss.xy[1];
            this.timer1.Enabled = true;
        }

        //生成
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                img(this.textBox1.Text,this.trackBar2.Value,this.trackBar1.Value);
                this.label1.Text = "fontsize: " + this.trackBar1.Value.ToString() + " imgsize: " + 
                    this.trackBar2.Value.ToString();
            }
        }

        //拖动条变化
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                img(this.textBox1.Text, this.trackBar2.Value, this.trackBar1.Value);
                this.label1.Text = "fontsize: " + this.trackBar1.Value.ToString() + " imgsize: " + 
                    this.trackBar2.Value.ToString();
            }
        }

        //1-100 切换Icon
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.Icon = sss.numicon[this.trackBar3.Value];
            this.notifyIcon1.Icon = sss.numicon[this.trackBar3.Value];
        }

        // 计算空闲时间
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINOUTINFO
        {
            public int cbSize;  //设置结构体块容量
            public uint dwTime; //抓获时间
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINOUTINFO Plii);
        public static long GetLastInputTime()
        {
            LASTINOUTINFO vLastInputinfo = new LASTINOUTINFO();
            vLastInputinfo.cbSize = Marshal.SizeOf(vLastInputinfo);
            GetLastInputInfo(ref vLastInputinfo);
            long count = Environment.TickCount - (long)vLastInputinfo.dwTime;
            count = count / 1000;
            return count;
        }

        //计时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine(GetLastInputTime());
            if(GetLastInputTime() == 0)
            {
                sss.press = 102;
            }
            else
            {
                if (sss.press > 0)
                {
                    sss.press--;
                    if(sss.press == 0)//触发
                    {
                        //this.WindowState = FormWindowState.Minimized;
                        run();
                    }
                }
            }
            this.Icon = sss.numicon[sss.press];
            this.notifyIcon1.Icon = sss.numicon[sss.press];
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);   //最大/小化指定窗口
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); //获取窗口所属的进程ID
        public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallback callPtr, int lParam); //枚举所有窗口
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);    //获取窗口标题
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);    //获取窗口是否可视

        public void run()
        {
            //IntPtr exe = FindWindow(null, "QQ");
            Process[] exe = Process.GetProcessesByName("QQ");

            //if(exe == IntPtr.Zero)
            this.textBox2.Text = "总计: " + exe.Length.ToString();
            if(exe.Length == 0)
            {
                Console.WriteLine("找不到进程: QQ.exe");
                this.textBox2.Text = "找不到进程: QQ.exe";
            }
            else
            {
                //ShowWindow(exe,9);
                bool areufind = false;
                foreach (Process proc in exe) //先批量恢复窗口
                {
                    // 枚举所有顶层窗口，并检查它们是否属于当前进程
                    EnumWindows((hWnd, lParam) =>
                    {
                        // 获取窗口所属的进程ID
                        GetWindowThreadProcessId(hWnd, out int procId);
                        if (procId == proc.Id) {
                            int chars = 256;
                            string areuwindow = "";
                            StringBuilder Buff = new StringBuilder(chars);  //标题容器
                            GetWindowText(hWnd, Buff, chars);   //获取标题
                            if (IsWindowVisible(hWnd) == false) //是不是窗口
                            {
                                areuwindow = "\t*非窗口*";
                            }
                            this.textBox2.AppendText("\r\n" + hWnd + "\t" + Buff + areuwindow);
                            if (Buff.ToString() == "QQ") { 
                                if(IsWindowVisible(hWnd) == true)
                                {
                                    areufind = true;
                                    ShowWindow(hWnd, 3);
                                }
                            }
                        }
                        return true; // 返回 true 继续枚举，false 停止枚举
                    }, 0);

                    //this.textBox2.AppendText("\r\n" + proc.MainWindowTitle + "\t" + proc.MainWindowHandle);
                    //ShowWindow(proc.MainWindowHandle, 9);
                }
                if(areufind == false)
                {
                    this.textBox2.AppendText("\r\n" + "找不到进程: QQ.exe");
                    Console.WriteLine("找不到进程: QQ.exe");
                    //MessageBox.Show("找不到进程: QQ.exe","Oops! ");
                }
                else //执行
                {
                    shot();
                }
            }
        }

        [DllImport("winmm")]
        static extern uint timeGetTime();

        public async void shot()
        {
            uint starttime = timeGetTime();
            String runtime;
            Bitmap bitmap = null;
            await Task.Run(() =>
            {
                //先截屏
                Screen screen = Screen.AllScreens.FirstOrDefault();
                Rectangle box = sss.box;
                bitmap = new Bitmap(box.Width, box.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bitmap);
                g.CopyFromScreen(box.X, box.Y, 0, 0, box.Size, CopyPixelOperation.SourceCopy);
                //后期处理图片
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for(int j = 0; j < bitmap.Height; j++)
                    {
                        Color pixel = bitmap.GetPixel(i, j);
                        byte R = pixel.R;
                        byte G = pixel.G;
                        byte B = pixel.B;
                        //Console.WriteLine("R: " + R + " G: " + G + " B: " + B);
                        int BBB = (R + G + B)/3;
                        //int BBB = (Math.Max(R,Math.Max(G,B)));
                        //对比度
                        if(BBB > sss.阈值)
                        {
                            BBB = BBB * sss.强度;
                        }
                        else
                        {
                            BBB = BBB / sss.强度;
                        }
                        if(BBB > 255)
                        {
                            BBB = 255;
                        }
                        else if(BBB < 0)
                        {
                            BBB = 0;
                        }
                        bitmap.SetPixel(i, j, Color.FromArgb(BBB,BBB,BBB));
                    }
                }

                //垃圾回收
                g = null;
            });
            runtime = "图像处理耗时: " + (timeGetTime() - starttime).ToString() + "ms";
            Form imgform = new img();
            imgform.Show();
            Auto_Click_Messsge_List_on_NTQQ.img.让我看看.Width = bitmap.Width / 2 + this.Width - this.ClientSize.Width;
            Auto_Click_Messsge_List_on_NTQQ.img.让我看看.Height = bitmap.Height / 2 + this.Height - this.ClientSize.Height;
            Auto_Click_Messsge_List_on_NTQQ.img.让我看看.Opacity = 1;
            //Auto_Click_Messsge_List_on_NTQQ.img.让我看看.pictureBox1.Image = bitmap;
            await Task.Run(() => Auto_Click_Messsge_List_on_NTQQ.img.让我看看.toimg(bitmap) );

            //OCR识别
            String text = "";
            await Task.Run(() => {
                using (var orc = new TesseractEngine(@"./tessdata", sss.lang))
                {
                    using (var img = PixConverter.ToPix(bitmap))
                    {
                        using (var page = orc.Process(img))
                        {
                            text = page.GetText();
                            while(text.IndexOf(" ") != -1)
                            {
                                runtime = runtime + "\r\n文本识别耗时: " + (timeGetTime() - starttime).ToString() + "ms";
                                text = text.Replace(" ","");
                            }
                            var textxy = page.GetIterator();
                            textxy.Begin();

                            do
                            {
                                do
                                {
                                    if (textxy.IsAtBeginningOf(PageIteratorLevel.TextLine))
                                    {
                                        // Get the text of the current line
                                        string word = textxy.GetText(PageIteratorLevel.TextLine);
                                        // Get bounding box for the current line
                                        if (textxy.TryGetBoundingBox(PageIteratorLevel.TextLine, out var rect))
                                        {
                                            //Console.WriteLine($"Text: {word}, Position: ({rect.X1}, {rect.Y1}, {rect.Width}, {rect.Height})");
                                            int startX = rect.X1;
                                            int startY = rect.Y1;
                                            int endX = Math.Min(rect.X1 + rect.Width, bitmap.Width); // 确保不超出位图宽度
                                            int endY = Math.Min(rect.Y1 + rect.Height, bitmap.Height); // 确保不超出位图高度

                                            // 绘制顶边和底边
                                            for (int x = startX; x < endX; x++)
                                            {
                                                if (startY >= 0 && startY < bitmap.Height) // 检查顶边是否在范围内
                                                    bitmap.SetPixel(x, startY, Color.Red); // 顶边

                                                if ((endY - 1) >= 0 && (endY - 1) < bitmap.Height) // 检查底边是否在范围内
                                                    bitmap.SetPixel(x, endY - 1, Color.Red); // 底边
                                            }

                                            // 绘制左边和右边
                                            for (int y = startY; y < endY; y++)
                                            {
                                                if (startX >= 0 && startX < bitmap.Width) // 检查左边是否在范围内
                                                    bitmap.SetPixel(startX, y, Color.Red); // 左边

                                                if ((endX - 1) >= 0 && (endX - 1) < bitmap.Width) // 检查右边是否在范围内
                                                    bitmap.SetPixel(endX - 1, y, Color.Red); // 右边
                                            }
                                        }
                                    }
                                } while (textxy.Next(PageIteratorLevel.Word));

                            } while (textxy.Next(PageIteratorLevel.Block));

                        }
                    }
                }
            });
            await Task.Run(() => Auto_Click_Messsge_List_on_NTQQ.img.让我看看.toimg(bitmap));
            this.textBox2.AppendText(runtime + "\r\n图像标注耗时: " + (timeGetTime() - starttime).ToString() + "ms\r\n");
            this.textBox2.AppendText("\r\n" + text);

            //垃圾回收
            //imgform.Dispose();
            bitmap = null;
            GC.Collect();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.textBox2.Width = 418 + this.Width - 461;
            this.textBox2.Height = 138 + this.Height - 428;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            shot();
        }
    }
}
