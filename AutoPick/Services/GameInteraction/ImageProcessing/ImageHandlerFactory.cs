namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.IO;
    using System.Numerics;

    using AutoPick.Models;
    using AutoPick.Services.Resources;

    public class ImageHandlerFactory : IImageHandlerFactory
    {
        private readonly string _detectionImagePath;

        private readonly IImageHandler[] _imageHandlers;

        public ImageHandlerFactory(IGameWindowTyper gameWindowTyper, IGameWindowClicker gameWindowClicker, IResourceResolver resourceResolver)
        {
            _detectionImagePath = resourceResolver.ResolveResourcePath(ResourceType.DetectionImages);

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

            var champPickImageHandler = new ChampionPickImageHandler(gameWindowTyper, TemplateFinder("Search.png", new Vector2(0.580078125f, 0.1371527777777778f)), TemplateFinder("Chat.png", new Vector2(0.017578125f, 0.9253472222222222f)), TemplateFinder("Pick.png", new Vector2(0.478515625f, 0.8350694444444444f)), GameStatus.PickingLane);

            SelectedRoleStore = champPickImageHandler;

            _imageHandlers = new IImageHandler[]
            {
                champPickImageHandler,
                new ClickImageHandler(gameWindowClicker, TemplateFinder("Accept.png", new Vector2(0.46875f, 0.7638888888888888f)), GameStatus.AcceptingMatch),
                new DefaultImageHandler(TemplateFinder("Queue.png", new Vector2(0.8818359375f, 0.06944444444444445f)), GameStatus.Searching),
                new DefaultImageHandler(TemplateFinder("Lobby.png", new Vector2(0.900390625f, 0.1267361111111111f)), GameStatus.InLobby)
            };
        }

        public ISelectedRoleStore SelectedRoleStore { get; }

        public IImageHandler[] LoadImageHandlers()
        {
            return _imageHandlers;
        }

        private TemplateFinder TemplateFinder(string filename, Vector2 normalizedTargetLocation)
        {
            return new TemplateFinder(new EmguCvImage(Path.Combine(_detectionImagePath, filename)), normalizedTargetLocation);
        }
    }
}