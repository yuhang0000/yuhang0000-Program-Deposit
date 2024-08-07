using System;
using System.IO;
using 全局变量集;
using static BiliBili_dynamic_deliverer.Form1;

namespace Command
{
    internal class 指令
    {
        public static void 写入配置文档()
        {
            File.WriteAllText("./Setting.ini",
                    "[BiliBili dynamic deliverer]" + "\r\n版本="+ 全局变量.版本 + 
                    "\r\n\n[抓取]" + "\r\n目标UID=" + 全局变量.目标UID + "\r\n刷新周期=" + 全局变量.刷新周期 + 
                    "\r\n操作间隙=" + 全局变量.操作间隙 + "\r\n随机刷新周期=" + 全局变量.随机刷新周期 + 
                    "\r\n随机操作间隙=" + 全局变量.随机操作间隙 + "\r\n抓取地址=" + 全局变量.抓取地址 + 
                    "\r\n程序打开时自动运行=" + 全局变量.程序打开时自动运行 + 
                    "\r\n启动时最小化到托盘=" + 全局变量.启动时最小化到托盘 + "\r\n显示输出时间=" + 全局变量.显示输出时间 + 
                    "\r\n\n[导出]" + "\r\n导出数据=" + 全局变量.导出数据 + "\r\n导出日志=" + 全局变量.导出日志 + 
                    "\r\n导出数据清单=" + 全局变量.导出数据清单 + "\r\n\n[动态]" + "\r\n保存动态=" + 全局变量.保存动态 + 
                    "\r\n包括图像=" + 全局变量.包括图像 + "\r\n包括评论=" + 全局变量.包括评论 + "\r\n\n[其他]" + 
                    "\r\n记录目标账号信息=" + 全局变量.记录目标账号信息 + "\r\n\n[登录凭证]" + 
                    "\r\nSESSDATA=" + 全局变量.SESSDATA + "\r\nToken=" + 全局变量.Token + 
                    "\r\n自动更新登录信息=" + 全局变量.自动更新登录信息 +
                    "\r\n\n[By:yuhang0000]");
        }

        public static void 读取配置文档()
        {
            String 配置文档 = File.ReadAllText("./Setting.ini");

        }

        //获取当前时间
        public static string 时间()
        {
            String output = 让我看看.textBox2.Text + "\r\n";
            if (全局变量.显示输出时间 == true)
            {
                output = output + System.DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "|";
            }
            return output;
        }
    }
}
