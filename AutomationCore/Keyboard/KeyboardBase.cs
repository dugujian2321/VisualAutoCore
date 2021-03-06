using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutomationCore
{
    public class KeyboardBase
    {
        [DllImport("user32.dll")]
        public static extern uint GetKeyState(long ch);

        /// <summary>
        /// uCode：定义一个键的扫描码或虚拟键码。该值如何解释依赖于uMapType参数的值。
        ///uMapType：定义将要执行的翻译。该参数的值依赖于uCode参数的值。取值如下：
        ///0：代表uCode是一虚拟键码且被翻译为一扫描码。若一虚拟键码不区分左右，则返回左键的扫描码。若未进行翻译，则函数返回O。
        ///1：代表uCode是一扫描码且被翻译为一虚拟键码，且此虚拟键码不区分左右。若未进行翻译，则函数返回0。
        ///2：代表uCode为一虚拟键码且被翻译为一未被移位的字符值存放于返回值的低序字中。死键（发音符号）则通过设置返回值的最高位来表示。若未进行翻译，则函数返回0。
        ///3：代表uCode为一扫描码且被翻译为区分左右键的一虚拟键码。若未进行翻译，则函数返回0。
        ///返回值：返回值可以是一扫描码，或一虚拟键码，或一字符值，这完全依赖于不同的uCode和uMapType的值。若未进行翻译，则函数返回0。
        /// </summary>
        /// <param name="Ucode"></param>
        /// <param name="uMapType"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern byte MapVirtualKey(byte Ucode, long uMapType);
        public enum VKKey
        {
            // mouse movements
            move = 0x0001,
            leftdown = 0x0002,
            leftup = 0x0004,
            rightdown = 0x0008,
            rightup = 0x0010,
            middledown = 0x0020,
            //keyboard stuff
            VK_LBUTTON = 1,
            VK_RBUTTON = 2,
            VK_CANCEL = 3,
            VK_MBUTTON = 4,
            VK_BACK = 8,
            VK_TAB = 9,
            VK_CLEAR = 12,
            VK_RETURN = 13,
            VK_SHIFT = 16,
            VK_CONTROL = 17,
            VK_MENU = 18,
            VK_PAUSE = 19,
            VK_CAPITAL = 20,
            VK_ESCAPE = 27,
            VK_SPACE = 32,
            VK_PRIOR = 33,
            VK_NEXT = 34,
            VK_END = 35,
            VK_HOME = 36,
            VK_LEFT = 37,
            VK_UP = 38,
            VK_RIGHT = 39,
            VK_DOWN = 40,
            VK_SELECT = 41,
            VK_PRINT = 42,
            VK_EXECUTE = 43,
            VK_SNAPSHOT = 44,
            VK_INSERT = 45,
            VK_DELETE = 46,
            VK_HELP = 47,
            VK_NUM0 = 48, //0
            VK_NUM1 = 49, //1
            VK_NUM2 = 50, //2
            VK_NUM3 = 51, //3
            VK_NUM4 = 52, //4
            VK_NUM5 = 53, //5
            VK_NUM6 = 54, //6
            VK_NUM7 = 55, //7
            VK_NUM8 = 56, //8
            VK_NUM9 = 57, //9
            VK_A = 65, //A
            VK_B = 66, //B
            VK_C = 67, //C
            VK_D = 68, //D
            VK_E = 69, //E
            VK_F = 70, //F
            VK_G = 71, //G
            VK_H = 72, //H
            VK_I = 73, //I
            VK_J = 74, //J
            VK_K = 75, //K
            VK_L = 76, //L
            VK_M = 77, //M
            VK_N = 78, //N
            VK_O = 79, //O
            VK_P = 80, //P
            VK_Q = 81, //Q
            VK_R = 82, //R
            VK_S = 83, //S
            VK_T = 84, //T
            VK_U = 85, //U
            VK_V = 86, //V
            VK_W = 87, //W
            VK_X = 88, //X
            VK_Y = 89, //Y
            VK_Z = 90, //Z
            VK_NUMPAD0 = 96, //0
            VK_NUMPAD1 = 97, //1
            VK_NUMPAD2 = 98, //2
            VK_NUMPAD3 = 99, //3
            VK_NUMPAD4 = 100, //4
            VK_NUMPAD5 = 101, //5
            VK_NUMPAD6 = 102, //6
            VK_NUMPAD7 = 103, //7
            VK_NUMPAD8 = 104, //8
            VK_NUMPAD9 = 105, //9
            VK_NULTIPLY = 106,
            VK_ADD = 107,
            VK_SEPARATOR = 108,
            VK_SUBTRACT = 109,
            VK_DECIMAL = 110,
            VK_DIVIDE = 111,
            VK_F1 = 112,
            VK_F2 = 113,
            VK_F3 = 114,
            VK_F4 = 115,
            VK_F5 = 116,
            VK_F6 = 117,
            VK_F7 = 118,
            VK_F8 = 119,
            VK_F9 = 120,
            VK_F10 = 121,
            VK_F11 = 122,
            VK_F12 = 123,
            VK_NUMLOCK = 144,
            VK_SCROLL = 145,
            middleup = 0x0040,
            xdown = 0x0080,
            xup = 0x0100,
            wheel = 0x0800,
            virtualdesk = 0x4000,
            absolute = 0x8000
        }
    }
}
