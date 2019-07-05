namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.IO;
    using System.Numerics;

    using AutoPick.Models;
    using AutoPick.Services.Resources;

    public class ImageHandlerFactory : IImageHandlerFactory
    {
        private readonly IGameWindowTyper _gameWindowTyper;

        private readonly IGameWindowClicker _gameWindowClicker;

        private readonly string _detectionImagePath;

        public ImageHandlerFactory(IGameWindowTyper gameWindowTyper, IGameWindowClicker gameWindowClicker, IResourceResolver resourceResolver)
        {
            _gameWindowTyper = gameWindowTyper;
            _gameWindowClicker = gameWindowClicker;
            _detectionImagePath = resourceResolver.ResolveResourcePath(ResourceType.DetectionImages);
        }

        public IImageHandler[] LoadImageHandlers()
        {
            // Image original coordinates: (all are relative and normalized to (1024, 576))
            // ChampionPickImageHandler
            // - Chat:   (18, 533)  -> (0.017578125f, 0.9253472222222222f)
            // - Search: (594, 79)  -> (0.580078125f, 0.1371527777777778f)
            // - Pick:   (490, 481) -> (0.478515625f, 0.8350694444444444f)
            // ClickImageHandler
            // - Accept: (480, 440) -> (0.46875f, 0.7638888888888888f)
            // QueueImageHandler
            // - Queue:  (903, 40)  -> (0.8818359375f, 0.06944444444444445f)
            // LobbyImageHandler
            // - Lobby:  (922, 73)  -> (0.900390625f, 0.1267361111111111f)

            return new IImageHandler[]
            {
                new ChampionPickImageHandler(_gameWindowTyper, TemplateFinder("Search.png", new Vector2(0.580078125f, 0.1371527777777778f)), TemplateFinder("Chat.png", new Vector2(0.017578125f, 0.9253472222222222f)), TemplateFinder("Pick.png", new Vector2(0.478515625f, 0.8350694444444444f)), GameStatus.PickingLane),
                new ClickImageHandler(_gameWindowClicker, TemplateFinder("Accept.png", new Vector2(0.46875f, 0.7638888888888888f)), GameStatus.AcceptingMatch),
                new DefaultImageHandler(TemplateFinder("Queue.png", new Vector2(0.8818359375f, 0.06944444444444445f)), GameStatus.Searching),
                new DefaultImageHandler(TemplateFinder("Lobby.png", new Vector2(0.900390625f, 0.1267361111111111f)), GameStatus.InLobby)
            };
        }

        private TemplateFinder TemplateFinder(string filename, Vector2 normalizedTargetLocation)
        {
            return new TemplateFinder(new EmguCvImage(Path.Combine(_detectionImagePath, filename)), normalizedTargetLocation);
        }
    }
}