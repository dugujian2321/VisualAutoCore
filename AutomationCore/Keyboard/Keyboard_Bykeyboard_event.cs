using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutomationCore
{

    public class Keyboard_Bykeyboard_event : KeyboardBase, IKeyboard
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);


        public static void SendKey(Keys key)
        {
            Random random = new Random();
            byte v_code = MapVirtualKey((byte)VKKey.VK_N, 0);
            keybd_event((byte)VKKey.VK_N, v_code, 0, 0);
            Thread.Sleep(random.Next(100, 150));
            keybd_event((byte)VKKey.VK_N, v_code, 2, 0);
        }

        public static void SendMultiKey(Keys key1, Keys key2)
        {
            Random random = new Random();
            keybd_event((byte)key1, 0, 0, 0);
            Thread.Sleep(random.Next(10, 50));
            keybd_event((byte)key2, 0, 0, 0);
            Thread.Sleep(random.Next(10, 50));
            keybd_event((byte)key2, 0, 2, 0);
            Thread.Sleep(random.Next(10, 50));
            keybd_event((byte)key1, 0, 2, 0);
        }

        public void Type(string str)
        {
            foreach (char c in str)
            {
                if (c > 'A' && c < 'Z')
                {
                    SendMultiKey(Keys.Shift, ScancodeMap.Char_KeyMap[c]);
                }
                else
                {
                    SendKey(ScancodeMap.Char_KeyMap[c]);
                }
            }
        }

        public void Type(Keys[] keys)
        {
            throw new NotImplementedException();
        }

        public void KeyPress(Keys key)
        {
            throw new NotImplementedException();
        }

        public void KeyRelease(Keys key)
        {
            throw new NotImplementedException();
        }

        public bool IsKeyPressed(Keys keys)
        {
            throw new NotImplementedException();
        }
    }
}
