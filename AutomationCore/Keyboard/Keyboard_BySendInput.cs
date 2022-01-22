using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutomationCore
{
    public unsafe class Keyboard_BySendInput : KeyboardBase, IKeyboard
    {
        const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;

        [DllImport("user32", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern byte VkKeyScan(char ch);



        [DllImport("kernel32.dll")]
        public static extern long GetLastError();

        [DllImport("Kernel32.dll")]
        private static extern int FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out]StringBuilder lpBuffer, uint nSize, IntPtr arguments);

        public void Type(string str)
        {
            SimulateInputString(str);
        }

        public unsafe void SimulateInputString(string sText)
        {
            char[] cText = sText.ToCharArray();
            foreach (var c in sText)
            {
                KeyPress(c);
            }
        }

        private unsafe void KeyPress(char c)
        {
            Random rnd = new Random();
            byte vk = VkKeyScan(c);
            ushort scancode = MapVirtualKey(vk, 0);
            KeyDown(scancode);
            Thread.Sleep(rnd.Next(10, 50));
            KeyUp(scancode);
        }

        private unsafe void KeyDown(ushort scancode)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = 1;
            input[0].ki.dwExtraInfo = IntPtr.Zero;
            //input[0].ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
            input[0].ki.wScan = (ushort)(scancode & 0xff);
            input[0].ki.dwFlags = 0x0000 | 0x0008;//输入scancode
            var line = SendInput(1, input, Marshal.SizeOf(input[0]));
            if (line != 1)
            {
                uint dwFlags = FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER;
                var errCode = GetLastError();
                StringBuilder lpBuffer = new StringBuilder(260); //声明StringBuilder的初始大小

                int count = FormatMessage(dwFlags, IntPtr.Zero, (uint)1436, 0, lpBuffer, 260, IntPtr.Zero);
            }
        }
        private unsafe void KeyUp(ushort scancode)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = 1;
            input[0].ki.dwExtraInfo = IntPtr.Zero;
            //input[0].ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
            input[0].ki.wScan = (ushort)(scancode & 0xff);
            input[0].ki.dwFlags = 0x0002 | 0x0008;
            var line = SendInput(1, input, Marshal.SizeOf(input[0]));
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

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {

            [FieldOffset(0)]
            public int type;

            [FieldOffset(4)]
            public tagKEYBDINPUT ki;

            [FieldOffset(4)]
            public MouseInput mi;

            [FieldOffset(4)]
            public tagHARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public int Mousedata;
            public int dwFlag;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct tagKEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct tagHARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }
    }


}
