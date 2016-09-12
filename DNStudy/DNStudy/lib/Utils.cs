using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNStudy.lib
{
    class Utils
    {
        static public int screenWidth, screenHeight;
        [DllImport("user32.dll", EntryPoint = "keybd_event")]

        public static extern void keybd_event(

        byte bVk, //虚拟键值

        byte bScan,// 一般为0

        int dwFlags, //这里是整数类型 0 为按下，2为释放

        int dwExtraInfo //这里是整数类型 一般情况下设成为 0

        );


        public static void keybd_event(

        byte bVk, //虚拟键值

        String bScan,// 一般为0

        int dwFlags, //这里是整数类型 0 为按下，2为释放

        int dwExtraInfo //这里是整数类型 一般情况下设成为 0

        )
        {
            keybd_event(bVk, Convert.ToByte(bScan, 16), dwFlags, dwExtraInfo);
        }

        static public int x_offset = 0;
        static public int y_offset = -15;

        static public int WM_LBUTTONDOWN = 513; // 鼠标左键按下 
        static public int WM_LBUTTONUP = 514; // 鼠标左键抬起 
        static public int WM_RBUTTONDOWN = 516; // 鼠标右键按下 
        static public int WM_RBUTTONUP = 517; // 鼠标右键抬起 
        static public int WM_MBUTTONDOWN = 519; // 鼠标中键按下 
        static public int WM_MBUTTONUP = 520; // 鼠标中键抬起


        static private readonly int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标移动
        static private readonly int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标左键按下
        static private readonly int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        static private readonly int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        static private readonly int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        static private readonly int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        static private readonly int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 

        static private readonly int MOUSEEVENTF_ABSOLUTE = 0x8001;//鼠标绝对位置
        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void m_l_click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 1, 0);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 1, 0);
            Thread.Sleep(50);
        }

        public static void m_l_doubleclick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 2, 0);
        }

        public static void m_r_click()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 1, 0);
        }

        public static void m_m_doubleclick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 1, 0);
        }

        //public static void m_m_click()
        //{
        //    mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 1, 0);
        //}

        //public static void m_m_doubleclick()
        //{
        //    mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 1, 0);
        //}
        public static void m_moveR(int x, int y)
        {
            mouse_event(MOUSEEVENTF_MOVE, x, y, 0, 0);
        }

        public static void m_moveTo(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE, x * 65535 / screenWidth + x_offset, y * 65535 / screenHeight + y_offset, 0, 0);
            
            //Thread.Sleep(500);
        }

        public static void m_moveTo_l_click(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE, x * 65535 / screenWidth + x_offset, y * 65535 / screenHeight + y_offset, 0, 0);
            Thread.Sleep(800);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 1, 0);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 1, 0);
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);

        public static void sendMouseClick(IntPtr hWnd, int x, int y)
        {
            SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, (IntPtr)(y * 65536 + x));
            Thread.Sleep(10);
            SendMessage(hWnd, WM_LBUTTONUP, (IntPtr)1, (IntPtr)(y * 65536 + x));
        }

        public static void sendMousePress(IntPtr hWnd, int x, int y)
        {
            for (int i = 0; i < 50; i++)
            {
                SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, (IntPtr)(y * 65536 + x));
                Thread.Sleep(45);
            }
            SendMessage(hWnd, WM_LBUTTONUP, (IntPtr)1, (IntPtr)(y * 65536 + x));
        }

        
    }
}
