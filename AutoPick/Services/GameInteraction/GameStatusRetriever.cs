namespace AutoPick.Services.GameInteraction
{
    using System;

    using AutoPick.Models;

    public class GameStatusRetriever : IGameStatusRetriever
    {
        private readonly IGameWindowManager _gameWindowManager;

        private readonly IGameImageProcessor _gameImageProcessor;

        public GameStatusRetriever(IGameWindowManager gameWindowManager, IGameImageProcessor gameImageProcessor)
        {
            _gameWindowManager = gameWindowManager;
            _gameImageProcessor = gameImageProcessor;
        }

        public GameStatusUpdate GetCurrentStatus()
        {
            if (!_gameWindowManager.IsWindowActive())
            {
                return new GameStatusUpdate(GameStatus.Offline, null);
            }

            if (_gameWindowManager.IsWindowMinimised())
            {
                return new GameStatusUpdate(GameStatus.Minimised, null);
            }

            IntPtr gameImage = _gameWindowManager.CaptureWindow();

            GameStatusUpdate gameStatusUpdate = _gameImageProcessor.ProcessGameImage(gameImage);

            _gameWindowManager.ReleaseWindowCapture(gameImage);

            return gameStatusUpdate;
        }
    }
}