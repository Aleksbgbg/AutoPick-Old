namespace AutoPick.Services.Interfaces
{
    using AutoPick.Models;

    public interface IGamePoller
    {
        GameStatusUpdate GetCurrentStatus();
    }
}