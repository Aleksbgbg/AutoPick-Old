namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System;

    public interface IToImageConverter
    {
        IImage ImageFrom(IntPtr pointer);
    }
}