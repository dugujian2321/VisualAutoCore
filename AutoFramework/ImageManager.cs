
using AutomationCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using static AutomationCore.MyHwnd;

namespace AutoFramework
{
    public class ImageManager
    {
        public static ImageManager Instance;
        Point pWindow = default;
        IntPtr HWND;
        public ImageManager(IntPtr hwnd)
        {
            HWND = hwnd;
            var windowRec = new RECT();
            GetWindowRect(HWND, out windowRec);
            pWindow = new Point(windowRec.Left, windowRec.Top);
        }
        public Point SearchPicture(Rectangle rec, string targetPath, double threshold = 0.96, int timeout = 0, double times = 1)
        {
            //rec.X = rec.X - pWindow.X;
            //rec.Y = rec.Y - pWindow.Y;
            Point res = new Point();
            res = BaseFunction.SearchPicture(HWND, rec, targetPath, threshold, 0, 1);
            if (res == new Point(-1, -1)) return res;
            //res.X = res.X + pWindow.X;
            //res.Y = res.Y + pWindow.Y;
            return res;
        }
        public Point SearchMaskedPicture(Rectangle rec, Rectangle mask, string targetPath, double threshold = 0.96, int timeout = 0, double times = 1)
        {
            //rec.X = rec.X - pWindow.X;
            //rec.Y = rec.Y - pWindow.Y;
            Point res = new Point();
            res = BaseFunction.SearchPictureWithMask(HWND, rec, mask, targetPath, threshold, 0, 1);
            if (res == new Point(-1, -1)) return res;
            //res.X = res.X + pWindow.X;
            //res.Y = res.Y + pWindow.Y;
            return res;
        }

        public List<Point> ColorCount(Rectangle rectangle, Color color, int allowedError)
        {
            int r_err_lower = color.R - allowedError;
            int r_err_upper = color.R + allowedError;
            int g_err_lower = color.G - allowedError;
            int g_err_upper = color.G + allowedError;
            int b_err_lower = color.B - allowedError;
            int b_err_upper = color.B + allowedError;
            List<Point> result = new List<Point>();
#if DEBUG
            List<Point> pointsForDye = new List<Point>();
#endif
            using (Bitmap screen = (Bitmap)BaseFunction.CaptureFromWindow(HWND, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom))
            {
                for (int i = 0; i < screen.Width; i++)
                {
                    for (int j = 0; j < screen.Height; j++)
                    {
                        Color color1 = screen.GetPixel(i, j);
                        if (IsSameColor(color1, r_err_lower, r_err_upper, g_err_lower, g_err_upper, b_err_lower, b_err_upper))
                        {
#if DEBUG
                            pointsForDye.Add(new Point(i, j));
#endif
                            result.Add(new Point(rectangle.X + i, rectangle.Y + j));
                        }
                    }
                }
#if DEBUG
                //string path = @"E:\VisualStudioProduct-Self Use\SDYXLAutomation\SDAuto\DebugPics\test.jpg";
                //screen.Save(path);
                //DyeImage(path, pointsForDye, Color.Red);
#endif
            }
            return result;
        }


        public bool IsSameColor(Color result, int r_err_lower, int r_err_upper, int g_err_lower, int g_err_upper, int b_err_lower, int b_err_upper)
        {
            if (result.R >= r_err_lower && result.R <= r_err_upper &&
                result.G >= g_err_lower && result.G <= g_err_upper &&
                result.B >= b_err_lower && result.B <= b_err_upper)
            {
                return true;
            }
            return false;
        }

        public bool IsBlackArea(Rectangle rectangle)
        {
            Bitmap screen = (Bitmap)BaseFunction.CaptureFromWindow(HWND, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);

            double totalPixels = rectangle.Width * rectangle.Height * 1.0;
            double blackPixels = 0.0;
            for (int i = 0; i < screen.Width; i++)
            {
                for (int j = 0; j < screen.Height; j++)
                {
                    if (BaseFunction.IsBlack(screen.GetPixel(i, j)))
                    {
                        blackPixels++;
                    }
                }
            }
            screen.Dispose();
            return blackPixels / totalPixels > 0.95;
        }

        public void DyeImage(string imgPath, List<Point> points, Color dyeColor)
        {
            FileInfo file = new FileInfo(imgPath);
            Bitmap bitmap = Bitmap.FromFile(imgPath) as Bitmap;

            foreach (var p in points)
            {
                bitmap.SetPixel(p.X, p.Y, dyeColor);
            }
            //string savePath = Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(imgPath) + "_dyed" + file.Extension);
            //bitmap.Save(savePath);
        }

        public List<Point> FindMostBrightArea(Rectangle rectangle)
        {
            List<Point> result = new List<Point>();
            using (Bitmap bitmap = (Bitmap)BaseFunction.CaptureFromWindow(HWND, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height))
            {
                //bitmap.Save(@"E:\VisualStudioProduct-Self Use\SDYXLAutomation\SDAuto\controlicons\mainarea\screen.jpg");
                for (int i = rectangle.X; i <= rectangle.Right; i++)
                {
                    for (int j = rectangle.Y; j <= rectangle.Bottom; j++)
                    {
                        Point p = new Point(i, j);
                        Color color = BaseFunction.GetColorFromImage(p, bitmap);
                        HSV hsv = BaseFunction.ColorToHSV(color);
                        if (hsv.V > 0.55)
                        {
                            result.Add(p);
                        }
                    }
                }
            }
            return result;
        }

        public Point GetCircleCenter(List<Point> points, int maxDistance)
        {
            try
            {
                double distance = 0;
                int count = points.Count;
                Point p1, p2;
                p1 = p2 = new Point();
                for (int i = 0; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        double tempDis = GetTwoPointsDistance(points[i], points[j]);
                        if (tempDis <= maxDistance && tempDis > distance)
                        {
                            p1 = points[i];
                            p2 = points[j];
                            distance = tempDis;
                        }
                    }
                }
                double x = Convert.ToDouble((p1.X + p2.X) / 2);
                double y = Convert.ToDouble((p1.Y + p2.Y) / 2);
                return new Point((int)x, (int)y);
            }
            catch
            {
                return new Point(-1, -1);
            }
        }

        public double GetTwoPointsDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public bool PointInArea(Point p, Rectangle rectangle)
        {
            return p.X >= rectangle.Location.X && p.X <= rectangle.Right && p.Y >= rectangle.Top && p.Y <= rectangle.Bottom;
        }

        public Point SearchPictureIgnoreBlack(Rectangle rect, double diffratio, double accuracy, string targetPath, int timeout = 0, double times = 1)
        {
            return BaseFunction.SearchPictureIgnoreBlack(HWND, rect, diffratio, accuracy, targetPath, timeout, times);
        }

        public Color GetColorFromScreen(Point point)
        {
            return BaseFunction.GetColorFromScreen(point);
        }
    }
}
