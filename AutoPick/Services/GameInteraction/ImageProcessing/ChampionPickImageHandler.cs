namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Diagnostics;
    using System.Numerics;

    using AutoPick.Extensions;
    using AutoPick.Models;

    public class ChampionPickImageHandler : ImageHandlerBase
    {
        private readonly IGameWindowTyper _gameWindowTyper;

        private readonly ITemplateFinder _chatFinder;

        private readonly ITemplateFinder _searchFinder;

        public ChampionPickImageHandler(IGameWindowTyper gameWindowTyper, ITemplateFinder searchFinder, ITemplateFinder chatFinder, ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus)
        {
            _gameWindowTyper = gameWindowTyper;
            _chatFinder = chatFinder;
            _searchFinder = searchFinder;
        }

        public string Champion { private get; set; }

        public string Lane { private get; set; }

        private protected override void TakeAction(Vector2 matchCenter)
        {
            EnterLaneInChat();
            SearchChampion();
        }

        private void EnterLaneInChat()
        {
            TemplateMatchResult chatMatchResult = _chatFinder.FindTemplateIn(Image);

            Debug.Assert(chatMatchResult.IsMatch);

            Vector2 chatCenter = chatMatchResult.MatchArea.Center();
            _gameWindowTyper.TypeAt((int)chatCenter.X, (int)chatCenter.Y, Lane);
            _gameWindowTyper.PressEnter();
        }

        private void SearchChampion()
        {
            TemplateMatchResult championSearchMatchResult = _searchFinder.FindTemplateIn(Image);

            Debug.Assert(championSearchMatchResult.IsMatch);

            Vector2 championSearchCenter = championSearchMatchResult.MatchArea.Center();
            _gameWindowTyper.TypeAt((int)championSearchCenter.X, (int)championSearchCenter.Y, Champion);
        }
    }
}