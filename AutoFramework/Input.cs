using AutomationCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace AutoFramework
{
    public enum KeyboardType
    {
        KeyboardEvent,
        SendInput,
        WingRing0
    }

    public enum MouseType
    {
        SendMessage,
        mouse_event
    }
    public class Input
    {
        static Random rnd = new Random();
        static IKeyboard Keyboard;
        static IMouse Mouse;
        public Input(KeyboardType keyboard, MouseType mouse, IntPtr hwnd)
        {
            switch (keyboard)
            {
                case KeyboardType.KeyboardEvent:
                    Keyboard = new Keyboard_Bykeyboard_event();
                    break;
                case KeyboardType.SendInput:
                    Keyboard = new Keyboard_BySendInput();
                    break;
                case KeyboardType.WingRing0:
                    Keyboard = new Keyboard_ByWinRing0();
                    break;
            }

            switch (mouse)
            {
                case MouseType.mouse_event:
                    Mouse = new Mouse_Bymouse_event();
                    break;
                case MouseType.SendMessage:
                    Mouse = new Mouse_ByMessage(hwnd);
                    break;
            }
        }

        public static void Click(Point p, int offset_X = 5, int offset_Y = 5, int delay = 500)
        {
            Point point = new Point();
            offset_X = rnd.Next(offset_X * -1, offset_X + 1);
            offset_Y = rnd.Next(offset_Y * -1, offset_Y + 1);
            point.X = p.X + offset_X;
            point.Y = p.Y + offset_Y;
            Mouse.MoveTo(point.X, point.Y);
            Thread.Sleep(rnd.Next(30, 80));
            Mouse.Click(point.X, point.Y);
            Thread.Sleep(rnd.Next(50, 100) + delay);
        }

        public static void Click(int x, int y, int offset_X = 5, int offset_Y = 5, int delay = 500)
        {
            Point point = new Point(x, y);
            Click(point, offset_X, offset_Y, delay);
        }

        public static void Press(MouseKeys key, Point p) => Mouse.Press(key, p);
        public static void Release(MouseKeys key, Point p) => Mouse.Release(key, p);

        public static void DragDrop(Point start, Point end, int offset_X = 5, int offset_Y = 5, int delay = 500)
        {
            Point a = new Point();

            int a_offsetX = rnd.Next(offset_X * -1, offset_X + 1);
            int a_offsetY = rnd.Next(offset_Y * -1, offset_Y + 1);
            a.X = start.X + a_offsetX;
            a.Y = start.Y + a_offsetY;

            Point b = new Point();
            int b_offsetX = rnd.Next(offset_X * -1, offset_X + 1);
            int b_offsetY = rnd.Next(offset_Y * -1, offset_Y + 1);
            b.X = end.X + b_offsetX;
            b.Y = end.Y + b_offsetY;

            Mouse.MoveTo(a.X, a.Y);
            Thread.Sleep(rnd.Next(20, 40));
            Mouse.Press(MouseKeys.LeftKey, a);
            Thread.Sleep(rnd.Next(100, 150));
            int x_unit = (b.X - a.X);
            int y_unit = (b.Y - a.Y);
            for (int i = 1; i < 10; i++)
            {
                Mouse.MoveTo(a.X + x_unit * i / 10, a.Y + y_unit * i / 10);
                Thread.Sleep(10);
            }
            Mouse.MoveTo(b.X, b.Y);
            Thread.Sleep(rnd.Next(100, 150));
            Mouse.Release(MouseKeys.LeftKey, b);
            Thread.Sleep(delay + rnd.Next(10, 30));
        }

        public static void MoveTo(int x, int y)
        {
            Mouse.MoveTo(x, y);
        }
        public static void MoveTo(Point p)
        {
            Mouse.MoveTo(p.X, p.Y);
        }

    }
}
