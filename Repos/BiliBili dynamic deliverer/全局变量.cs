using System;
using static System.Net.WebRequestMethods;

namespace 全局变量集
{
    internal class 全局变量
    {
        public static String 版本 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static DateTime 编译时间 = System.IO.File.GetLastWriteTime(typeof(全局变量).Assembly.Location);
        public static String[] 目标UID;
        public static String 抓取地址 = "https://api.bilibili.com/x/polymer/web-dynamic/v1/feed/all?offset=&host_mid=";
        public static int 刷新周期 = 600000;
        public static int 操作间隙 = 1000;
        public static int 随机刷新周期 = 60000;
        public static int 随机操作间隙 = 500;
        public static bool 程序打开时自动运行 = false;
        public static bool 启动时最小化到托盘 = false;
        public static bool 显示输出时间 = true;
        public static bool 导出数据 = true;
        public static bool 导出日志 = true;
        public static bool 导出数据清单 = true;
        public static bool 保存动态 = true;
        public static bool 包括图像 = true;
        public static bool 包括评论 = true;
        public static bool 记录目标账号信息 = true;
        public static String SESSDATA = "";
        public static String Token = "";
        public static bool 自动更新登录信息 = true;

    }
}
