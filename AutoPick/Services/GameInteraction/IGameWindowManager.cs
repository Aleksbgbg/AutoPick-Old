namespace AutoPick.Services.GameInteraction
{
    using System;

    public interface IGameWindowManager
    {
        bool IsWindowActive();

        bool IsWindowMinimised();

        IntPtr CaptureWindow();

        void ReleaseWindowCapture(IntPtr windowCapture);
    }
}