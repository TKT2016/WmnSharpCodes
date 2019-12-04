using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WmnSharpStdCodes.Windows
{
    public delegate bool CallBack(IntPtr hwnd, int lParam);

    [Serializable]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [Flags]
    public enum MouseEventFlag : uint //设置鼠标动作的键值
    {
        Move = 0x0001,               //发生移动
        LeftDown = 0x0002,           //鼠标按下左键
        LeftUp = 0x0004,             //鼠标松开左键
        RightDown = 0x0008,          //鼠标按下右键
        RightUp = 0x0010,            //鼠标松开右键
        MiddleDown = 0x0020,         //鼠标按下中键
        MiddleUp = 0x0040,           //鼠标松开中键
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,              //鼠标轮被移动
        VirtualDesk = 0x4000,        //虚拟桌面
        Absolute = 0x8000
    }

    [StructLayout(LayoutKind.Sequential)] //声明键盘钩子的封送结构类型 
    public struct KeyboardHookStruct
    {
        public int vkCode; //表示一个在1到254间的虚似键盘码 
        public int scanCode; //表示硬件扫描码 
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public enum WindowSearch
    {
        GW_HWNDFIRST = 0, //同级别第一个
        GW_HWNDLAST = 1, //同级别最后一个
        GW_HWNDNEXT = 2, //同级别下一个
        GW_HWNDPREV = 3, //同级别上一个
        GW_OWNER = 4, //属主窗口
        GW_CHILD = 5 //子窗口}获取与指定窗口具有指定关系的窗口的句柄 
    }
}
