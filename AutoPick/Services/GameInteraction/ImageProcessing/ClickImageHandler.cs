﻿namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    using AutoPick.Models;

    public class ClickImageHandler : ImageHandlerBase
    {
        private readonly IGameWindowClicker _gameWindowClicker;

        public ClickImageHandler(IGameWindowClicker gameWindowClicker, IImage template, GameStatus gameStatus) : base(template, gameStatus)
        {
            _gameWindowClicker = gameWindowClicker;
        }

        private protected override void TakeAction(Rectangle matchArea)
        {
            _gameWindowClicker.Click(matchArea.X, matchArea.Y);
        }
    }
}