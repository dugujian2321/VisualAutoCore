using OpenCvSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutomationCore
{
    enum MouseEventFlag : uint
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000,
    }
    /// <summary>
    /// 直接操作鼠标硬件，会与用户发生鼠标使用上的争夺
    /// </summary>
    public class Mouse_Bymouse_event : IMouse
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        public void Click(int x, int y)
        {    
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(40, 60));
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, x, y, 0, UIntPtr.Zero);
        }

        public void MoveTo(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
        }

        public void Press(MouseKeys key, System.Drawing.Point point)
        {
            throw new NotImplementedException();
        }

        public void Release(MouseKeys key, System.Drawing.Point point)
        {
            throw new NotImplementedException();
        }
    }
}
