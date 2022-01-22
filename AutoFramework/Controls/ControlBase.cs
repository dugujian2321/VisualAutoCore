using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AutoFramework
{
    public class ControlBase
    {
        public bool IsBlack = false;
        public Random rnd = new Random();
        public Point pNULL = new Point(-1, -1);
        public Image MarkImg = null;

        public ControlBase Parent;

        private Rectangle rect;
        public Rectangle RECT
        {
            get
            {
                return rect;
            }
            set
            {
                rect = value;
                locationP = rect.Location;
            }
        }

        public List<ControlBase> Children = new List<ControlBase>();

        private Point locationP;
        public Point LocationP { get { return locationP; } } //TopLeft corner point coordinate
        public Point pControlCenter
        {
            get
            {
                return new Point(RECT.Location.X + Convert.ToInt32(this.Width / 2), RECT.Location.Y + Convert.ToInt32(this.Height / 2));
            }
        }
        public string ControlPicPath { get; private set; }

        private int w;
        public int Width
        {
            get
            {
                if (RECT != default(Rectangle))
                {
                    return RECT.Width;
                }
                return w;
            }
            set { w = value; }
        }

        private int h;
        public int Height { get { return RECT == default ? h : RECT.Height; } set { h = value; } }

        protected ControlBase() : this(null)
        {

        }

        public ControlBase(ControlBase parent) : this(string.Empty, parent)
        {
        }

        public ControlBase(string srcFile, ControlBase parent)
        {
            ControlPicPath = srcFile;
            Parent = parent;
            //if (parent != null)
            //    parent.Children.Add(this);

            if (File.Exists(srcFile))
            {
                using (MarkImg = Image.FromFile(ControlPicPath))
                {
                    w = MarkImg.Width;
                    h = MarkImg.Height;
                }
            }
        }

        public Point MarkPos
        {
            get
            {
                if (IsBlack)
                {
                    return ImageManager.Instance.SearchPictureIgnoreBlack(RECT, 0.2, 10, ControlPicPath);
                }
                else
                {
                    return ImageManager.Instance.SearchPicture(RECT, ControlPicPath);
                }

            }
        }

        public bool Exist
        {
            get
            {
                Point p;
                if (!IsBlack)
                {
                    p = ImageManager.Instance.SearchPicture(RECT, ControlPicPath);
                }
                else
                {
                    p = ImageManager.Instance.SearchPictureIgnoreBlack(RECT, 0.2, 10, ControlPicPath);
                }

                return p != pNULL;
            }
        }
    }
}
