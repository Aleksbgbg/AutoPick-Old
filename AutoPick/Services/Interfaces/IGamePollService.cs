namespace AutoPick.Services.Interfaces
{
    using System;

    using AutoPick.Models;

    public interface IGamePollService
    {
        event Action<GameStatusUpdate> GameUpdated;
    }
}