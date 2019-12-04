using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WmnSharpStdCodes.Windows
{
    public abstract class Kernel32
    {
        public const int PAGE_NOACCESS = 0x01;
        public const int PAGE_READONLY = 0x02;
        public const int PAGE_READWRITE = 0x04;
        public const int PAGE_WRITECOPY = 0x08;

        public const int PAGE_EXECUTE = 0x10;
        public const int PAGE_EXECUTE_READ = 0x20;
        public const int PAGE_EXECUTE_READWRITE = 0x40;
        public const int PAGE_EXECUTE_WRITECOPY = 0x80;

        public const int PAGE_GUARD = 0x100;
        public const int PAGE_NOCACHE = 0x200;
        public const int PAGE_WRITECOMBINE = 0x400;
        //public const int PAGE_REVERT_TO_FILE_MAP = 0x80000000;

        public const int MEM_COMMIT = 0x1000;
        public const int MEM_RESERVE = 0x2000;
        public const int MEM_DECOMMIT = 0x4000;
        public const int MEM_RELEASE = 0x8000;
        public const int MEM_FREE = 0x10000;
        public const int MEM_PRIVATE = 0x20000;
        public const int MEM_MAPPED = 0x40000;
        public const int MEM_TOP_DOWN = 0x100000;

        public const int MEM_WRITE_WATCH = 0x200000;
        public const int MEM_PHYSICAL = 0x400000;
        public const int MEM_ROTATE = 0x800000;
        public const int MEM_DIFFERENT_IMAGE_BASE_OK = 0x800000;
        public const int MEM_RESET_UNDO = 0x1000000;
        public const int MEM_LARGE_PAGES = 0x20000000;

        /// <summary>
        /// 查询地址空间中内存地址存储的信息
        /// </summary>
        /// <param name="hProcess">句柄</param>
        /// <param name="lpAddress">内存地址</param>
        /// <param name="lpBuffer">结构体指针</param>
        /// <param name="dwLength">结构体大小</param>
        /// <returns>写入字节数</returns>
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer,
        int dwLength);

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength); 

        /// <summary>
        /// 根据进程句柄读入该进程的某个内存空间
        /// </summary>
        /// <param name="lpProcess">进程句柄</param>
        /// <param name="lpBaseAddress">内存读取的起始地址</param>
        /// <param name="lpBuffer">写入地址</param>
        /// <param name="nSize">写入字节数</param>
        /// <param name="BytesRead">实际传递的字节数</param>
        /// <returns>读取结果</returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory
        (
            IntPtr lpProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int nSize,
            IntPtr BytesRead
        );

        [DllImport("kernel32.dll ")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out int lpBuffer, int nSize, out int lpNumberOfBytesRead); 

        //二维数组
        [DllImport("kernel32.dll ")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[,] lpBuffer, int nSize, out int lpNumberOfBytesRead);
        //一维数组
        [DllImport("kernel32.dll ")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead); 

        /// <summary>
        /// 打开一个已存在的进程对象，并返回进程的句柄
        /// </summary>
        /// <param name="iAccess">渴望得到的访问权限</param>
        /// <param name="Handle">是否继承句柄</param>
        /// <param name="ProcessID">进程PID</param>
        /// <returns>进程的句柄</returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess
        (
            int iAccess,
            bool Handle,
            int ProcessID
        );

        /// <summary>
        /// 写入某一进程的内存区域
        /// </summary>
        /// <param name="lpProcess">进程句柄</param>
        /// <param name="lpBaseAddress">写入的内存首地址</param>
        /// <param name="lpBuffer">写入数据的指针</param>
        /// <param name="nSize">写入字节数</param>
        /// <param name="BytesWrite">实际写入字节数的指针</param>
        /// <returns>大于0代表成功</returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory
            (
                IntPtr lpProcess,
                IntPtr lpBaseAddress,
                int[] lpBuffer,
                int nSize,
                IntPtr BytesWrite
            );

        public struct MEMORY_BASIC_INFORMATION
        {
            //区域基地址
            public int BaseAddress;
            //分配基地址
            public int AllocationBase;
            //区域被初次保留时赋予的保护属性
            public int AllocationProtect;
            //区域大小
            public int RegionSize;
            //状态
            public int State;
            //保护属性
            public int Protect;
            //类型
            public int lType;
        }

        [DllImport("kernel32.dll")]
        public static extern void CloseHandle
            (
            IntPtr hObject
            );


        public const int STD_INPUT_HANDLE = -10;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_ERROR_HANDLE = -12;
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 1;
        public const int SPIF_SENDWININICHANGE = 2;
        public const int GWL_STYLE = -16;
        public const uint WS_MINIMIZEBOX = 131072;
        public const uint WS_MAXIMIZEBOX = 65536;
        public const uint WS_CAPTION = 12582912;
        public const int SWP_NOSIZE = 1;

        [DllImport("kernel32")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        public static extern uint GetConsoleTitle([Out] StringBuilder lpConsoleTitle, uint nSize);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
