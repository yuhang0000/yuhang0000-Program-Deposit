using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bili_favorites_list
{
    internal class 指令
    {
        public static String 截取字符(String 文本, String 头, String 尾, int 第几个 = 1)
        {
            int 开始位置 = -1;
            int 结束位置 = -1;
            int 匹配计数 = 0;

            // 查找头部标记的位置
            开始位置 = 文本.IndexOf(头);
            while (开始位置 != -1 && 匹配计数 < 第几个 - 1)
            {
                开始位置 = 文本.IndexOf(头, 开始位置 + 头.Length);
                匹配计数++;
            }

            if (开始位置 == -1)
                return "";

            // 查找尾部标记的位置
            结束位置 = 文本.IndexOf(尾, 开始位置);
            if (结束位置 == -1)
                return "";

            // 返回截取的子串
            return 文本.Substring(开始位置 + 头.Length, 结束位置 - 开始位置 - 头.Length);
        }
    }
}
