namespace AutoPick.Services
{
    using System;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class GamePoller : IGamePoller
    {
        private readonly IWin32Kit _win32Kit;

        private readonly IGameImageProcessor _gameImageProcessor;

        public GamePoller(IWin32Kit win32Kit, IGameImageProcessor gameImageProcessor)
        {
            _win32Kit = win32Kit;
            _gameImageProcessor = gameImageProcessor;
        }

        public GameStatusUpdate GetCurrentStatus()
        {
            if (!_win32Kit.IsWindowActive())
            {
                return new GameStatusUpdate(GameStatus.Offline, null);
            }

            if (_win32Kit.IsWindowMinimised())
            {
                return new GameStatusUpdate(GameStatus.Minimised, null);
            }

            IntPtr gameImage = _win32Kit.CaptureWindow();

            return _gameImageProcessor.ProcessGameImage(gameImage);
        }
    }
}