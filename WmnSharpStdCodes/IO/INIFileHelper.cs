using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmnSharpStdCodes.IO
{
    public static class INIFileHelper
    {
        //private string fileName;
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetPrivateProfileInt(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           int nDefault,// 如果 Key 值没有找到，则返回缺省的值是多少
           string lpFileName
           );
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetPrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           string lpDefault,// 如果 Key 值没有找到，则返回缺省的字符串的地址
           System.Text.StringBuilder lpReturnedString,// 返回字符串的缓冲区地址
           int nSize,// 缓冲区的长度
           string lpFileName
           );
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(
           string lpAppName,// 指向包含 Section 名称的字符串地址
           string lpKeyName,// 指向包含 Key 名称的字符串地址
           string lpString,// 要写的字符串地址
           string lpFileName
           );

        

        /// <summary>
        /// 获得INI文件的某一键的数值
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="def">
        /// 该键名无值时的默认值
        /// </param>
        public static int GetInt(string fileName, string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, fileName);
        }

        /// <summary>
        /// 获得INI文件的某一键的字符串
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="def">
        /// 该键名无值时的默认值
        /// </param>
        public static string GetString(string fileName, string section, string key, string def)
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, fileName);
            return temp.ToString();
        }

        /// <summary>
        /// 设置INI文件的某一键的数值
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="iVal">
        /// INI文件的键值
        /// </param>
        public static void WriteInt(string fileName, string section, string key, int iVal)
        {
            WritePrivateProfileString(section, key, iVal.ToString(), fileName);
        }

        /// <summary>
        /// 设置INI文件的某一键的字符串
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        /// <param name="strVal">
        /// INI文件的键值
        /// </param>
        public static void WriteString(string fileName, string section, string key, string strVal)
        {
            WritePrivateProfileString(section, key, strVal, fileName);
        }

        /// <summary>
        /// 删除某一个键
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        /// <param name="key">
        /// INI文件的键名
        /// </param>
        public static void DelKey(string fileName, string section, string key)
        {
            WritePrivateProfileString(section, key, null, fileName);
        }

        /// <summary>
        /// 删除某一个章节
        /// </summary>
        /// <param name="section">
        /// INI文件的章节
        /// </param>
        public static void DelSection(string fileName, string section)
        {
            WritePrivateProfileString(section, null, null, fileName);
        }
    }
}
