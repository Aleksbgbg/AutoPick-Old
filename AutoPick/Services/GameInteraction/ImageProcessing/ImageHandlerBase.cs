namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    using AutoPick.Models;

    public abstract class ImageHandlerBase : IImageHandler
    {
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
                image.Draw(templateMatchResult.MatchArea);

                TakeAction(templateMatchResult.MatchArea);

                return new ImageProcessingResult(_gameStatus, image);
            }

            return ImageProcessingResult.Failed;
        }

        private protected virtual void TakeAction(Rectangle matchArea)
        {
        }
    }
}