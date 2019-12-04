using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCommonCodes.WinOS
{
    public abstract class ProcessHelper
    {
        public static Process[] GetAll()
        {
            Process[] arrayProcess = Process.GetProcesses();
            return arrayProcess;
        }

        /// <summary>
        /// 根据窗口标题获取PID
        /// </summary>
        /// <param name="windowTitle">窗口标题</param>
        /// <returns></returns>
        public static int GetPidByTitle(string windowTitle)
        {
            int rs = 0;
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess)
            {
                if (p.MainWindowTitle.IndexOf(windowTitle) != -1)
                {
                    rs = p.Id;
                    break;
                }
            }
            return rs;
        }

        /// <summary>
        /// 根据进程名获取进程
        /// </summary>
        /// <param name="processName">进程名字</param>
        public static Process GetProcessByProcessName(string processName)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);
            foreach (Process p in arrayProcess)
            {
                return p;
            }
            return null;
        }

        /// <summary>
        /// 根据进程名获取PID
        /// </summary>
        /// <param name="processName">进程名字</param>
        /// <returns></returns>
        public static int GetPidByProcessName(string processName)
        {
            Process process= GetProcessByProcessName(processName);
            if(process!=null)
            {
                return process.Id;
            }
            return 0;
        }

        /// <summary>
        /// 根据窗口标题查找窗口句柄
        /// </summary>
        /// <param name="title">窗口标题</param>
        /// <returns></returns>
        public static IntPtr FindWindow(string title)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.MainWindowTitle.IndexOf(title) != -1)
                {
                    return p.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }
    }
}
