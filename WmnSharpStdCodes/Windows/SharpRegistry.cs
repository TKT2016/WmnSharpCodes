﻿/**
    2019-12-04:
    卓语言:中文编程，世界第一个非名参式编程语言
    网址:www.zyuyan.org
*/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WmnSharpStdCodes.Windows
{
    /// <summary>
    /// 操作windows注册表，支持32位和64位
    /// 版本:1.0，2019-12-04 09:17:15
    /// </summary>
    public class SharpRegistry
    {
        public RegistryKey RootRegistry { get; private set; }
        public RegistryKey CurrentRegistry { get; private set; }
        public string RootKeyName{ get; private set; }
        public string CurrentKeyName { get; private set; }

        public SharpRegistry(string fullKey)
        {
            if(string.IsNullOrWhiteSpace(fullKey))
            {
                throw new Exception("注册表项错误");
            }
            RootRegistry = GetRoot(fullKey);
            if(RootRegistry==null)
            {
                throw new Exception("注册表项错误");
            }
            RootKeyName = GetRootName(fullKey);
            CurrentKeyName = fullKey.Substring(RootKeyName.Length);
            Open();
        }

        public bool Exists
        {
            get
            {
                return CurrentRegistry != null;
            }
        }

        public void Create()
        {
            if (Exists) return;
            CurrentRegistry = RootRegistry.CreateSubKey(CurrentKeyName);
        }

        public void Delete()
        {
            Open();
            if (!Exists) return;
            RootRegistry.DeleteSubKey(CurrentKeyName, true);
            CurrentRegistry = null;
            Close();
        }

        private bool IsClose = false;

        internal void Open()
        {
            if (CurrentRegistry == null)
            {
                CurrentRegistry = RootRegistry.OpenSubKey(CurrentKeyName, true);
            }
            else if(IsClose)
            {
                CurrentRegistry = RootRegistry.OpenSubKey(CurrentKeyName, true);
            }
            IsClose = false;
        }

        public void Close()
        {
            if (CurrentRegistry != null)
            {
                CurrentRegistry.Close();
            }
            IsClose = true;
        }

        #region 注册表键值操作

        public object ReadDefault()
        {
            return ReadSub("");
        }

        public void WriteDefault(object value)
        {
            WriteSub("",value);
        }

        public object ReadSub(string item)
        {
            Open();
            object obj = CurrentRegistry.GetValue(item);
            return obj;
        }

        public void WriteSub(string item,object value)
        {
            Open();
            CurrentRegistry.SetValue(item, value);
            Close();
        }

        public void DeleteSub(string subName)
        {
            Open();
            CurrentRegistry.DeleteValue(subName);
            Close();
        }

        public bool ExistsSub(string subName)
        {
            Open();
            string[] subNames = GetSubItemNames();
            if (subNames == null) return false;
            if (subNames.Length == 0) return false;
            foreach (string keyName in subNames)
            {
                if (keyName == subName) 
                {
                    return true;
                }
            }
            return false;
        }

        public string[] GetSubItemNames()
        {
            Open();
            if (!Exists) return null;
            return CurrentRegistry.GetSubKeyNames();
        }

        #endregion

        public static RegistryKey GetRoot(string subKey)
        {
            if (string.IsNullOrWhiteSpace(subKey))
            {
                throw new ArgumentNullException("参数subKey不能为空");
            }
            string rootKeyName = subKey.ToUpper(CultureInfo.InvariantCulture);
            RegistryView registryView = RegistryView.Default;
            /* 必须区分64位和32位 */
            if(Environment.Is64BitOperatingSystem)
            {
                registryView = RegistryView.Registry64;
            }

            if (rootKeyName.StartsWith("HKEY_CLASSES_ROOT\\", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView);
            }
            else if (rootKeyName.StartsWith("HKEY_CURRENT_USER\\", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            }
            else if (rootKeyName.StartsWith("HKEY_LOCAL_MACHINE\\", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            }
            else if (rootKeyName.StartsWith("HKEY_USERS\\", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.Users, registryView);
            }
            else if (rootKeyName.StartsWith("HKEY_CURRENT_CONFIG\\", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, registryView);
            }
            else
            {
                //throw new Exception("注册表根目录不存在");
            }
            return null;
        }

        public static string GetRootName(string subKey)
        {
            if(string.IsNullOrWhiteSpace(subKey))
            {
                throw new ArgumentNullException("参数subKey不能为空");
            }
            string rootKeyName = subKey.ToUpper(CultureInfo.InvariantCulture);
            if (rootKeyName.StartsWith("HKEY_CLASSES_ROOT\\", StringComparison.OrdinalIgnoreCase))
            {
                return "HKEY_CLASSES_ROOT\\";
            }
            else if (rootKeyName.StartsWith("HKEY_CURRENT_USER\\", StringComparison.OrdinalIgnoreCase))
            {
                return "HKEY_CURRENT_USER\\";
            }
            else if (rootKeyName.StartsWith("HKEY_LOCAL_MACHINE\\", StringComparison.OrdinalIgnoreCase))
            {
                return "HKEY_LOCAL_MACHINE\\";
            }
            else if (rootKeyName.StartsWith("HKEY_USERS\\", StringComparison.OrdinalIgnoreCase))
            {
                return "HKEY_USERS\\";
            }
            else if (rootKeyName.StartsWith("HKEY_CURRENT_CONFIG\\", StringComparison.OrdinalIgnoreCase))
            {
                return "HKEY_CURRENT_CONFIG\\";
            }
            else
            {
                //throw new Exception("注册表根目录不存在");
            }
            return null;
        }
    }
}
