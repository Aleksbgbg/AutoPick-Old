namespace AutoPick.Services.GameInteraction
{
    using System;

    public interface IGameMonitor
    {
        event Action<GameStatusUpdate> GameUpdated;
    }
}