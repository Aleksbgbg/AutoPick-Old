namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.IO;

    using AutoPick.Models;
    using AutoPick.Services.Resources;

    public class ImageHandlerFactory : IImageHandlerFactory
    {
        private readonly IGameWindowClicker _gameWindowClicker;

        private readonly string _detectionImagePath;

        public ImageHandlerFactory(IGameWindowClicker gameWindowClicker, IResourceResolver resourceResolver)
        {
            _gameWindowClicker = gameWindowClicker;
            _detectionImagePath = resourceResolver.ResolveResourcePath(ResourceType.DetectionImages);
        }

        public IImageHandler[] LoadImageHandlers()
        {
            return new IImageHandler[]
            {
                new ClickImageHandler(_gameWindowClicker, ImageFromFile("Accept.png"), GameStatus.AcceptingMatch),
                new DefaultImageHandler(ImageFromFile("Queue.png"), GameStatus.Searching),
                new DefaultImageHandler(ImageFromFile("Lobby.png"), GameStatus.InLobby)
            };
        }

        private IImage ImageFromFile(string filename)
        {
            return new EmguCvImage(Path.Combine(_detectionImagePath, filename));
        }
    }
}