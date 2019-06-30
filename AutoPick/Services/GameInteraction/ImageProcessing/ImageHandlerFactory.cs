namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.IO;
    using System.Numerics;

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
                new ClickImageHandler(_gameWindowClicker, TemplateFinder("Accept.png", new Vector2(0.46875f, 0.7638888888888888f)), GameStatus.AcceptingMatch), // (480, 440) / (1024, 576)
                new DefaultImageHandler(TemplateFinder("Queue.png", new Vector2(0.8818359375f, 0.06944444444444445f)), GameStatus.Searching),                    // (903, 40) / (1024, 576)
                new DefaultImageHandler(TemplateFinder("Lobby.png", new Vector2(0.900390625f, 0.1267361111111111f)), GameStatus.InLobby)                        // (922, 73) / (1024, 576)
            };
        }

        private TemplateFinder TemplateFinder(string filename, Vector2 normalizedTargetLocation)
        {
            return new TemplateFinder(new EmguCvImage(Path.Combine(_detectionImagePath, filename)), normalizedTargetLocation);
        }
    }
}