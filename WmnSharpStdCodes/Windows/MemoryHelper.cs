using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WmnSharpStdCodes.Windows
{
    public static class MemoryHelper
    {
        /// <summary>
        /// 读取内存中的值
        /// </summary>
        /// <param name="baseAddress">地址</param>
        /// <param name="processName">进程名</param>
        public static int ReadMemoryValue( int pid,int baseAddress)
        {
            try
            {
                var buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, pid);
                Kernel32.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero); //将制定内存中的值读入缓冲区
                Kernel32.CloseHandle(hProcess);
                return Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        ///// <summary>
        ///// 读取内存中的值
        ///// </summary>
        ///// <param name="baseAddress">地址</param>
        ///// <param name="processName">进程名</param>
        //public static int ReadMemoryValue(int baseAddress, string processName)
        //{
        //    try
        //    {
        //        var buffer = new byte[4];
        //        IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
        //        IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, ProcessUtil.GetPidByProcessName(processName));
        //        Kernel32.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero); //将制定内存中的值读入缓冲区
        //        Kernel32.CloseHandle(hProcess);
        //        return Marshal.ReadInt32(byteAddress);
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress">地址</param>
        /// <param name="processName">进程名</param>
        /// <param name="value"></param>
        public static void WriteMemoryValue(int pid, int baseAddress, int value)
        {
            IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, pid); //0x1F0FFF 最高权限
            Kernel32.WriteProcessMemory(hProcess, (IntPtr)baseAddress, new[] { value }, 4, IntPtr.Zero);
            Kernel32.CloseHandle(hProcess);
        }

        ///// <summary>
        ///// 将值写入指定内存地址中
        ///// </summary>
        ///// <param name="baseAddress">地址</param>
        ///// <param name="processName">进程名</param>
        ///// <param name="value"></param>
        //public static void WriteMemoryValue(int baseAddress, string processName, int value)
        //{
        //    IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, ProcessUtil.GetPidByProcessName(processName)); //0x1F0FFF 最高权限
        //    Kernel32.WriteProcessMemory(hProcess, (IntPtr)baseAddress, new[] { value }, 4, IntPtr.Zero);
        //    Kernel32.CloseHandle(hProcess);
        //}

        public static List<int> SearchAddress(IntPtr hProcess,  int searchValue)//, int PAGE_READWRITE, int MEM_COMMIT)
        {
            Kernel32.MEMORY_BASIC_INFORMATION MBInfo = new Kernel32.MEMORY_BASIC_INFORMATION();
            int MBSize = Marshal.SizeOf(MBInfo); //获取结构体大小[单次读取字节数]

            int ReadSize = 0;//实际读取的字节数
            //从0开始查询，直到查询到整形的最大值2147483647
            List<int> AddressList = new List<int>();
            byte[] DataArray = new byte[4];
            int StartAddress = 0x000000;//从0x00开始查询
            long MaxAddress = 0x7fffffff;

            //long address = 0;
            //do
            //{
            //    Kernel32.MEMORY_BASIC_INFORMATION m;
            //    var hp2 = System.Diagnostics.Process.GetCurrentProcess().Handle;
            //    int result = Kernel32.VirtualQueryEx
            //        (hp2, (IntPtr)address, out m, (uint)Marshal.SizeOf(typeof(Kernel32.MEMORY_BASIC_INFORMATION)));
            //    Debug.WriteLine("{0}-{1} : {2} bytes result={3}", m.BaseAddress, (uint)m.BaseAddress + (uint)m.RegionSize - 1, m.RegionSize, result);
            //    if (address == (long)m.BaseAddress + (long)m.RegionSize)
            //        break;
            //    address = (long)m.BaseAddress + (long)m.RegionSize;
            //} while (address <= MaxAddress);

            while (StartAddress >= 0 && StartAddress <= MaxAddress && MBInfo.RegionSize >= 0)
            {
                MBSize = Kernel32.VirtualQueryEx(hProcess, (IntPtr)StartAddress, out MBInfo, (uint)Marshal.SizeOf(MBInfo)); //读取结果存入输出参数MBInfo
                if (MBSize == Marshal.SizeOf(typeof(Kernel32.MEMORY_BASIC_INFORMATION)))//如果实际读取到的字节数等于结构体MEMORY_BASIC_INFORMATION字节数，表示读取成功
                {
                    //PAGE_READWRITE:允许读写的内存区。
                    //MEM_COMMIT:已分配物理内存[要找的数值确定了,那么内存肯定提前分配了]。
                    if (MBInfo.Protect == Kernel32.PAGE_READWRITE && MBInfo.State == Kernel32.MEM_COMMIT)
                    {
                        byte[] FindArray = new byte[MBInfo.RegionSize];
                        //把读取到的字节写入上面定义的数组byData中
                        if (Kernel32.ReadProcessMemory(hProcess, (int)StartAddress, FindArray, (int)(MBInfo.RegionSize), out ReadSize))
                        {
                            if (ReadSize == MBInfo.RegionSize)//如果读取的字节数无误
                            {
                                DealData(FindArray, StartAddress, AddressList, searchValue);//处理数据[对比分析]
                            }
                        }  
                    }
                }
                else
                {
                    break;
                }
                StartAddress += MBInfo.RegionSize;
            }
            
            return AddressList;
        }

        private static void DealData(byte[] DataArray, int StartAddress, List<int> AddressList,int value)
        {
            byte[] intBuff = new byte[4];

            for (int i = 0; i < DataArray.Length - 4; i+=4)
            {
                Array.Copy(DataArray, i, intBuff, 0, 4);
                int num = BitConverter.ToInt32(intBuff, 0);
                //Debug.WriteLine(num);
                if (num == value)
                {
                    AddressList.Add(StartAddress + i);
                }
            }
        }

        //public static void SearchAddress(IntPtr hProcess, byte[] DataArray, int PAGE_READWRITE, int MEM_COMMIT)
        //{
        //    Kernel32.MEMORY_BASIC_INFORMATION MBInfo = new Kernel32.MEMORY_BASIC_INFORMATION();
        //    //获取结构体大小[单次读取字节数]
        //    int MBSize = Marshal.SizeOf(MBInfo);
        //    //从0x00开始查询
        //    int StartAddress = 0x000000;
        //    //实际读取的字节数
        //    int ReadSize = 0;
        //    //从0开始查询，直到查询到整形的最大值2147483647
        //    List<int> AddressList = new List<int>();
        //    while (StartAddress >= 0 && StartAddress <= 0x7fffffff && MBInfo.RegionSize >= 0)
        //    {
        //        //读取结果存入输出参数MBInfo
        //        MBSize = Kernel32.VirtualQueryEx(hProcess, (IntPtr)StartAddress, out MBInfo, Marshal.SizeOf(MBInfo));
        //        //如果实际读取到的字节数等于结构体MEMORY_BASIC_INFORMATION字节数，表示读取成功
        //        if (MBSize == Marshal.SizeOf(typeof(Kernel32.MEMORY_BASIC_INFORMATION)))
        //        {
        //            //PAGE_READWRITE:允许读写的内存区。
        //            //MEM_COMMIT:已分配物理内存[要找的数值确定了,那么内存肯定提前分配了]。
        //            if (MBInfo.Protect == PAGE_READWRITE && MBInfo.State == MEM_COMMIT)
        //            {
        //                byte[] FindArray = new byte[MBInfo.RegionSize];
        //                //把读取到的字节写入上面定义的数组byData中
        //                if (Kernel32.ReadProcessMemory(hProcess, (int)StartAddress, FindArray, (int)(MBInfo.RegionSize), out ReadSize))
        //                    //if (Kernel32.ReadProcessMemory(hProcess, (IntPtr)StartAddress, (IntPtr)FindArray, MBInfo.RegionSize,out ReadSize))
        //                    //如果读取的字节数无误
        //                    if (ReadSize == MBInfo.RegionSize)
        //                    {
        //                        //处理数据[对比分析]
        //                        DealData(DataArray, StartAddress, AddressList);
        //                    }
        //            }

        //            //IntPtr hwnd = FindWindow(null, "计算器");
        //            //const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        //            //const int PROCESS_VM_READ = 0x0010;
        //            //const int PROCESS_VM_WRITE = 0x0020;
        //            //if (hwnd != IntPtr.Zero)
        //            //{
        //            //    int calcID;
        //            //    int calcProcess;
        //            //    int dataAddress;
        //            //    int readByte;
        //            //    GetWindowThreadProcessId(hwnd, out calcID);
        //            //    calcProcess = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE, false, calcID);
        //            //    //如果地址0X0047C9D4存在信息
        //            //    ReadProcessMemory(calcProcess, 0X0047C9D4, out dataAddress, 4, out readByte);
        //            //    MessageBox.Show(dataAddress.ToString());
        //            //}
        //            //else
        //            //{
        //            //    MessageBox.Show("没有找到窗体");
        //            //}
        //        }
        //        else
        //        {
        //            break;
        //        }
        //        StartAddress += MBInfo.RegionSize;
        //    }
        //}

        
    }
}
