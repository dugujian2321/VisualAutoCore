using AutomationCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AutoFramework
{
    public static class WindowOperation
    {
        public static void MoveWindow(string windowName, int x, int y)
        {
            MyHwnd.MoveWindow(windowName, x, y);
        }
        public static void MoveWindow(IntPtr HWND, int x, int y, int width, int height)
        {
            MyHwnd.MoveWindow(HWND, x, y, width, height);
        }

        public static void InitializeWindow(string name, int hwndAfter, int x, int y, int width, int height, int flags, bool movewindow = true)
        {
            MyHwnd.InitializeWindow(name, hwndAfter, x, y, width, height, flags, movewindow);
        }
        public static void InitializeWindow(IntPtr hwnd, int hwndAfter, int x, int y, int width, int height, int flags, bool movewindow = true)
        {
            MyHwnd.InitializeWindow(hwnd, hwndAfter, x, y, width, height, flags, movewindow);
        }
        public static IntPtr FindWindow(string lpclassname, string caption)
        {
            return MyHwnd.FindWindow(lpclassname, caption);
        }

        public static IntPtr FindWindow(IntPtr parent, IntPtr child, string lpclassname, string caption)
        {
            return MyHwnd.FindWindowEx(parent, child, lpclassname, caption);
        }


        public static void MoveChildWindow(string parentWinText, string targetWinText, int ToX, int ToY, int width, int height)
        {
            IntPtr parent = FindWindow(null, parentWinText);
            IntPtr child = FindSpringWindow(parent, targetWinText);
            MoveWindow(child, ToX, ToY, width, height);
        }

        public static IntPtr FindSpringWindow(IntPtr source, string windowName)
        {
            List<IntPtr> childWins = new List<IntPtr>();
            IntPtr result = IntPtr.Zero;
            result = FindWindow(source, IntPtr.Zero, null, null);
            while (result != IntPtr.Zero)
            {
                childWins.Add(result);
                StringBuilder title = new StringBuilder();
                var c = MyHwnd.GetWindowText(result, title, 256);
                if (c > 0 && title.ToString() == windowName)
                {
                    return result;
                }
                else
                {
                    result = FindWindow(source, result, null, null);
                }
            }
            foreach (var child in childWins)
            {
                result = FindSpringWindow(child, windowName);
                if (result != IntPtr.Zero) break;
            }
            return result;
        }
    }
}
