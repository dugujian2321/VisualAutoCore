using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace AutomationCore
{
    public class MouseFlag
    {
        public static double OffsetX;
        public static double OffsetY;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_MOUSEMOVE = 0x0200;

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;

        enum MouseVirtualCode //鼠标按键虚拟码
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_MBUTTON = 0x04,
        }

        enum KeyBoardVirtualCode //鼠标按键虚拟码
        {
            VK_A = 0x41,
            VK_F1 = 0x70
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "MapVirtualKey")]
        public static extern long MapVirtualKey(long wCode, long wMapType);

        //消息发送API
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam            // 参数2
        );



        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        public static long MakeKeyLparam(long virtualKey, long flag)
        {
            string s = string.Empty;
            string firstByte = string.Empty;
            if (flag == WM_KEYDOWN)
            {
                firstByte = "00";
            }
            else
            {
                firstByte = "C0";
            }
            long scancode;
            scancode = MapVirtualKey(virtualKey, 0);
            string secondByte = string.Empty;
            string temp = "00" + scancode.ToString("X");
            secondByte = temp.Substring(temp.Length - 2, 2);
            s = firstByte + secondByte + "0001";
            return long.Parse(s, System.Globalization.NumberStyles.HexNumber);
        }

        public static void MouseMove(System.Drawing.Point moveTo, int offsetX = 0, int offsetY = 0)
        {
            Random rnd = new Random();
            offsetX = rnd.Next(offsetX * -1, offsetX + 1);
            offsetY = rnd.Next(offsetY * -1, offsetY + 1);
            SendMessage(BaseFunction.Handle, WM_MOUSEMOVE, 0, moveTo.X + offsetX + (moveTo.Y + offsetY) * 65536);
            //SetCursorPos(point.X + offsetX, point.Y + offsetY);
        }

        private static void MouseLefClickEvent(System.Drawing.Point p, uint data)
        {
            Random rnd = new Random();
            int dx = p.X - (int)OffsetX;
            int dy = p.Y - (int)OffsetY;
            //MouseMove(p);
            //GetCursorPos(ref temp);
            //while (!(temp.X == dx && temp.Y == dy))
            //{
            //    Thread.Sleep(20);
            //    GetCursorPos(ref temp);
            //}
            Thread.Sleep(rnd.Next(40, 60));
            SendMessage(BaseFunction.Handle, WM_LBUTTONDOWN, (int)MouseButtonFlag.MK_LBUTTON, dx + dy * 65536);
            SendMessage(BaseFunction.Handle, WM_LBUTTONUP, 0, dx + dy * 65536);

            ////mouse_event(MouseEventFlag.LeftDown, dx, dy, data, uIntPtr);
            //Thread.Sleep(rnd.Next(80, 110));
            ////mouse_event(MouseEventFlag.LeftUp, dx, dy, data, uIntPtr);
            //Thread.Sleep(rnd.Next(40, 60));
        }

        public static void TypeKey(IntPtr handle)
        {
            PostMessage(handle, WM_KEYDOWN, (int)KeyBoardVirtualCode.VK_F1, (int)MakeKeyLparam((long)KeyBoardVirtualCode.VK_F1, WM_KEYDOWN));
        }

        public static void MouseLefClickEvent(IntPtr handle, System.Drawing.Point p, uint data)
        {
            Random rnd = new Random();
            int dx = p.X - (int)OffsetX;
            int dy = p.Y - (int)OffsetY;
            Thread.Sleep(rnd.Next(40, 60));
            SendMessage(handle, WM_LBUTTONDOWN, (int)MouseButtonFlag.MK_LBUTTON, dx + dy * 65536);
            SendMessage(handle, WM_LBUTTONUP, 0, dx + dy * 65536);
        }

        public static void DragDrop(System.Drawing.Point startP, System.Drawing.Point endP, int speed = 0, uint data = 0, int offsetStartX = 0, int offsetStartY = 0, int offsetEndX = 0, int offsetEndY = 0)
        {
            Random rnd = new Random();
            SendMessage(BaseFunction.Handle, WM_LBUTTONDOWN, (int)MouseButtonFlag.MK_LBUTTON, startP.X + offsetStartX + (startP.Y + offsetStartY) * 65536);
            Thread.Sleep(500);
            SendMessage(BaseFunction.Handle, WM_MOUSEMOVE, (int)MouseButtonFlag.MK_LBUTTON, endP.X + offsetStartX + (endP.Y + offsetStartY) * 65536);
            Thread.Sleep(speed);
            SendMessage(BaseFunction.Handle, WM_LBUTTONUP, (int)MouseButtonFlag.MK_LBUTTON, endP.X + offsetEndX + (endP.Y + offsetEndY) * 65536);
        }

        public static void Click(System.Drawing.Point p, UIntPtr uIntPtr, int offset_X = 10, int offset_Y = 3)
        {
            Random rnd = new Random();
            offset_X = rnd.Next(offset_X * -1, offset_X + 1);
            offset_Y = rnd.Next(offset_Y * -1, offset_Y + 1);
            p.X += offset_X;
            p.Y += offset_Y;
            MouseLefClickEvent(p, 0);
            Thread.Sleep(rnd.Next(50, 150));
        }


    }
}


