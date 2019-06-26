namespace AutoPick.Services.Interfaces
{
    using AutoPick.Models;

    public interface IStatusMessageService
    {
        string ConvertToStatusMessage(GameStatus status);
    }
}