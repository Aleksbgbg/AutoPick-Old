namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Numerics;

    using AutoPick.Models;

    public class ChampionPickImageHandler : ImageHandlerBase
    {
        private readonly IGameWindowTyper _gameWindowTyper;

        private readonly ITemplateFinder _chatFinder;

        private readonly ITemplateFinder _searchFinder;

        public ChampionPickImageHandler(IGameWindowTyper gameWindowTyper, ITemplateFinder chatFinder, ITemplateFinder searchFinder, ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus)
        {
            _gameWindowTyper = gameWindowTyper;
            _chatFinder = chatFinder;
            _searchFinder = searchFinder;
        }

        public string Champion { private get; set; }

        public string Lane { private get; set; }

        private protected override void TakeAction(Vector2 matchCenter)
        {
            base.TakeAction(matchCenter);
        }
    }
}