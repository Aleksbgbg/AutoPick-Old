namespace AutoPick.Services.GameInteraction.ImageProcessing.ImageHandlers
{
    using System.Numerics;

    using AutoPick.Models;

    public class ClickImageHandler : ImageHandlerBase
    {
        private readonly IGameWindowClicker _gameWindowClicker;

        public ClickImageHandler(IGameWindowClicker gameWindowClicker, ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus)
        {
            _gameWindowClicker = gameWindowClicker;
        }

        private protected override void TakeAction(Vector2 matchCenter)
        {
            _gameWindowClicker.Click((int)matchCenter.X, (int)matchCenter.Y);
        }
    }
}