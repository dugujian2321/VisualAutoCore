using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using OpenLibSys;

namespace AutomationCore
{
    public class Keyboard_ByWinRing0 : KeyboardBase, IKeyboard
    {
        Ols ols;
        public Keyboard_ByWinRing0()
        {
            ols = new Ols();
            if (!IsAdmin())
            {
                MessageBox.Show("需使用管理员权限运行");
            }
            else
                InitializeWinRing0();
        }
        ~Keyboard_ByWinRing0()
        {
            if (ols != null)
                ols.Dispose();
        }
        private bool IsAdmin()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Platform == PlatformID.Win32Windows)
            {
                return true;
            }
            else
            {
                WindowsIdentity usrId = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(usrId);
                return p.IsInRole(@"BUILTIN\Administrators");
            }
        }
        private bool CheckSupportLibStatus()
        {
            // Check support library sutatus
            switch (ols.GetStatus())
            {
                case (uint)Ols.Status.NO_ERROR:
                    return true;
                case (uint)Ols.Status.DLL_NOT_FOUND:
                    MessageBox.Show("Status Error!! DLL_NOT_FOUND");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.Status.DLL_INCORRECT_VERSION:
                    MessageBox.Show("Status Error!! DLL_INCORRECT_VERSION");
                    break;
                case (uint)Ols.Status.DLL_INITIALIZE_ERROR:
                    MessageBox.Show("Status Error!! DLL_INITIALIZE_ERROR");
                    break;
            }
            return false;
        }

        private void KBCWait4IBE()
        {
            Int16 dwVal;
            Int16 b = 0x00000002;
            do
            {
                dwVal = ols.ReadIoPortByte(0x64);
            } while ((dwVal & b) != 0);
        }
        private bool CheckWinRing0Status()
        {
            // Check WinRing0 status
            switch (ols.GetDllStatus())
            {
                case (uint)Ols.OlsDllStatus.OLS_DLL_NO_ERROR:
                    return true;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED:
                    MessageBox.Show("DLL Status Error!! OLS_DRIVER_NOT_LOADED");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_UNSUPPORTED_PLATFORM:
                    MessageBox.Show("DLL Status Error!! OLS_UNSUPPORTED_PLATFORM");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_FOUND:
                    MessageBox.Show("DLL Status Error!! OLS_DLL_DRIVER_NOT_FOUND");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_UNLOADED:
                    MessageBox.Show("DLL Status Error!! OLS_DLL_DRIVER_UNLOADED");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK:
                    MessageBox.Show("DLL Status Error!! DRIVER_NOT_LOADED_ON_NETWORK");
                    Environment.Exit(0);
                    break;
                case (uint)Ols.OlsDllStatus.OLS_DLL_UNKNOWN_ERROR:
                    MessageBox.Show("DLL Status Error!! OLS_DLL_UNKNOWN_ERROR");
                    Environment.Exit(0);
                    break;
            }
            return false;
        }

        void InitializeWinRing0()
        {
            if (!CheckSupportLibStatus() || !CheckWinRing0Status())
            {
                return;
            }
        }

        public void Type(Keys[] keys)
        {
            foreach (var key in keys)
            {
                KeyStroke(ScancodeMap.KeyScancodeMap[key]);
            }
        }


        private void KeyStroke(Scancode scancode)
        {
            Random rnd = new Random();
            bool released = false;
            try
            {
                KeyPress(scancode);
                Thread.Sleep(rnd.Next(20, 80));
                KeyRelease(scancode);
                Thread.Sleep(rnd.Next(20, 80));
                released = true;
            }
            catch
            {

            }
            finally
            {
                if (!released)
                {
                    KeyRelease(scancode);
                    Thread.Sleep(rnd.Next(20, 80));
                }
            }
        }

        private void KeyPress(Scancode keyCode)
        {
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xD2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)keyCode);
        }

        private void KeyRelease(Scancode keyCode)
        {
            KBCWait4IBE();
            ols.WriteIoPortByte(0x64, 0xD2);
            KBCWait4IBE();
            ols.WriteIoPortByte(0x60, (byte)(0x80 + (byte)keyCode));
        }

        public void KeyPress(Keys key)
        {
            KeyPress(ScancodeMap.KeyScancodeMap[key]);
        }

        public void KeyRelease(Keys key)
        {
            KeyRelease(ScancodeMap.KeyScancodeMap[key]);
        }

        public bool IsKeyPressed(Keys keys)
        {
            var result = KeyboardBase.GetKeyState((long)MapVirtualKey((byte)ScancodeMap.KeyScancodeMap[keys], 0x0003));
            return result < 0 ? true : false;
        }
    }
}
