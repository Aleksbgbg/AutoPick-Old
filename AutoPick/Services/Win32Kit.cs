namespace AutoPick.Services
{
    using System;
    using System.Runtime.InteropServices;

    using AutoPick.Services.Interfaces;

    public class Win32Kit : IWin32Kit, IWindowClicker
    {
        private const string DllName = "AutoPick.Win32.dll";

        private const CallingConvention CallConvention = CallingConvention.Cdecl;

        private const string WindowName = "League of Legends";

        public bool IsWindowActive()
        {
            return IsWindowOpen(WindowName);
        }

        public bool IsWindowMinimised()
        {
            return IsWindowMinimised(WindowName);
        }

        public IntPtr CaptureWindow()
        {
            return CaptureWindow(WindowName);
        }

        public void ReleaseWindowCapture(IntPtr windowCapture)
        {
            CleanUpWindowCapture(windowCapture);
        }

        public void Click(int x, int y)
        {
            Click(WindowName, x, y);
        }

        [DllImport(DllName, CallingConvention = CallConvention)]
        private static extern bool IsWindowOpen(string windowName);

        [DllImport(DllName, CallingConvention = CallConvention)]
        private static extern bool IsWindowMinimised(string windowName);

        [DllImport(DllName, CallingConvention = CallConvention)]
        private static extern IntPtr CaptureWindow(string windowName);

        [DllImport(DllName, CallingConvention = CallConvention)]
        private static extern void CleanUpWindowCapture(IntPtr windowCapture);

        [DllImport(DllName, CallingConvention = CallConvention)]
        private static extern void Click(string windowName, int x, int y);
    }
}