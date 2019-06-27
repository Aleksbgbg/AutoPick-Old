namespace AutoPick.Services.GameInteraction
{
    using System;

    public interface IGameImageProcessor
    {
        GameStatusUpdate ProcessGameImage(IntPtr image);
    }
}