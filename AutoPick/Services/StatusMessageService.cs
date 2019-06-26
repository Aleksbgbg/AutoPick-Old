namespace AutoPick.Services
{
    using System.Collections.Generic;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class StatusMessageService : IStatusMessageService
    {
        private readonly Dictionary<GameStatus, string> _statusToStringMappings = new Dictionary<GameStatus, string>
        {
            [GameStatus.Offline] = "Game has not been launched",
            [GameStatus.Idle] = "Player is not in a lobby",
            [GameStatus.InLobby] = "Waiting in lobby",
            [GameStatus.Searching] = "Searching for game",
            [GameStatus.AcceptingMatch] = "Accepting game match",
            [GameStatus.ChampionSelect] = "Awaiting game start",
            [GameStatus.PickingLane] = "Picking selected champion role"
        };

        public string ConvertToStatusMessage(GameStatus status)
        {
            return _statusToStringMappings[status];
        }
    }
}