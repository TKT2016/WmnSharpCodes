using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WmnSharpStdCodes.IO
{
    /// <summary>
    /// 对文件的操作
    /// </summary>
    public static class FileHelper
    {
        public static void SaveFile(string content, string fileName)
        {
            using (System.IO.StreamWriter toWrite = new System.IO.StreamWriter(fileName))
            {
                toWrite.Write(content);
                toWrite.Close();
            }
        }

        public static string ReadFileUTF8(string fileName)
        {
           var text1 = File.ReadAllText(fileName);
           byte[] mybyte = Encoding.UTF8.GetBytes(text1);
            var text2 = Encoding.UTF8.GetString(mybyte);
            return text2;
        }

        /// <summary>
        /// 得到文件的完全名字 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FullFileName(string path)
        {
            return Path.GetFileName(path);//path不存在就返回path
        }

        /// <summary>
        /// 查检上传文件后缀是否符合要求
        /// </summary>
        /// <param name="filename">文件名，如：xxx.docx</param>
        /// <param name="strextensions">允许上传的文件名小写数组</param>
        /// <returns></returns>
        public static bool CheckFileExtension(string filename, string[] strextensions)
        {
            bool b = false;
            foreach (string s in strextensions)
            {
                if (filename.ToLower().EndsWith(s))
                    b = true;
            }
            return b;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        public static void WriteFile(string context/*写入内容*/, string filePath)
        {
            FileStream fs1 = null;
            fs1 = !File.Exists(filePath) ? new FileStream(filePath, FileMode.Create, FileAccess.Write) : new FileStream(filePath, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(context);//开始写入值
            sw.Close();
            fs1.Close();
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 复制文件,将我们的文件复制到一个新文件,新文件如果已经存在则覆盖
        /// </summary>
        /// <param name="frompath">源文件</param>
        /// <param name="topath">目标文件</param>
        public static bool CopyFile(string frompath, string topath)
        {
            try
            {
                if (File.Exists(frompath))
                {
                    File.Copy(frompath, topath, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="fromfile"></param>
        /// <param name="tofile"></param>
        public static void MoveFile(string fromfile, string tofile)
        {
            if (File.Exists(fromfile))
            {
                File.Move(fromfile, tofile);
            }
        }
        /// <summary>
        /// 创建文件夹如果存在则不创建
        /// </summary>
        /// <param name="dirpath"></param>
        public static void CreateDirectory(string dirpath)
        {
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void MoveDirectory(string sourceDirName, string destDirName)
        {
            if (Directory.Exists(sourceDirName))
            {
                Directory.Move(sourceDirName, destDirName);
            }

        }
        /// <summary>
        /// 文件夹复制
        /// </summary>
        /// <param name="sourceDirName">原始路径</param>
        /// <param name="destDirName">目标路径</param>
        /// <returns></returns>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
            {
                sourceDirName = sourceDirName + "\\";
            }

            if (destDirName.Substring(destDirName.Length - 1) != "\\")
            {
                destDirName = destDirName + "\\";
            }

            if (!Directory.Exists(sourceDirName)) return;
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            foreach (string item in Directory.GetFiles(sourceDirName))
            {
                File.Copy(item, destDirName + Path.GetFileName(item), true);
            }
            foreach (string item in Directory.GetDirectories(sourceDirName))
            {
                CopyDirectory(item, destDirName + item.Substring(item.LastIndexOf("\\", System.StringComparison.Ordinal) + 1));
            }
        }
    }
}
