namespace AutoPick.Services.Interfaces
{
    using System;

    public interface IWin32Kit
    {
        bool IsWindowActive();

        bool IsWindowMinimised();

        IntPtr CaptureWindow();

        void ReleaseWindowCapture(IntPtr windowCapture);
    }
}