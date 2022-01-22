using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationCore
{
    public interface IKeyboard
    {
        void Type(Keys[] keys);

        void KeyPress(Keys key);

        void KeyRelease(Keys key);

        bool IsKeyPressed(Keys keys);

    }
}
