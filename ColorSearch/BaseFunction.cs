using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
/// <summary>

/// </summary>
namespace ColorSearch
{
    public class BaseFunction
    {
        public static string Coordinate { get; set; }
        private static int Resolution_X = Screen.PrimaryScreen.Bounds.Width;
        private static int Resolution_Y = Screen.PrimaryScreen.Bounds.Height;
        private static Graphics gra;
        public static Bitmap bitmap;
        private static int number = 0;
        #region 直接从屏幕上获取颜色,较耗时，弃用
        /// <summary>
        /// 获取指定窗口的设备场景
        /// </summary>
        /// <param name="hwnd">将获取其设备场景的窗口的句柄。若为0，则要获取整个屏幕的DC</param>
        /// <returns>指定窗口的设备场景句柄，出错则为0</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// 释放由调用GetDC函数获取的指定设备场景
        /// </summary>
        /// <param name="hwnd">要释放的设备场景相关的窗口句柄</param>
        /// <param name="hdc">要释放的设备场景句柄</param>
        /// <returns>执行成功为1，否则为0</returns>
        [DllImport("user32.dll")]
        public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        /// <summary>
        /// 在指定的设备场景中取得一个像素的RGB值
        /// </summary>
        /// <param name="hdc">一个设备场景的句柄</param>
        /// <param name="nXPos">逻辑坐标中要检查的横坐标</param>
        /// <param name="nYPos">逻辑坐标中要检查的纵坐标</param>
        /// <returns>指定点的颜色</returns>
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        static IntPtr hdc = GetDC(IntPtr.Zero);
        private static List<byte> GetColor(int x, int y)
        {
            uint pixel = GetPixel(hdc, x, y);
            List<byte> list = new List<byte>() { };
            list.Add((byte)(pixel & 0x000000FF));
            list.Add((byte)((pixel & 0x0000FF00) >> 8));
            list.Add((byte)((pixel & 0x00FF0000) >> 16));
            return list;
        }

        private static bool IsColorSimilar(Color color1, Color color2, double threshold)
        {
            //Assert.IsTrue(threshold >= 0 && threshold <= 1, "threshold out of range");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (color1.R >= color2.R * threshold
                && color1.R <= color2.R * (2 - threshold)
                && color1.G >= color2.G * threshold
                && color1.G <= color2.G * (2 - threshold)
                && color1.B >= color2.B * threshold
                && color1.B <= color2.B * (2 - threshold))
            {
                return true;
            }
            sw.Stop();
            double a = sw.ElapsedMilliseconds;
            return false;
        }

        private static Color GetColorFromScreen(int x, int y)
        {
            uint pixel = GetPixel(hdc, x, y);
            List<byte> list = new List<byte>() { };
            // Color color = Color.FromRgb((byte)(pixel & 0x000000FF), (byte)((pixel & 0x0000FF00) >> 8), (byte)((pixel & 0x00FF0000) >> 16));
            list.Add((byte)(pixel & 0x000000FF));
            list.Add((byte)((pixel & 0x0000FF00) >> 8));
            list.Add((byte)((pixel & 0x00FF0000) >> 16));
            Color color = Color.FromArgb(Convert.ToInt32((pixel & 0x000000FF)), Convert.ToInt32(((pixel & 0x0000FF00) >> 8)), Convert.ToInt32(((pixel & 0x00FF0000) >> 16)));
            return color;
        }
        private static Color GetColorFromScreen(System.Drawing.Point point)
        {
            try
            {
                int x = point.X;
                int y = point.Y;
                uint pixel = GetPixel(hdc, x, y);
                List<byte> list = new List<byte>() { };
                // Color color = Color.FromRgb((byte)(pixel & 0x000000FF), (byte)((pixel & 0x0000FF00) >> 8), (byte)((pixel & 0x00FF0000) >> 16));
                list.Add((byte)(pixel & 0x000000FF));
                list.Add((byte)((pixel & 0x0000FF00) >> 8));
                list.Add((byte)((pixel & 0x00FF0000) >> 16));
                Color color = Color.FromArgb(Convert.ToInt32((pixel & 0x000000FF)), Convert.ToInt32(((pixel & 0x0000FF00) >> 8)), Convert.ToInt32(((pixel & 0x00FF0000) >> 16)));
                return color;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<List<byte>> GetFullScreenColor()
        {
            List<List<byte>> list = new List<List<byte>>() { };
            for (int x = 0; x <= Resolution_X; x++)
            {
                for (int y = 0; y <= Resolution_Y; y++)
                {
                    list.Add(GetColor(x, y));
                }
            }
            ReleaseDC(IntPtr.Zero, hdc);
            return list;
        }
        #endregion 直接从屏幕上获取颜色
        public static void ClearFolder(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string str in files)
            {
                File.Delete(str);
            }
        }
        public static int GetScreenWidth()
        {
            return Resolution_X;
        }
        public static int GetScreenHeight()
        {
            return Resolution_Y;
        }

        public static Color GetPointColor(int x, int y)
        {
            Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, x, y, x, y);
            return bitScreen.GetPixel(0, 0);
        }

        public static Color GetPointColor(System.Drawing.Point p)
        {
            return GetPointColor(p.X, p.Y);
        }

        #region 截图，然后获取颜色值
        private static Bitmap CaptureScreen(System.Drawing.Point start, System.Drawing.Point end)
        {
            int Height = end.Y - start.Y;
            int Width = end.X - start.X;
            bitmap = new Bitmap(Convert.ToInt16(Width), Convert.ToInt16(Height));
            gra = Graphics.FromImage(bitmap);
            gra.CopyFromScreen(start, new System.Drawing.Point(0, 0), new System.Drawing.Size(Width, Height));
            return bitmap;
        }
        #endregion 截图，然后获取颜色值

        public static void ScalePicture(Bitmap bit, int width, int height)
        {
            bit = (Bitmap)bit.GetThumbnailImage(width, height, null, IntPtr.Zero);
            //bit.Save($@"e:\test\scaled_{number}.png");
        }

