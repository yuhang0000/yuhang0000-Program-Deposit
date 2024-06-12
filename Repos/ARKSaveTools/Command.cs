using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using ARKSaveTools;

namespace Command_list
{
    public partial class Command
    {
        public class 全局变量
        {
            public static string Path;
            public static int 文件数量;
            public static string 当前操作是啥;
            public static string Buildtime = "2024-6-2";
        }
        public static string 选择文件夹位置(string Path)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择存档文件夹所在位置";
            dialog.SelectedPath = Path;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Path = dialog.SelectedPath;
            }
            return Path;
        }
        public static string 备份(string HostPath,string ToPath, string FilePath)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ToPath));
            string Path = FilePath.Substring(HostPath.Length);//截取文件名
            System.IO.File.Copy(FilePath, ToPath + Path, true);
            return null;
        }

        //这是一个UI更新委托
        public static Action<string> 俺想要UI更新 { get; set; }

        public static string 遍历文件(string HostPath,string ToPath)
        {
            DirectoryInfo dir = new DirectoryInfo(HostPath);
            if (Directory.Exists(HostPath) == false)
            {
                俺想要UI更新("错误");
                MessageBox.Show("找不到该路径:\r\n " + HostPath,"错误");
                return null;
            }
            FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                {
                    遍历文件(fsinfo.FullName, fsinfo.FullName);//递归调用
                }
                else
                {
                    全局变量.文件数量++;//和 全局变量.文件数量 = 全局变量.文件数量 + 1; 一样，但这个更简短
                    //if (俺想要UI更新 != null){ 俺想要UI更新("找到文件: " + fsinfo.FullName);}
                    备份(HostPath,ToPath,fsinfo.FullName);
                }
            }
            //Console.WriteLine(全局变量.文件数量);
            if (全局变量.当前操作是啥 == "备份")
            { 
            俺想要UI更新("已备份 " + 全局变量.文件数量 + " 个文件到: " + ARKSaveTools.Form1.让我看看.comboBox1.Text);
            }
            else if(全局变量.当前操作是啥 == "恢复")
            {
                俺想要UI更新("已从: "+ ARKSaveTools.Form1.让我看看.comboBox1.Text + " 中恢复 " + 全局变量.文件数量 + " 个文件" );
            }
            //对象引用未设置到对象的实例? 直接在前面加上 让我看看！！！
            SystemSounds.Beep.Play();
            return HostPath;
        }

    }
}