using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinMouseHelper
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //检查是否多开
            bool IsOpen = false;
            Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out IsOpen);
            if (IsOpen == false)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();

            //自动执行
            if (args != null && args.Length > 0 && args[0].ToLower() == "autorun")
            {
                form.AutoRun = true;
            }

            Application.Run(form);
        }
    }
}
