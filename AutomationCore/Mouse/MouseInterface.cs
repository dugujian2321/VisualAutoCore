using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationCore
{
    public interface IMouse
    {
        void Click(int x, int y);
        void MoveTo(int x, int y);
        void Press(MouseKeys key, System.Drawing.Point point);
        void Release(MouseKeys key, System.Drawing.Point point);
    }

    public enum MouseKeys
    {
        LeftKey,
        RightKey,
        MiddleKey
    }
}