        public static List<int> GetNumberPixList(Bitmap bit)
        {
            List<int> list = new List<int> { };
            int width = bit.Width;
            int height = bit.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (bit.GetPixel(i, j).R != 255)
                    {
                        list.Add(1);
                    }
                    else
                    {
                        list.Add(0);
                    }
                }
            }
            return list;
        }
        public static Int64 RecognizeNumber(Bitmap number, List<Bitmap> baselines)
        {
            // number.Save(@"e:\test\recognizeing.png");
            for (int i = 0; i <= 9; i++)
            {
                if (FindPicInSource(number, baselines[i], 0.95))
                {
                    return i;
                }
            }
            return int.MaxValue;
        }
        public static bool CompareTwoBitmaps(Bitmap newItem, Bitmap baseLine)
        {
            if (Math.Abs(newItem.Width - baseLine.Width) > 2) return false;
            if (FindPicInSource(newItem, baseLine, 0.8))
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="canvas"></param>
        /// <param name="percentage"></param>
        /// <param name="accurate">0-1</param>
        /// <returns></returns>
        public static bool FindPicInSource(Bitmap child, Bitmap parent, double threshhold)
        {
            double samePointNum = 0;
            List<int> childList = BaseFunction.GetNumberPixList(child);
            List<int> parentList = BaseFunction.GetNumberPixList(parent);
            if (childList.Count != parentList.Count) return false;
            try
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i] == parentList[i])
                    {
                        samePointNum++;
                    }
                }
            }
            catch { }
            if (samePointNum / Convert.ToDouble(childList.Count) >= threshhold) return true;
            return false;
        }
        private static readonly object locker = new object();
        private static readonly object col_locker = new object();
        private static System.Drawing.Point Result { get; set; }

        private static IntPtr handle;
        public static IntPtr Handle
        {
            get { return handle; }
            set
            {
                handle = (IntPtr)(long)(ulong)(value);
            }
        }

        private static int index = 0;
        private static bool found = false;

        public static System.Drawing.Point FindInArea(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPicPath)
        {
            return FindPicAsync(TopLeftX, TopLeftY, BotRightX, BotRightY, accuracy, targetPicPath);
        }
        public static System.Drawing.Point FindInArea(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPicPath, CancellationToken token)
        {
            return FindPicAsync(TopLeftX, TopLeftY, BotRightX, BotRightY, accuracy, targetPicPath, token);
        }
        private static System.Drawing.Point FindPicAsync(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPicPath)
        {
            found = false;
            Result = new System.Drawing.Point(-1, -1);
            List<Task> tasks = new List<Task>();
            using (Bitmap bmp = new Bitmap(targetPicPath))
            {
                int width = BotRightX - TopLeftX + 1;
                int height = BotRightY - TopLeftY + 1;
                int bmp_width = bmp.Width;
                int bmp_height = bmp.Height;
                double taskNumbers = 0;

                double denominator_vertical = bmp_height;
                double denominator_horizontal = bmp_width;

                double horizontal_parts = Math.Truncate(Convert.ToDouble(width) / Convert.ToDouble(denominator_horizontal));
                double vertical_parts = Math.Truncate(Convert.ToDouble(height) / Convert.ToDouble(denominator_vertical));
                if (horizontal_parts * vertical_parts > 16)
                {
                    denominator_horizontal = height / 4;
                    denominator_vertical = width / 4;
                    taskNumbers = 16;
                }
                else
                    taskNumbers = (horizontal_parts) * (vertical_parts);
                double row = 1, column = 1;
                double left_width;
                double left_height;
                for (double i = 1; i <= taskNumbers; i++)
                {
                    if (i == 1)
                    {
                        left_width = BotRightX - TopLeftX + 1; //区域剩余宽度
                        left_height = BotRightY - TopLeftY + 1;//区域剩余高度
                    }
                    else
                    {
                        left_width = Math.Truncate(BotRightX - TopLeftX + 1 - (column - 1) * denominator_horizontal); //屏幕剩余宽度
                        left_height = Math.Truncate(BotRightY - TopLeftY + 1 - (row - 1) * denominator_vertical);//屏幕剩余高度
                    }

                    if (left_width < bmp_width)
                    {
                        column = 1;
                        row++;
                        i--;
                        continue;
                    }
                    if (left_height < bmp_height)
                    {
                        break;
                    }
                    Action<TransmissionEntity> action = (a) =>
                     {
                         index++;
                         double col, r;
                         col = a.a;
                         r = a.b;
                         int endX = TopLeftX + Convert.ToInt32(col * a.Denominator_horizontal) + 1 + bmp_width - 1;
                         int endY = TopLeftY + Convert.ToInt32(r * a.Denominator_vertical) + 1 + bmp_height - 1;
                         int startX = TopLeftX + Convert.ToInt32((col - 1) * a.Denominator_horizontal);
                         int startY = TopLeftY + Convert.ToInt32((r - 1) * a.Denominator_vertical);
                         if (endX > BotRightX)
                         {
                             endX = BotRightX;
                         }
                         if (endY > BotRightY)
                         {
                             endY = BotRightY;
                         }
                         FindPicInArea(startX, startY,
                            endX, endY, accuracy, targetPicPath);
                     };
                    TransmissionEntity tr = new TransmissionEntity(column, row, denominator_horizontal, denominator_vertical);
                    Task task = Task.Factory.StartNew(() => action(tr));

                    tasks.Add(task);
                    column++;
                }
            }
            Task.WaitAll(tasks.ToArray());
            return Result;
        }
        private static System.Drawing.Point FindPicAsync(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPicPath, CancellationToken token)
        {
            bool tasksDone = false;
            found = false;
            Result = new System.Drawing.Point(-1, -1); List<Task> tasks = new List<Task>();
            using (Bitmap bmp = new Bitmap(targetPicPath))
            {
                int width = BotRightX - TopLeftX + 1;
                int height = BotRightY - TopLeftY + 1;
                int bmp_width = bmp.Width;
                int bmp_height = bmp.Height;
                double taskNumbers = 0;

                double denominator_vertical = bmp_height;
                double denominator_horizontal = bmp_width;

                double horizontal_parts = Math.Truncate(Convert.ToDouble(width) / Convert.ToDouble(denominator_horizontal));
                double vertical_parts = Math.Truncate(Convert.ToDouble(height) / Convert.ToDouble(denominator_vertical));
                if (horizontal_parts * vertical_parts > 16)
                {
                    denominator_horizontal = height / 4;
                    denominator_vertical = width / 4;
                    taskNumbers = 16;
                }
                else
                    taskNumbers = (horizontal_parts) * (vertical_parts);
                double row = 1, column = 1;
                double left_width;
                double left_height;
                for (double i = 1; i <= taskNumbers; i++)
                {
                    if (i == 1)
                    {
                        left_width = BotRightX - TopLeftX + 1; //区域剩余宽度
                        left_height = BotRightY - TopLeftY + 1;//区域剩余高度
                    }
                    else
                    {
                        left_width = Math.Truncate(BotRightX - TopLeftX + 1 - (column - 1) * denominator_horizontal); //屏幕剩余宽度
                        left_height = Math.Truncate(BotRightY - TopLeftY + 1 - (row - 1) * denominator_vertical);//屏幕剩余高度
                    }

                    if (left_width < bmp_width)
                    {
                        column = 1;
                        row++;
                        i--;
                        continue;
                    }
                    if (left_height < bmp_height)
                    {
                        break;
                    }
                    Action<TransmissionEntity> action = (a) =>
                    {
                        index++;
                        double col, r;
                        col = a.a;
                        r = a.b;
                        int endX = TopLeftX + Convert.ToInt32(col * denominator_horizontal) + 1 + bmp_width - 1;
                        int endY = TopLeftY + Convert.ToInt32(r * denominator_vertical) + 1 + bmp_height - 1;
                        int startX = TopLeftX + Convert.ToInt32((col - 1) * denominator_horizontal);
                        int startY = TopLeftY + Convert.ToInt32((r - 1) * denominator_vertical);
                        if (endX > BotRightX)
                        {
                            endX = BotRightX;
                        }
                        if (endY > BotRightY)
                        {
                            endY = BotRightY;
                        }
                        FindPicInArea(startX, startY,
                           endX, endY, accuracy, targetPicPath);
                    };
                    TransmissionEntity tr = new TransmissionEntity(column, row);
                    Task task = Task.Factory.StartNew(() => action(tr), token);
                    tasks.Add(task);
                    column++;
                }
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception e)
            {
                tasksDone = true;
            }
            if (!tasksDone)
            {
                Result = new System.Drawing.Point(-1, -1);
            }
            return Result;
        }

        public static unsafe System.Drawing.Point SearchPictureInBreakPointMode(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, double tolerance, string targetPath, IEnumerable<System.Drawing.Point> exceptions, System.Drawing.Point startAt, int timeout = 0, double times = 1, int step = 0)
        {
            TopLeftX = Convert.ToInt32(TopLeftX * times);
            TopLeftY = Convert.ToInt32(TopLeftY * times);
            BotRightX = Convert.ToInt32(BotRightX * times);
            BotRightY = Convert.ToInt32(BotRightY * times);
            System.Drawing.Point res = new System.Drawing.Point(-1, -1);
            byte[] bmp_rgb;
            byte[] screen_rgb;
            int allowedDiffs = 0;
            int totalpoints = 0;
            int bmp_width = 0;
            int bmp_height = 0;
            int bmp_stride = 0;
            int screen_width = 0;
            int screen_height = 0;
            int screen_stride = 0;
            Stopwatch sw = new Stopwatch();

            int screen_pos = 0;
            int bmp_pos = 0;
            byte b_s = 0;
            byte g_s = 0;
            byte r_s = 0;
            bool match = true;
            int diffs = 0;

            IntPtr p_bmpRgb;
            IntPtr p_screenRgb;

            sw.Start();
            using (Bitmap bmp = new Bitmap(targetPath))
            {
                bmp_width = bmp.Width;
                bmp_height = bmp.Height;
                totalpoints = bmp_width * bmp_height;
                bmp_rgb = BitmapToArray(bmp, out bmp_stride);
                p_bmpRgb = Marshal.UnsafeAddrOfPinnedArrayElement(bmp_rgb, 0);
                allowedDiffs = Convert.ToInt32(Math.Ceiling(accuracy * bmp.Width * bmp.Height));
            }
            fixed (byte* p = bmp_rgb)
            {
                int area_width = BotRightX - TopLeftX + 1;
                int area_height = BotRightY - TopLeftY + 1;
                screen_width = area_width;
                screen_height = area_height;
                for (int h = 0; h < bmp_height; h++)
                {
                    for (int w = 0; w < bmp_width; w++)
                    {
                        bmp_pos = h * bmp_stride + w * 3;
                        byte b_b = Marshal.ReadByte(p_bmpRgb + bmp_pos);
                        byte g_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 1);
                        byte r_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 2);
                        if (b_b == 0 && g_b == 0 && r_b == 0) //忽略纯黑点
                        {
                            totalpoints -= 1;
                            allowedDiffs = Convert.ToInt32(accuracy * Convert.ToDouble(totalpoints));
                        }
                    }
                }
                do
                {
                    Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, TopLeftX, TopLeftY, BotRightX, BotRightY);
                    screen_rgb = BitmapToArray(bitScreen, out screen_stride);
                    //bitScreen.Save(@"C:\Users\Jason\Desktop\default.jpg");
                    p_screenRgb = Marshal.UnsafeAddrOfPinnedArrayElement(screen_rgb, 0);

                    int screen_StartX = -1;
                    int screen_StartY = -1;

                    if (startAt.X == -1)
                    {
                        screen_StartX = 0;
                        screen_StartY = 0;
                    }
                    else
                    {
                        screen_StartX = startAt.X - TopLeftX + bmp_width;
                        screen_StartY = startAt.Y - TopLeftY;
                    }

                    for (int y = screen_StartY; y <= screen_height - bmp_height; y++)
                    {
                        for (int x = screen_StartX; x <= screen_width - bmp_width; x++)
                        {
                            if (ExceptionPointsContains(new System.Drawing.Point(x + TopLeftX, y + TopLeftY), exceptions))
                            {
                                x += bmp_width;
                                continue;
                            }
                            diffs = 0;
                            Coordinate = $"{x + TopLeftX},{y + TopLeftY}";
                            int temp_x = x;
                            int temp_y = y;
                            match = true;
                            for (int h = 0; h < bmp_height; h++)
                            {
                                temp_x = x;
                                for (int w = 0; w < bmp_width; w++)
                                {
                                    screen_pos = temp_y * screen_stride + temp_x * 3;
                                    bmp_pos = h * bmp_stride + w * 3;
                                    byte b_b = Marshal.ReadByte(p_bmpRgb + bmp_pos);
                                    byte g_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 1);
                                    byte r_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 2);

                                    if (b_b == 0 && g_b == 0 && r_b == 0) //忽略纯黑点
                                    {
                                        temp_x++;
                                        continue;
                                    }

                                    b_s = Marshal.ReadByte(p_screenRgb + screen_pos);
                                    g_s = Marshal.ReadByte(p_screenRgb + screen_pos + 1);
                                    r_s = Marshal.ReadByte(p_screenRgb + screen_pos + 2);

                                    if (!IsSameColor(r_b, g_b, b_b, r_s, g_s, b_s, tolerance))
                                    {
                                        diffs++;
                                        if (diffs >= allowedDiffs)
                                        {
                                            w = bmp_width;
                                            h = bmp_height;
                                            match = false;
                                        }
                                    }
                                    temp_x++;
                                }
                                temp_y++;
                            }
                            if (match)
                            {
                                return new System.Drawing.Point(TopLeftX + x, TopLeftY + y);
                            }
                        }
                    }
                } while (timeout != 0 && sw.ElapsedMilliseconds < timeout && res.X == -1);
            }
            sw.Stop();
            return res;
        }

        private static bool ExceptionPointsContains(System.Drawing.Point p, IEnumerable<System.Drawing.Point> exceptPoints)
        {
            if (exceptPoints.Any(
               a => a.X >= p.X - 5 && a.X <= p.X + 5 && a.Y >= p.Y - 5 && a.Y <= p.Y + 5
                ))
            {
                return true;
            }
            return false;
        }
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static byte[] ToGray(Bitmap bmp)
        {
            int totalP = bmp.Width * bmp.Height;
            int sum = 0;
            double average = 0;
            byte[] result = new byte[bmp.Width * bmp.Height];
            int current = 0;
            bmp.Save(@"C:\Users\Jason\Desktop\bmp_gray.jpg");
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    byte gray = (byte)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    bmp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    sum += gray;
                    result[current++] = gray;
                }
            }
            bmp.Save(@"C:\Users\Jason\Desktop\bmp_gray.jpg");
            average = (double)sum / totalP;
            for (int i = 0; i < result.Length; i++)
            {
                byte cur = result[i];
                if (cur > average)
                {
                    result[i] = 1;
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        //public static unsafe Point SearchPicture(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double diffratio, double accuracy, string targetPath, int timeout = 0, double times = 1)
        //{
        //    TopLeftX = Convert.ToInt32(TopLeftX * times);
        //    TopLeftY = Convert.ToInt32(TopLeftY * times);
        //    BotRightX = Convert.ToInt32(BotRightX * times);
        //    BotRightY = Convert.ToInt32(BotRightY * times);
        //    Point res = new Point(-1, -1);
        //    byte[] bmp_rgb;
        //    byte[] screen_rgb;
        //    int allowedDiffs = 0;
        //    int totalpoints = 0;
        //    int bmp_width = 0;
        //    int bmp_height = 0;
        //    int bmp_stride = 0;
        //    int screen_width = 0;
        //    int screen_height = 0;
        //    int screen_stride = 0;
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();

        //    int screen_pos = 0;
        //    int bmp_pos = 0;
        //    byte b_s = 0;
        //    byte g_s = 0;
        //    byte r_s = 0;
        //    bool match = true;
        //    int diffs = 0;

        //    IntPtr p_bmpGray;
        //    IntPtr p_screenGray;

        //    byte[] bmp_gray;

        //    using (Bitmap bmp = new Bitmap(targetPath))
        //    {
        //        bmp_width = bmp.Width;
        //        bmp_height = bmp.Height;
        //        totalpoints = bmp_width * bmp_height;
        //        //bmp_rgb = BitmapToArray(bmp, out bmp_stride);
        //        bmp_gray = ToGray(bmp);
        //        p_bmpGray = Marshal.UnsafeAddrOfPinnedArrayElement(bmp_gray, 0);
        //        allowedDiffs = Convert.ToInt32(Math.Ceiling(diffratio * bmp.Width * bmp.Height));
        //    }
        //    fixed (byte* p = bmp_gray)
        //    {
        //        int area_width = BotRightX - TopLeftX + 1;
        //        int area_height = BotRightY - TopLeftY + 1;
        //        screen_width = area_width;
        //        screen_height = area_height;
        //        for (int i = 0; i < bmp_gray.Length; i++)
        //        {
        //            byte b_b = Marshal.ReadByte(p_bmpGray + i);
        //            //byte g_b = Marshal.ReadByte(p_bmpGray + bmp_pos + 1);
        //            //byte r_b = Marshal.ReadByte(p_bmpGray + bmp_pos + 2);
        //            if (b_b == 0) //忽略纯黑点
        //            {
        //                totalpoints -= 1;
        //                allowedDiffs = Convert.ToInt32(diffratio * Convert.ToDouble(totalpoints));
        //            }

        //        }

        //        do
        //        {
        //            Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, TopLeftX, TopLeftY, BotRightX, BotRightY);
        //            byte[] screen_gray = ToGray(bitScreen);
        //            p_screenGray = Marshal.UnsafeAddrOfPinnedArrayElement(screen_gray, 0);
        //            for (int y = 0; y < screen_height - bmp_height + 1; y++)
        //            {
        //                for (int x = 0; x < screen_width - bmp_width + 1; x++)
        //                {
        //                    diffs = 0;
        //                    match = true;
        //                    int temp_x = x;
        //                    int temp_y = y;
        //                    for (int h = 0; h < bmp_height; h++)
        //                    {
        //                        temp_x = x;
        //                        for (int w = 0; w < bmp_width; w++)
        //                        {
        //                            screen_pos = temp_y * screen_width + temp_x;
        //                            bmp_pos = h * bmp_width + w;
        //                            byte b_b = Marshal.ReadByte(p_bmpGray + bmp_pos);
        //                            b_s = Marshal.ReadByte(p_screenGray + screen_pos);
        //                            if (b_b != b_s)
        //                            {
        //                                diffs++;
        //                                if (diffs >= allowedDiffs)
        //                                {
        //                                    w = bmp_width;
        //                                    h = bmp_height;
        //                                    match = false;
        //                                }
        //                            }
        //                            temp_x++;
        //                        }
        //                        temp_y++;
        //                    }
        //                }
        //            }
        //        } while (timeout != 0 && sw.ElapsedMilliseconds < timeout && res.X == -1);
        //    }
        //    sw.Stop();
        //    return res;
        //}

        public static System.Drawing.Point SearchPicture(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, string targetPath, int timeout = 0, double times = 1)
        {
            OpenCvSharp.Mat srcMat = null;
            OpenCvSharp.Mat dstMat = null;
            OpenCvSharp.OutputArray outArray = null;
            try
            {
                double threshold = 0.95;
                Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, TopLeftX, TopLeftY, BotRightX, BotRightY);
                Mat screenShot = bitScreen.ToMat();


                srcMat = bitScreen.ToMat();
                dstMat = Cv2.ImRead(targetPath, ImreadModes.AnyColor);
                outArray = OpenCvSharp.OutputArray.Create(srcMat);
                OpenCvSharp.Cv2.MatchTemplate(srcMat, dstMat, outArray, TemplateMatchModes.CCorrNormed);
                double minValue, maxValue;
                OpenCvSharp.Point location, point;
                OpenCvSharp.Cv2.MinMaxLoc(OpenCvSharp.InputArray.Create(outArray.GetMat()), out minValue, out maxValue, out location, out point);
                Console.WriteLine(maxValue);
                if (maxValue >= threshold)
                    return new System.Drawing.Point(TopLeftX + point.X, TopLeftY + point.Y);
                return new System.Drawing.Point(-1, -1);
            }
            catch (Exception ex)
            {
                return System.Drawing.Point.Empty;
            }
            finally
            {
                if (srcMat != null)
                    srcMat.Dispose();
                if (dstMat != null)
                    dstMat.Dispose();
                if (outArray != null)
                    outArray.Dispose();
            }
        }

        public static unsafe System.Drawing.Point SearchPicture(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double diffratio, double accuracy, string targetPath, int timeout = 0, double times = 1, bool ignoreBlack = false)
        {
            if (ignoreBlack)
            {
                TopLeftX = Convert.ToInt32(TopLeftX * times);
                TopLeftY = Convert.ToInt32(TopLeftY * times);
                BotRightX = Convert.ToInt32(BotRightX * times);
                BotRightY = Convert.ToInt32(BotRightY * times);
                System.Drawing.Point res = new System.Drawing.Point(-1, -1);
                byte[] bmp_rgb;
                byte[] screen_rgb;
                int allowedDiffs = 0;
                int totalpoints = 0;
                int bmp_width = 0;
                int bmp_height = 0;
                int bmp_stride = 0;
                int screen_width = 0;
                int screen_height = 0;
                int screen_stride = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                int screen_pos = 0;
                int bmp_pos = 0;
                byte b_s = 0;
                byte g_s = 0;
                byte r_s = 0;
                bool match = true;
                int diffs = 0;

                IntPtr p_bmpRgb;
                IntPtr p_screenRgb;

                using (Bitmap bmp = new Bitmap(targetPath))
                {
                    bmp_width = bmp.Width;
                    bmp_height = bmp.Height;
                    totalpoints = bmp_width * bmp_height;
                    bmp_rgb = BitmapToArray(bmp, out bmp_stride);
                    p_bmpRgb = Marshal.UnsafeAddrOfPinnedArrayElement(bmp_rgb, 0);
                    allowedDiffs = Convert.ToInt32(Math.Ceiling(diffratio * bmp.Width * bmp.Height));
                }
                fixed (byte* p = bmp_rgb)
                {
                    int area_width = BotRightX - TopLeftX + 1;
                    int area_height = BotRightY - TopLeftY + 1;
                    screen_width = area_width;
                    screen_height = area_height;
                    for (int h = 0; h < bmp_height; h++)
                    {
                        for (int w = 0; w < bmp_width; w++)
                        {
                            bmp_pos = h * bmp_stride + w * 3;
                            byte b_b = Marshal.ReadByte(p_bmpRgb + bmp_pos);
                            byte g_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 1);
                            byte r_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 2);
                            if (b_b == 0 && g_b == 0 && r_b == 0) //忽略纯黑点
                            {
                                totalpoints -= 1;
                                allowedDiffs = Convert.ToInt32(diffratio * Convert.ToDouble(totalpoints));
                            }
                        }
                    }

                    do
                    {
                        Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, TopLeftX, TopLeftY, BotRightX, BotRightY);
                        screen_rgb = BitmapToArray(bitScreen, out screen_stride);
                        p_screenRgb = Marshal.UnsafeAddrOfPinnedArrayElement(screen_rgb, 0);
                        for (int y = 0; y < screen_height - bmp_height + 1; y++)
                        {
                            for (int x = 0; x < screen_width - bmp_width + 1; x++)
                            {
                                diffs = 0;
                                match = true;
                                int temp_x = x;
                                int temp_y = y;
                                for (int h = 0; h < bmp_height; h++)
                                {
                                    temp_x = x;
                                    for (int w = 0; w < bmp_width; w++)
                                    {
                                        screen_pos = temp_y * screen_stride + temp_x * 3;
                                        bmp_pos = h * bmp_stride + w * 3;
                                        byte b_b = Marshal.ReadByte(p_bmpRgb + bmp_pos);
                                        byte g_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 1);
                                        byte r_b = Marshal.ReadByte(p_bmpRgb + bmp_pos + 2);
                                        if (b_b == 0 && g_b == 0 && r_b == 0) //忽略纯黑点
                                        {
                                            temp_x++;
                                            continue;
                                        }
                                        b_s = Marshal.ReadByte(p_screenRgb + screen_pos);
                                        g_s = Marshal.ReadByte(p_screenRgb + screen_pos + 1);
                                        r_s = Marshal.ReadByte(p_screenRgb + screen_pos + 2);
                                        if (!IsSameColor(r_b, g_b, b_b, r_s, g_s, b_s, accuracy))
                                        {
                                            diffs++;
                                            if (diffs >= allowedDiffs)
                                            {
                                                w = bmp_width;
                                                h = bmp_height;
                                                match = false;
                                            }
                                        }
                                        temp_x++;
                                    }
                                    temp_y++;
                                }
                                if (match)
                                {
                                    return new System.Drawing.Point(TopLeftX + x, TopLeftY + y);
                                }
                            }
                        }
                    } while (timeout != 0 && sw.ElapsedMilliseconds < timeout && res.X == -1);
                }
                sw.Stop();
                return res;
            }
            else
            {
                return SearchPicture(TopLeftX, TopLeftY, BotRightX, BotRightY, targetPath, 0, 1);
            }
        }

        public static System.Drawing.Point SearchPicture(Rectangle rec, double diffratio, double accuracy, string targetPath, int timeout = 0, double times = 1)
        {
            return SearchPicture(rec.X, rec.Y, rec.X + rec.Width - 1, rec.Y + rec.Height - 1, diffratio, accuracy, targetPath, timeout, times);
        }


        //public static Point SearchPicture(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPath, int timeout = 0, double times = 1)
        //{
        //    TopLeftX = Convert.ToInt32(TopLeftX * times);
        //    TopLeftY = Convert.ToInt32(TopLeftY * times);
        //    BotRightX = Convert.ToInt32(BotRightX * times);
        //    BotRightY = Convert.ToInt32(BotRightY * times);
        //    Point res = new Point(-1, -1);
        //    byte[] bmp_rgb;
        //    byte[] screen_rgb;
        //    int allowedDiffs = 0;
        //    double identityPoints = 0;
        //    int totalpoints = 0;
        //    int bmp_width = 0;
        //    int bmp_height = 0;
        //    int bmp_stride = 0;
        //    int screen_width = 0;
        //    int screen_height = 0;
        //    int screen_stride = 0;
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    using (Bitmap bmp = new Bitmap(targetPath))
        //    {
        //        bmp_width = bmp.Width;
        //        bmp_height = bmp.Height;
        //        totalpoints = bmp_width * bmp_height;
        //        bmp_rgb = BitmapToArray(bmp, out bmp_stride);
        //        allowedDiffs = Convert.ToInt32(Math.Ceiling((1 - accuracy) * bmp.Width * bmp.Height));
        //        identityPoints = totalpoints * accuracy;
        //    }
        //    do
        //    {
        //        int area_width = BotRightX - TopLeftX + 1;
        //        int area_height = BotRightY - TopLeftY + 1;
        //        screen_width = area_width;
        //        screen_height = area_height;
        //        using (Bitmap bitScreen = new Bitmap(area_width, area_height, PixelFormat.Format24bppRgb))
        //        {
        //            using (Graphics gra = Graphics.FromImage(bitScreen))
        //            {
        //                gra.CopyFromScreen(TopLeftX, TopLeftY, 0, 0, new Size(area_width, area_height));
        //                screen_rgb = BitmapToArray(bitScreen, out screen_stride);
        //            }
        //        }
        //        int screen_pos = 0;
        //        int bmp_pos = 0;
        //        byte b_s = 0;
        //        byte g_s = 0;
        //        byte r_s = 0;

        //        for (int y = 0; y <= screen_height - bmp_height; y++)
        //        {
        //            for (int x = 0; x <= screen_width - bmp_width; x++)
        //            {
        //                int diffs = 0;
        //                int identities = 0;
        //                int temp_x = x;
        //                int temp_y = y;
        //                for (int h = 0; h < bmp_height; h++)
        //                {
        //                    temp_x = x;
        //                    for (int w = 0; w < bmp_width; w++)
        //                    {
        //                        screen_pos = temp_y * screen_stride + temp_x * 3;
        //                        bmp_pos = h * bmp_stride + w * 3;
        //                        byte b_b = bmp_rgb[bmp_pos];
        //                        byte g_b = bmp_rgb[bmp_pos + 1];
        //                        byte r_b = bmp_rgb[bmp_pos + 2];

        //                        b_s = screen_rgb[screen_pos];
        //                        g_s = screen_rgb[screen_pos + 1];
        //                        r_s = screen_rgb[screen_pos + 2];

        //                        if (!IsSameColor(r_b, g_b, b_b, r_s, g_s, b_s, accuracy))
        //                        {
        //                            diffs++;
        //                            if (diffs >= allowedDiffs)
        //                            {
        //                                h = bmp_height - 1;
        //                                break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            identities++;
        //                        }

        //                        if (identities >= identityPoints || (h == bmp_height - 1 && w == bmp_width - 1 && diffs < allowedDiffs))
        //                        {
        //                            res = new Point(TopLeftX + x, TopLeftY + y);
        //                            return res;
        //                        }
        //                        temp_x++;
        //                    }
        //                    temp_y++;
        //                }
        //            }
        //        }
        //    } while (timeout != 0 && sw.ElapsedMilliseconds < timeout && res.X == -1);
        //    sw.Stop();
        //    return res;
        //}

        public static Image CaptureFromWindow(IntPtr intPtr, int TopLeftX, int TopLeftY, int BotRightX, int BotRightY)
        {
            /// <summary>
            /// Creates an Image object containing a screen shot of a specific window
            /// </summary>
            /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
            /// <returns></returns>
            // get te hDC of the target window
            IntPtr hdcSrc = MyHwnd.GetWindowDC(handle);
            // get the size
            //System.Windows.Rect windowRect = new System.Windows.Rect();
            //MyHwnd.GetWindowRect(handle, out windowRect);
            int width = Convert.ToInt32(BotRightX - TopLeftX + 1);
            int height = Convert.ToInt32(BotRightY - TopLeftY + 1);
            // create a device context we can copy to
            IntPtr hdcDest = MyHwnd.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = MyHwnd.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = MyHwnd.SelectObject(hdcDest, hBitmap);
            // bitblt over
            MyHwnd.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, TopLeftX, TopLeftY, CopyPixelOperation.SourceCopy);
            // restore selection
            MyHwnd.SelectObject(hdcDest, hOld);
            // clean up
            MyHwnd.DeleteDC(hdcDest);
            MyHwnd.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);

            // free up the Bitmap object
            MyHwnd.DeleteObject(hBitmap);

            Bitmap bitScreen = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics gra = Graphics.FromImage(bitScreen);
            gra.DrawImage(img, new System.Drawing.PointF(0, 0));

            return bitScreen;
        }

        public static void ContinuousClick(System.Drawing.Point point, int times)
        {
            for (int i = 1; i <= times; i++)
            {
                MouseOperation.MouseFlag.Click(point);
                Thread.Sleep(20 + new Random().Next(5, 11));
            }

        }

        public static bool PicExists(int topX, int topY, int botX, int botY, double diffratio, double accuracy, string path, bool ignoreBlack = false)
        {
            if (topY < 0) topY = 0;
            var p = SearchPicture(topX, topY, botX, botY, diffratio, accuracy, path, 0, 1, ignoreBlack);
            return p.X != -1;
        }

        public static bool PicExists(Rectangle rectangle, double diffratio, double accuracy, string path)
        {
            return PicExists(rectangle.X, rectangle.Y, rectangle.X + rectangle.Width - 1, rectangle.Y + rectangle.Height - 1, diffratio, accuracy, path);
        }

        public static byte[] BitmapToArray(Bitmap bmp, out int stride)
        {
            Rectangle rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
            int depth = Bitmap.GetPixelFormatSize(bmp.PixelFormat);

            BitmapData bitmapData = bmp.LockBits(rectangle, ImageLockMode.ReadWrite, bmp.PixelFormat);
            // Get the address of the first line.
            int step = depth / 8;

            IntPtr ptr = bitmapData.Scan0;
            stride = Math.Abs(bitmapData.Stride);
            // Declare an array to hold the bytes of the bitmap.
            int bytes = step * bmp.Width * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, rgbValues.Length);
            bmp.UnlockBits(bitmapData);
            return rgbValues;
        }

        private static void FindPicInArea(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPath)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            if (found)
            {
                return;
            }
            if (TopLeftX >= BotRightX || TopLeftY >= BotRightY)
            {
                return;
            }
            //LogService.Log(sw.ElapsedMilliseconds.ToString());
            //sw.Reset(); sw.Start();
            //从上到下找图从左往右找图
            using (Bitmap bmp = new Bitmap(targetPath))
            {
                //LogService.Log(sw.ElapsedMilliseconds.ToString());
                //sw.Reset(); sw.Start();
                int area_width = BotRightX - TopLeftX + 1;
                int area_height = BotRightY - TopLeftY + 1;
                double allowDiffs = 0.1 * bmp.Width * bmp.Height;
                using (Bitmap bitScreen = new Bitmap(area_width, area_height))
                {
                    using (Graphics gra = Graphics.FromImage(bitScreen))
                    {
                        gra.CopyFromScreen(TopLeftX, TopLeftY, 0, 0, new System.Drawing.Size(area_width, area_height));
                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                        //sw.Reset(); sw.Start();
                        //bitScreen.Save($@"E:\VisualStudioProduct-Self Use\SDAuto\SDAuto\Pics\Jie\test.jpg");
                        Color color = new Color();
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        for (int i = 0; i <= area_width - bmp.Width; i++) //指定范围的左右方向
                        {
                            for (int j = 0; j <= area_height - bmp.Height; j++) //指定范围的上下方向
                            {
                                int diff = 0;
                                int identities = 0;
                                for (int x = 0; x < bmp.Width; x++) //目标图片的左右方向
                                {
                                    for (int y = 0; y < bmp.Height; y++) //目标图片的上下方向
                                    {
                                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                                        if (found)
                                        {
                                            return;
                                        }
                                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                                        color = bitScreen.GetPixel(i + x, j + y);
                                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                                        if (!IsSameColor(bmp.GetPixel(x, y), color, accuracy))
                                        {
                                            diff++;
                                            if (diff > allowDiffs)
                                            {
                                                x = bmp.Width - 1;
                                                break;
                                            }
                                        }
                                        else
                                            identities++;
                                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                                        //sw.Reset(); sw.Start();
                                        if ((x == bmp.Width - 1 && y == bmp.Height - 1 && diff <= allowDiffs) || identities >= accuracy * bmp.Width * bmp.Height)
                                        {
                                            if (found)
                                            {
                                                return;
                                            }
                                            Result = new System.Drawing.Point(TopLeftX + i, TopLeftY + j);
                                            found = true;
                                            return;
                                        }
                                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                                        //sw.Reset(); sw.Start();
                                    }
                                }
                            }
                        }
                        //LogService.Log(sw.ElapsedMilliseconds.ToString());
                        sw.Reset();
                    }
                }
            }
        }

        public static System.Drawing.Point FindPic(int TopLeftX, int TopLeftY, int BotRightX, int BotRightY, double accuracy, string targetPicPath)
        {
            //从上到下找图从左往右找图
            System.Drawing.Point point = new System.Drawing.Point();
            Bitmap bitScreen = new Bitmap(BotRightX - TopLeftX + 1, BotRightY - TopLeftY + 1);
            Graphics gra = Graphics.FromImage(bitScreen);
            gra.CopyFromScreen(TopLeftX, TopLeftY, 0, 0, new System.Drawing.Size(BotRightX - TopLeftX + 1, BotRightY - TopLeftY + 1));
            //bitScreen.Save(@"C:\Users\Jason\Desktop\2020-02-Test.jpg");
            //Dictionary<Point, Color> dic = new Dictionary<Point, Color>();
            using (Bitmap bit = new Bitmap(targetPicPath))
            {
                Color color = new Color();
                for (int i = 0; i <= bitScreen.Width - bit.Width; i++) //屏幕的左右方向
                {
                    for (int j = 0; j <= bitScreen.Height - bit.Height; j++) //屏幕的上下方向
                    {
                        for (int x = 0; x < bit.Width; x++) //目标图片的左右方向
                        {
                            for (int y = 0; y < bit.Height; y++) //目标图片的上下方向
                            {
                                color = bitScreen.GetPixel(i + x, j + y);
                                if (!IsSameColor(bit.GetPixel(x, y), color, accuracy))
                                {
                                    x = bit.Width - 1;
                                    break;
                                }
                                if (x == bit.Width - 1 && y == bit.Height - 1)
                                {
                                    point = new System.Drawing.Point(i, j);
                                    //MessageBox.Show("找到");
                                    i = BotRightX;
                                    j = BotRightY;
                                }
                            }
                        }

                    }
                }
            }
            return point;
        }

        private static bool IsSameColor(double baseline, double newValue, double accurate)
        {

            if (newValue >= baseline * accurate && newValue <= baseline * (2 - accurate))
                return true;
            return false;
        }

        private static double ParseRGB(double num)
        {
            if (num > 255)
                num = 255;
            else if (num < 0)
                num = 0;
            return num;
        }

        private static bool IsSameColor(double baseline_r, double baseline_g, double baseline_b, double new_r, double new_g, double new_b, double tolerance)
        {

            Color baseline = Color.FromArgb(Convert.ToInt32(baseline_r), Convert.ToInt32(baseline_g), Convert.ToInt32(baseline_b));
            Color newcolor = Color.FromArgb(Convert.ToInt32(new_r), Convert.ToInt32(new_g), Convert.ToInt32(new_b));
            return IsSameColor(baseline, newcolor, tolerance);
        }

        public static double[] AreaColorPercentage(Color[] colors, Rectangle[] areas, double accuracy)
        {
            double[] result = new double[colors.Length];
            Bitmap[] screenShots = new Bitmap[colors.Length];

            Color color;
            Rectangle area;
            int BotRightX = 0;
            int TopLeftX = 0;
            int BotRightY = 0;
            int TopLeftY = 0;

            for (int i = 0; i < colors.Length; i++)
            {
                color = colors[i];
                area = areas[i];
                BotRightX = area.X + area.Width - 1;
                TopLeftX = area.X;
                BotRightY = area.Y + area.Height - 1;
                TopLeftY = area.Y;
                Bitmap bitScreen = (Bitmap)CaptureFromWindow(Handle, TopLeftX, TopLeftY, BotRightX, BotRightY);
                //screen_rgb = BitmapToArray(bitScreen, out screen_stride);
                screenShots[i] = bitScreen;
            }

            for (int i = 0; i < colors.Length; i++)
            {
                color = colors[i];
                area = areas[i];
                BotRightX = area.X + area.Width - 1;
                TopLeftX = area.X;
                BotRightY = area.Y + area.Height - 1;
                TopLeftY = area.Y;

                double totalPoints = area.Width * area.Height;
                double sameColorPoints = 0;

                //bitScreen.Save(@"C:\Users\Jason\Desktop\default.jpg");
                using (Bitmap bitScreen = screenShots[i])
                {
                    for (int x = 0; x <= area.Width - 1; x++)
                    {
                        for (int y = 0; y <= area.Height - 1; y++)
                        {
                            if (IsSameColor(color, bitScreen.GetPixel(x, y), accuracy))
                            {
                                sameColorPoints++;
                            }
                        }
                    }
                }
                result[i] = sameColorPoints / totalPoints;
                //Bitmap bitScreen = new Bitmap(BotRightX - TopLeftX + 1, BotRightY - TopLeftY + 1);
                //Graphics gra = Graphics.FromImage(bitScreen);
                //gra.CopyFromScreen(TopLeftX, TopLeftY, 0, 0, new Size(BotRightX - TopLeftX + 1, BotRightY - TopLeftY + 1));
                //bitScreen.Save(@"C:\Users\Jason\Desktop\2020-05-99999.jpg");
            }
            return result;
        }

        public static bool IsSameColor(Colors baseline, Colors newColor)
        {
            if (baseline.GetColorType() == newColor.GetColorType())
                return true;
            else
                return false;
        }

        public static HSVColor GetAverageHSV(Rectangle rec)
        {
            Bitmap bitmap = (Bitmap)CaptureFromWindow(Handle, rec.Left, rec.Top, rec.Right, rec.Bottom);
            int totalPoints = bitmap.Width * bitmap.Height;
            double totalH = 0;
            double totalS = 0;
            double totalV = 0;
            double h, s, v;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    ColorToHSV(color, out h, out s, out v);
                    totalH += h;
                    totalS += s;
                    totalV += v;
                }
            }
            return new HSVColor(totalH / totalPoints, totalS / totalPoints, totalV / totalPoints);
        }

        public static double DistanceOf(Color baseline, Color newColor)
        {
            double h = 100;
            double r = 100;
            double hue_baseline, hue_new;
            double saturation_baseline, saturation_new;
            double value_baseline, value_new;
            ColorToHSV(baseline, out hue_baseline, out saturation_baseline, out value_baseline);
            ColorToHSV(newColor, out hue_new, out saturation_new, out value_new);
            double x1 = r * value_baseline * saturation_baseline * Math.Cos(hue_baseline / 180 * Math.PI);
            double y1 = r * value_baseline * saturation_baseline * Math.Sin(hue_baseline / 180 * Math.PI);
            double z1 = h * (1 - value_baseline);
            double x2 = r * value_new * saturation_new * Math.Cos(hue_new / 180 * Math.PI);
            double y2 = r * value_new * saturation_new * Math.Sin(hue_new / 180 * Math.PI);
            double z2 = h * (1 - value_new);
            double dx = x1 - x2;
            double dy = y1 - y2;
            double dz = z1 - z2;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static bool IsSameColor(Color baseline, Color newColor, double accuracy)
        {
            var dis = DistanceOf(baseline, newColor);
            if (dis <= accuracy)
                return true;
            return false;
        }
        public static int GetRedPoints(Bitmap bit)
        {
            int number = 0;
            for (int x = 0; x < bit.Width; x++)
            {
                for (int y = 0; y < bit.Height; y++)
                {
                    if (bit.GetPixel(x, y).R > 91 && bit.GetPixel(x, y).R != 255)
                    {
                        number++;
                    }
                }
            }
            return number;
        }
    }

    public class MouseOperation
    {
        public class MouseFlag
        {
            public static double OffsetX;
            public static double OffsetY;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONUP = 0x0202;
            const int WM_MOUSEMOVE = 0x0200;
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
            enum MouseButtonFlag : uint
            {
                MK_LBUTTON = 1,
                MK_RBUTTON = 2,
                MK_SHIFT = 4,
                MK_CONTROL = 8,
                MK_MBUTTON = 16
            }


            [DllImport("user32.dll", EntryPoint = "SendMessage")]
            public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
            [DllImport("user32.dll")]
            static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

            [DllImport("user32.dll")]
            public static extern int SetCursorPos(int x, int y);
            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);


            public static void MouseMove(System.Drawing.Point moveTo, int offsetX = 0, int offsetY = 0)
            {
                Random rnd = new Random();
                offsetX = rnd.Next(offsetX * -1, offsetX + 1);
                offsetY = rnd.Next(offsetY * -1, offsetY + 1);
                SendMessage(BaseFunction.Handle, WM_MOUSEMOVE, 0, moveTo.X + offsetX + (moveTo.Y + offsetY) * 65536);
                //SetCursorPos(point.X + offsetX, point.Y + offsetY);
            }

            public static void MouseLeftClickGlobal(System.Drawing.Point p)
            {
                Random rnd = new Random();
                int dx = p.X;
                int dy = p.Y;

                Thread.Sleep(rnd.Next(40, 60));
                mouse_event(MouseEventFlag.LeftDown, dx, dy, 0, UIntPtr.Zero);
                mouse_event(MouseEventFlag.LeftUp, dx, dy, 0, UIntPtr.Zero);
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

            public static void Click(System.Drawing.Point p, int offset_X = 10, int offset_Y = 3)
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

    public class KeyBoardOperation
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        private static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public static void SendKeys(Keys key)
        {
            keybd_event(key, 0, 0, 0);
        }
    }

    public class MyHwnd
    {
        const int WM_GETTEXT = 0x000D;
        const int WM_SETTEXT = 0x000C;
        const int WM_CLICK = 0x00F5;
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("user32")]
        public static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumThreadWindows")]
        public static extern int EnumThreadWindows(IntPtr dwThreadId, WaitCallback lpfn, int lParam);

        [DllImport("user32.dll", EntryPoint = "EnumChildWindows")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        public delegate bool CallBack(IntPtr hwnd, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(int hwnd, string lpString);

        [DllImport("user32.dll", EntryPoint = "MoveWindow", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int Width, int Height, bool repaint);

        [DllImport("User32.dll")]
        public static extern bool ClientToScreen(IntPtr hwnd, ref System.Drawing.Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        /// <summary>
        /// 初始化窗口位置，分辨率
        /// </summary>
        /// <param name="WindowName">目标窗口的名称</param>
        /// <param name="hWnd">目标窗口的句柄</param>
        /// <param name="hWndInsertAfter">目标窗口Z轴参数</param>
        /// <param name="x">将窗口移至点P的横坐标</param>
        /// <param name="y">将窗口移至点P的纵坐标</param>
        /// <param name="Width">设置窗口宽度</param>
        /// <param name="Height">设置窗口高度</param>
        /// <param name="flags">其他值</param>
        public static void InitializeWindow(string WindowName, int hWndInsertAfter, int x, int y, int Width, int Height, int flags, bool movewindow = true)
        {
            IntPtr hWnd = FindWindow(null, WindowName);
            var rec = new RECT();
            var windowRec = new RECT();
            GetWindowRect(hWnd, out windowRec);
            Point pWindow = new Point(windowRec.Left, windowRec.Top);
            GetClientRect(hWnd, out rec);
            var pClientLTop = new Point();
            var pClientRBottom = new Point();
            ClientToScreen(hWnd, ref pClientLTop);
            pClientRBottom.X = pClientLTop.X + (rec.Right - rec.Left + 1) - 1;
            pClientRBottom.Y = pClientLTop.Y + (rec.Bottom - rec.Top + 1) - 1;

            double offsetBottomX = windowRec.Right - pClientRBottom.X;
            double offsetBottomY = windowRec.Bottom - pClientRBottom.Y;
            int offsetTopX = pClientLTop.X - windowRec.Left;
            int offsetTopY = pClientLTop.Y - windowRec.Top;

            MouseOperation.MouseFlag.OffsetX = offsetTopX;
            MouseOperation.MouseFlag.OffsetY = offsetTopY;

            int offsetX = Convert.ToInt32(offsetBottomX + offsetTopX);
            int offsetY = Convert.ToInt32(offsetBottomY + offsetTopY);

            if (movewindow && (pClientLTop != Point.Empty || (rec.Right - rec.Left + 1) != Width || (rec.Bottom - rec.Top + 1) != Height)) SetWindowPos(hWnd, hWndInsertAfter, -1 * offsetTopX, -1 * offsetTopY, Width + (int)offsetX, Height + (int)offsetY, flags);
        }

        public static void MoveWindow(string WindowName, int x, int y)
        {
            IntPtr hWnd = FindWindow(null, WindowName);
            var windowRec = new RECT();
            GetWindowRect(hWnd, out windowRec);
            SetWindowPos(hWnd, 0, x, y, windowRec.Right - windowRec.Left + 1, windowRec.Bottom - windowRec.Top + 1, 0);
        }

    }


}


