namespace AutoPick.Services.Interfaces
{
    using System;

    using AutoPick.Models;

    public interface IGameImageProcessor
    {
        GameStatusUpdate ProcessGameImage(IntPtr image);
    }
}