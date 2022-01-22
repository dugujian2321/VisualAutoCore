using OpenCvSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutomationCore
{
    enum MouseButtonFlag : uint
    {
        MK_LBUTTON = 1,
        MK_RBUTTON = 2,
        MK_SHIFT = 4,
        MK_CONTROL = 8,
        MK_MBUTTON = 16
    }

    /// <summary>
    /// 鼠标后台操作类，部分游戏不支持
    /// </summary>
    public class Mouse_ByMessage : IMouse
    {
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_MOUSEMOVE = 0x0200;

        const int WM_CLOSE = 0x10;

        IntPtr HWND; //目标窗口句柄

        public Mouse_ByMessage(IntPtr hwnd)
        {
            HWND = hwnd;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public void Click(int x, int y)
        {
            Random rnd = new Random();
            Press(MouseKeys.LeftKey, new System.Drawing.Point(x, y));
            Thread.Sleep(rnd.Next(5, 10));
            Release(MouseKeys.LeftKey, new System.Drawing.Point(x, y));
        }
        public void ClickPost(int x, int y)
        {
            Random rnd = new Random();
            PostMessage(HWND, WM_LBUTTONDOWN, (int)MouseButtonFlag.MK_LBUTTON, x + y * 65536);
            Thread.Sleep(rnd.Next(5, 10));
            PostMessage(HWND, WM_LBUTTONUP, 0, x + y * 65536);
        }
        public void MoveTo(int x, int y)
        {
            Random rnd = new Random();
            SendMessage(HWND, WM_MOUSEMOVE, 0, x + y * 65536);
        }

        public void CloseWindow()
        {
            PostMessage(HWND, WM_CLOSE, 0, 0);
        }

        public void Press(MouseKeys key, System.Drawing.Point pressAt)
        {
            int x = pressAt.X;
            int y = pressAt.Y;
            if (key == MouseKeys.LeftKey)
                SendMessage(HWND, WM_LBUTTONDOWN, (int)MouseButtonFlag.MK_LBUTTON, x + y * 65536);
        }

        public void Release(MouseKeys key, System.Drawing.Point releaseAt)
        {
            int x = releaseAt.X;
            int y = releaseAt.Y;
            if (key == MouseKeys.LeftKey)
                SendMessage(HWND, WM_LBUTTONUP, 0, x + y * 65536);
        }
    }
}
