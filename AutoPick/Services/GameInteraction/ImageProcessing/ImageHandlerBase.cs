namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;
    using System.Numerics;

    using AutoPick.Models;

    public abstract class ImageHandlerBase : IImageHandler
    {
        private const int BorderMargin = 3;

        private readonly ITemplateFinder _templateFinder;

        private readonly GameStatus _gameStatus;

        public ImageHandlerBase(ITemplateFinder templateFinder, GameStatus gameStatus)
        {
            _templateFinder = templateFinder;
            _gameStatus = gameStatus;
        }

        public ImageProcessingResult ProcessImage(IImage image)
        {
            TemplateMatchResult templateMatchResult = _templateFinder.FindTemplateIn(image);

            if (templateMatchResult.IsMatch)
            {
                Rectangle matchBorder = new Rectangle(templateMatchResult.MatchArea.Location,
                                                      templateMatchResult.MatchArea.Size);
                matchBorder.Inflate(BorderMargin, BorderMargin);
                image.Draw(matchBorder);

                TakeAction(new Vector2(templateMatchResult.MatchArea.X + (templateMatchResult.MatchArea.Width / 2),
                                       templateMatchResult.MatchArea.Y + (templateMatchResult.MatchArea.Height / 2)));

                return new ImageProcessingResult(_gameStatus, image);
            }

            return ImageProcessingResult.Failed;
        }

        private protected virtual void TakeAction(Vector2 matchCenter)
        {
        }
    }
}