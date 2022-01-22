using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationCore
{
    public class ScancodeMap
    {
        public static Dictionary<char, Scancode> CharScancodeMap = new Dictionary<char, Scancode>();
        public static Dictionary<Keys, Scancode> KeyScancodeMap = new Dictionary<Keys, Scancode>();
        public static Dictionary<char, Keys> Char_KeyMap = new Dictionary<char, Keys>();

        static ScancodeMap()
        {
            CharScancodeMap.Add('A', Scancode.A);
            CharScancodeMap.Add('B', Scancode.B);
            CharScancodeMap.Add('C', Scancode.C);
            CharScancodeMap.Add('D', Scancode.D);
            CharScancodeMap.Add('E', Scancode.E);
            CharScancodeMap.Add('F', Scancode.F);
            CharScancodeMap.Add('G', Scancode.G);
            CharScancodeMap.Add('H', Scancode.H);
            CharScancodeMap.Add('I', Scancode.I);
            CharScancodeMap.Add('J', Scancode.J);
            CharScancodeMap.Add('K', Scancode.K);
            CharScancodeMap.Add('L', Scancode.L);
            CharScancodeMap.Add('M', Scancode.M);
            CharScancodeMap.Add('N', Scancode.N);
            CharScancodeMap.Add('O', Scancode.O);
            CharScancodeMap.Add('P', Scancode.P);
            CharScancodeMap.Add('Q', Scancode.Q);
            CharScancodeMap.Add('R', Scancode.R);
            CharScancodeMap.Add('S', Scancode.S);
            CharScancodeMap.Add('T', Scancode.T);
            CharScancodeMap.Add('U', Scancode.U);
            CharScancodeMap.Add('V', Scancode.V);
            CharScancodeMap.Add('W', Scancode.W);
            CharScancodeMap.Add('X', Scancode.X);
            CharScancodeMap.Add('Y', Scancode.Y);
            CharScancodeMap.Add('Z', Scancode.Z);

            CharScancodeMap.Add('1', Scancode.Num1);
            CharScancodeMap.Add('2', Scancode.Num2);
            CharScancodeMap.Add('3', Scancode.Num3);
            CharScancodeMap.Add('4', Scancode.Num4);
            CharScancodeMap.Add('5', Scancode.Num5);
            CharScancodeMap.Add('6', Scancode.Num6);
            CharScancodeMap.Add('7', Scancode.Num7);
            CharScancodeMap.Add('8', Scancode.Num8);
            CharScancodeMap.Add('9', Scancode.Num9);
            CharScancodeMap.Add('0', Scancode.Num0);


            KeyScancodeMap.Add(Keys.A, Scancode.A);
            KeyScancodeMap.Add(Keys.B, Scancode.B);
            KeyScancodeMap.Add(Keys.C, Scancode.C);
            KeyScancodeMap.Add(Keys.D, Scancode.D);
            KeyScancodeMap.Add(Keys.E, Scancode.E);
            KeyScancodeMap.Add(Keys.F, Scancode.F);
            KeyScancodeMap.Add(Keys.G, Scancode.G);
            KeyScancodeMap.Add(Keys.H, Scancode.H);
            KeyScancodeMap.Add(Keys.I, Scancode.I);
            KeyScancodeMap.Add(Keys.J, Scancode.J);
            KeyScancodeMap.Add(Keys.K, Scancode.K);
            KeyScancodeMap.Add(Keys.L, Scancode.L);
            KeyScancodeMap.Add(Keys.M, Scancode.M);
            KeyScancodeMap.Add(Keys.N, Scancode.N);
            KeyScancodeMap.Add(Keys.O, Scancode.O);
            KeyScancodeMap.Add(Keys.P, Scancode.P);
            KeyScancodeMap.Add(Keys.Q, Scancode.Q);
            KeyScancodeMap.Add(Keys.R, Scancode.R);
            KeyScancodeMap.Add(Keys.S, Scancode.S);
            KeyScancodeMap.Add(Keys.T, Scancode.T);
            KeyScancodeMap.Add(Keys.U, Scancode.U);
            KeyScancodeMap.Add(Keys.V, Scancode.V);
            KeyScancodeMap.Add(Keys.W, Scancode.W);
            KeyScancodeMap.Add(Keys.X, Scancode.X);
            KeyScancodeMap.Add(Keys.Y, Scancode.Y);
            KeyScancodeMap.Add(Keys.Z, Scancode.Z);

            KeyScancodeMap.Add(Keys.D1, Scancode.Num1);
            KeyScancodeMap.Add(Keys.D2, Scancode.Num2);
            KeyScancodeMap.Add(Keys.D3, Scancode.Num3);
            KeyScancodeMap.Add(Keys.D4, Scancode.Num4);
            KeyScancodeMap.Add(Keys.D5, Scancode.Num5);
            KeyScancodeMap.Add(Keys.D6, Scancode.Num6);
            KeyScancodeMap.Add(Keys.D7, Scancode.Num7);
            KeyScancodeMap.Add(Keys.D8, Scancode.Num8);
            KeyScancodeMap.Add(Keys.D9, Scancode.Num9);
            KeyScancodeMap.Add(Keys.D0, Scancode.Num0);


            KeyScancodeMap.Add(Keys.Control, Scancode.Ctrl);
            KeyScancodeMap.Add(Keys.LShiftKey, Scancode.LShift);
            KeyScancodeMap.Add(Keys.RShiftKey, Scancode.RShift);
            KeyScancodeMap.Add(Keys.Alt, Scancode.Alt);
            KeyScancodeMap.Add(Keys.Enter, Scancode.Enter);
            KeyScancodeMap.Add(Keys.Back, Scancode.Backspace);

            Char_KeyMap.Add('a', Keys.A);
            Char_KeyMap.Add('b', Keys.B);
            Char_KeyMap.Add('c', Keys.C);
            Char_KeyMap.Add('d', Keys.D);
            Char_KeyMap.Add('e', Keys.E);
            Char_KeyMap.Add('f', Keys.F);
            Char_KeyMap.Add('g', Keys.G);
            Char_KeyMap.Add('h', Keys.H);
            Char_KeyMap.Add('i', Keys.I);
            Char_KeyMap.Add('j', Keys.J);
            Char_KeyMap.Add('k', Keys.K);
            Char_KeyMap.Add('l', Keys.L);
            Char_KeyMap.Add('m', Keys.M);
            Char_KeyMap.Add('n', Keys.N);
            Char_KeyMap.Add('o', Keys.O);
            Char_KeyMap.Add('p', Keys.P);
            Char_KeyMap.Add('q', Keys.Q);
            Char_KeyMap.Add('r', Keys.R);
            Char_KeyMap.Add('s', Keys.S);
            Char_KeyMap.Add('t', Keys.T);
            Char_KeyMap.Add('u', Keys.U);
            Char_KeyMap.Add('v', Keys.V);
            Char_KeyMap.Add('w', Keys.W);
            Char_KeyMap.Add('x', Keys.X);
            Char_KeyMap.Add('y', Keys.Y);
            Char_KeyMap.Add('z', Keys.Z);

            Char_KeyMap.Add('1', Keys.D1);
            Char_KeyMap.Add('2', Keys.D2);
            Char_KeyMap.Add('3', Keys.D3);
            Char_KeyMap.Add('4', Keys.D4);
            Char_KeyMap.Add('5', Keys.D5);
            Char_KeyMap.Add('6', Keys.D6);
            Char_KeyMap.Add('7', Keys.D7);
            Char_KeyMap.Add('8', Keys.D8);
            Char_KeyMap.Add('9', Keys.D9);
            Char_KeyMap.Add('0', Keys.D0);
        }
    }
    public enum Scancode
    {
        A = 0x1E,
        B = 0x30,
        C = 0x2E,
        D = 0x20,
        E = 0x12,
        F = 0x21,
        G = 0x22,
        H = 0x23,
        I = 0x17,
        J = 0x24,
        K = 0x25,
        L = 0x26,
        M = 0x32,
        N = 0x31,
        O = 0x18,
        P = 0x19,
        Q = 0x10,
        R = 0x13,
        S = 0x1F,
        T = 0x14,
        U = 0x16,
        V = 0x2F,
        W = 0x11,
        X = 0x2D,
        Y = 0x15,
        Z = 0x2C,

        Num1 = 0x02,
        Num2 = 0x03,
        Num3 = 0x04,
        Num4 = 0x05,
        Num5 = 0x06,
        Num6 = 0x07,
        Num7 = 0x08,
        Num8 = 0x09,
        Num9 = 0x0A,
        Num0 = 0x0B,

        Ctrl = 0x1D,
        Alt = 0x38,
        LShift = 0x2A,
        RShift = 0x36,
        Enter = 0x1C,
        Space = 0x39,
        Del = 0xe0 + 0x53,
        Backspace = 0x0e,

        F1 = 0x3B,
        F2 = 0x3C,
        F3 = 0x3D,
        F4 = 0x3E,
        F5 = 0x3F,
        F6 = 0x40,
        F7 = 0x41,
        F8 = 0x42,
        F9 = 0x43,
        F10 = 0x44,

        LEFT = 0x4B,
        RIGHT = 0x4D,
        UP = 0x48,
        DOWN = 0x50,

        /// <summary>
        /// 逗号
        /// </summary>
        COMMA = 0x33,

        /// <summary>
        /// 句号
        /// </summary>
        FULLPOINT = 0x34,
    }
}
