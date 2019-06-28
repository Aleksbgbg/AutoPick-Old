namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Numerics;

    using AutoPick.Models;

    public abstract class ImageHandlerBase : IImageHandler
    {
        private const double MatchThreshold = 0.75;

        private static readonly Vector2 DefaultImageSize = new Vector2(1024, 576);

        private readonly IImage _template;

        private readonly GameStatus _gameStatus;

        public ImageHandlerBase(IImage template, GameStatus gameStatus)
        {
            _template = template;
            _gameStatus = gameStatus;
        }

        public ImageProcessingResult ProcessImage(IImage image)
        {
            ResizeTemplateToMatchImageDimensions(image.Width, image.Height);

            TemplateMatchResult result = image.MatchTemplate(_template, MatchThreshold);

            if (result.IsMatch)
            {
                image.Draw(result.MatchArea);

                TakeAction(result.MatchArea);

                return new ImageProcessingResult(true, _gameStatus, image);
            }

            return new ImageProcessingResult();
        }

        private protected virtual void TakeAction(Rectangle matchArea)
        {
        }

        private void ResizeTemplateToMatchImageDimensions(int width, int height)
        {
            float defaultImageLength = DefaultImageSize.Length();
            float actualImageLength = new Vector2(width, height).Length();

            if (Math.Abs(defaultImageLength - actualImageLength) > 0.01)
            {
                _template.Resize(actualImageLength / defaultImageLength);
            }
        }
    }
}