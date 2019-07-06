namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using AutoPick.Models;
    using AutoPick.Services.GameInteraction.ImageProcessing.ImageHandlers;

    public class GameImageProcessor : IGameImageProcessor
    {
        private readonly IImageHandler[] _imageHandlers;

        public GameImageProcessor(IImageHandlerFactory imageHandlerFactory)
        {
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
            return new GameStatusUpdate(gameStatus, image.ToBitmapImage());
        }
    }
}