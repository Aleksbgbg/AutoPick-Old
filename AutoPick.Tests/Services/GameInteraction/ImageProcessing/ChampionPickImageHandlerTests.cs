namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;
    using System.Numerics;

    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class ChampionPickImageHandlerTests
    {
        private readonly Mock<IGameWindowTyper> _gameWindowTyperMock;

        private readonly Mock<ITemplateFinder> _chatFinderMock;

        private readonly Mock<ITemplateFinder> _searchFinderMock;

        private readonly Mock<ITemplateFinder> _lockFinderMock;

        private IImage _image;

        public ChampionPickImageHandlerTests()
        {
            _gameWindowTyperMock = new Mock<IGameWindowTyper>();

            _chatFinderMock = new Mock<ITemplateFinder>();

            _searchFinderMock = new Mock<ITemplateFinder>();

            _lockFinderMock = new Mock<ITemplateFinder>();
        }

        [Fact]
        public void TestEntersLaneIntoChat()
        {
            Vector2 chatPosition = new Vector2(50f, 70f);
            const string lane = "Mid";
            SetupFindChat(chatPosition);

            ChampionPickImageHandler(lane: lane).ProcessImage(_image);

            VerifyTypeAt(chatPosition, lane);
            VerifyPressEnter();
        }

        [Fact]
        public void TestSearchesChampion()
        {
            Vector2 championSearchPosition = new Vector2(150f, 220f);
            const string champion = "Katarina";
            SetupFindChampionSearch(championSearchPosition);

            ChampionPickImageHandler(champion: champion).ProcessImage(_image);

            VerifyTypeAt(championSearchPosition, champion);
        }

        private ChampionPickImageHandler ChampionPickImageHandler(string champion = default, string lane = default)
        {
            var handler = new ChampionPickImageHandler(_gameWindowTyperMock.Object, _chatFinderMock.Object, _searchFinderMock.Object, _lockFinderMock.Object, default);
            handler.Champion = champion;
            handler.Lane = lane;

            return handler;
        }

        private void SetupFindChat(Vector2 position)
        {
            SetupImage();

            SetupFindTemplate(_lockFinderMock, default);
            SetupFindTemplate(_chatFinderMock, position);
            SetupFindTemplate(_searchFinderMock, default);
        }

        private void SetupFindChampionSearch(Vector2 position)
        {
            SetupImage();

            SetupFindTemplate(_lockFinderMock, default);
            SetupFindTemplate(_chatFinderMock, default);
            SetupFindTemplate(_searchFinderMock, position);
        }

        private void SetupImage()
        {
            _image = new Mock<IImage>().Object;
        }

        private void SetupFindTemplate(Mock<ITemplateFinder> templateFinderMock, Vector2 position)
        {
            templateFinderMock.Setup(finder => finder.FindTemplateIn(_image))
                              .Returns(new TemplateMatchResult(new Rectangle((int)position.X, (int)position.Y, 0, 0)));
        }

        private void VerifyTypeAt(Vector2 position, string text)
        {
            _gameWindowTyperMock.Verify(typer => typer.TypeAt((int)position.X, (int)position.Y, text), Times.Once);
        }

        private void VerifyPressEnter()
        {
            _gameWindowTyperMock.Verify(typer => typer.PressEnter(), Times.Once);
        }
    }
}