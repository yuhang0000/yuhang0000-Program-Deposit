﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Click_Messsge_List_on_NTQQ
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] aegs)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (aegs.Length == 0)
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Run(new Form1(aegs));
            }
        }
    }
}
