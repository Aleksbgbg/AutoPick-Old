namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Windows.Media;

    using AutoPick.Models;

    public class GameImageProcessor : IGameImageProcessor
    {
        private readonly IFromImageConverter _fromImageConverter;

        private readonly IImageHandler[] _imageHandlers;

        public GameImageProcessor(IFromImageConverter fromImageConverter, IImageHandlerFactory imageHandlerFactory)
        {
            _fromImageConverter = fromImageConverter;
            _imageHandlers = imageHandlerFactory.LoadImageHandlers();
        }

        public GameStatusUpdate ProcessGameImage(IImage image)
        {
            foreach (IImageHandler handler in _imageHandlers)
            {
                ImageProcessingResult processingResult = handler.ProcessImage(image);

                if (processingResult.Succeeded)
                {
                    return CreateStatusUpdate(processingResult.GameStatus, processingResult.ResultantImage);
                }
            }

            return CreateStatusUpdate(GameStatus.Idle, image);
        }

        private GameStatusUpdate CreateStatusUpdate(GameStatus gameStatus, IImage image)
        {
            ImageSource imageSource = _fromImageConverter.ImageSourceFrom(image);
            return new GameStatusUpdate(gameStatus, imageSource);
        }
    }
}