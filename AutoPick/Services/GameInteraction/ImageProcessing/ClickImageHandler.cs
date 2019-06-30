namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    using AutoPick.Models;

    public class ClickImageHandler : ImageHandlerBase
    {
        private readonly IGameWindowClicker _gameWindowClicker;

        public ClickImageHandler(IGameWindowClicker gameWindowClicker, ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus)
        {
            _gameWindowClicker = gameWindowClicker;
        }

        private protected override void TakeAction(Rectangle matchArea)
        {
            _gameWindowClicker.Click(matchArea.X + (matchArea.Width / 2),
                                     matchArea.Y + (matchArea.Height / 2));
        }
    }
}