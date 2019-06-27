namespace AutoPick.Services.GameInteraction
{
    using System;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    public class GameStatusRetriever : IGameStatusRetriever
    {
        private readonly IGameWindowManager _gameWindowManager;

        private readonly IGameImageProcessor _gameImageProcessor;

        private readonly IToImageConverter _toImageConverter;

        public GameStatusRetriever(IGameWindowManager gameWindowManager, IGameImageProcessor gameImageProcessor, IToImageConverter toImageConverter)
        {
            _gameWindowManager = gameWindowManager;
            _gameImageProcessor = gameImageProcessor;
            _toImageConverter = toImageConverter;
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

            IntPtr imagePointer = _gameWindowManager.CaptureWindow();
            IImage windowImage = _toImageConverter.ImageFrom(imagePointer);

            GameStatusUpdate gameStatusUpdate = _gameImageProcessor.ProcessGameImage(windowImage);

            _gameWindowManager.ReleaseWindowCapture(imagePointer);

            return gameStatusUpdate;
        }
    }
}