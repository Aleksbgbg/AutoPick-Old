namespace AutoPick.Services.GameInteraction
{
    using System;

    using AutoPick.Models;

    public class GameMonitor : IGameMonitor
    {
        private readonly IThreadRunner _threadRunner;

        private readonly IGameStatusRetriever _gameStatusRetriever;

        public GameMonitor(IThreadRunner threadRunner, IGameStatusRetriever gameStatusRetriever)
        {
            _threadRunner = threadRunner;
            _gameStatusRetriever = gameStatusRetriever;

            ChangeDelay(GameStatus.Offline);
            threadRunner.ThreadAwake += ThreadAwake;
            threadRunner.Start();
        }

        private void ThreadAwake()
        {
            GameStatusUpdate gameStatusUpdate = _gameStatusRetriever.GetCurrentStatus();

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
                    newDelay = 1000;
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