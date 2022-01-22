using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ColorSearch;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseFunction.Handle = MyHwnd.FindWindow(null, "神都夜行录");
            Image img = BaseFunction.CaptureFromWindow(BaseFunction.Handle, 919, 223, 1189, 356);
            byte[] screen_rgb;
            int screen_stride = 0;
            using (Bitmap bitScreen = new Bitmap(1189 - 919 + 1, 356 - 223 + 1, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bitScreen))
                {
                    g.DrawImage(img, new PointF(0, 0));
                }
                screen_rgb = BaseFunction.BitmapToArray(bitScreen, out screen_stride);
                //bitScreen.Save(@"C:\Users\Jason\Desktop\default.jpg");
            }
        }

        static void GetControl(IntPtr parent)
        {
            int res = MyHwnd.EnumChildWindows(parent, (h, l) =>
            {

                return true;

            }, 0);
            Console.WriteLine(res);
        }
        static void Calculator(IntPtr parent, int lvl)
        {
            if (parent == IntPtr.Zero) return;
            int text = 0;
            IntPtr child = IntPtr.Zero;
            StringBuilder sb = new StringBuilder();

            child = MyHwnd.FindWindowEx(parent, IntPtr.Zero, null, null);
            if (child != IntPtr.Zero)
            {
                text = MyHwnd.GetWindowText(child, sb, int.MaxValue);
            }
            for (int i = 1; i <= lvl; i++)
            {
                Console.Write("\t");
            }
            Console.WriteLine(sb.ToString() + "," + child);
            Calculator(child, lvl + 1);
        }

        static IntPtr Calculator2(IntPtr hwnd, string lpszWindow, bool bChild)
        {
            IntPtr iResult = IntPtr.Zero;
            // 首先在父窗体上查找控件
            iResult = MyHwnd.FindWindowEx(hwnd, IntPtr.Zero, null, lpszWindow);
            // 如果找到直接返回控件句柄
            if (iResult != IntPtr.Zero) return iResult;

            // 如果设定了不在子窗体中查找
            if (!bChild) return iResult;

            // 枚举子窗体，查找控件句柄
            IntPtr curr = IntPtr.Zero;
            int i = MyHwnd.EnumChildWindows(
            hwnd,
            (h, l) =>
            {
                IntPtr f1 = MyHwnd.FindWindowEx(h, curr, null, null);
                if (f1 == IntPtr.Zero)
                    return true;
                else
                {
                    curr = f1;
                    var res = MyHwnd.FindWindowEx(curr, IntPtr.Zero, null, lpszWindow);

                    if (res == IntPtr.Zero)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            },
            0);
            // 返回查找结果
            return iResult;
        }
        public static bool Test1(IntPtr intPtr, int pram)
        {
            return true;
        }

        public static void C()
        {

        }
    }
}
