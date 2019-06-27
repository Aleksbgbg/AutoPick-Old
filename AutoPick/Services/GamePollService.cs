namespace AutoPick.Services
{
    using System;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class GamePollService : IGamePollService
    {
        private readonly IThreadRunner _threadRunner;

        private readonly IGamePoller _gamePoller;

        public GamePollService(IThreadRunner threadRunner, IGamePoller gamePoller)
        {
            _threadRunner = threadRunner;
            _gamePoller = gamePoller;

            ChangeDelay(GameStatus.Offline);
            threadRunner.ThreadAwake += ThreadAwake;
            threadRunner.Start();
        }

        private void ThreadAwake()
        {
            GameStatusUpdate gameStatusUpdate = _gamePoller.GetCurrentStatus();

            GameUpdated?.Invoke(gameStatusUpdate);

            ChangeDelay(gameStatusUpdate.GameStatus);
        }

        public event Action<GameStatusUpdate> GameUpdated;

        private void ChangeDelay(GameStatus gameStatus)
        {
            int newDelay;

            switch (gameStatus)
            {
                case GameStatus.Offline:
                    newDelay = 2000;
                    break;

                case GameStatus.Minimised:
                    newDelay = 1000;
                    break;

                case GameStatus.Idle:
                case GameStatus.InLobby:
                case GameStatus.Searching:
                case GameStatus.ChampionSelect:
                    newDelay = 5000;
                    break;

                case GameStatus.AcceptingMatch:
                case GameStatus.PickingLane:
                    newDelay = 100;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            _threadRunner.ChangeDelay(newDelay);
        }
    }
}