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

        [Fact]
        public void TestEntersLaneIntoChat()
        {

        }

        private ChampionPickImageHandler ChampionPickImageHandler(string champion = default, string lane = default)
        {
            return new ChampionPickImageHandler(_gameWindowTyperMock.Object, _chatFinderMock.Object, _searchFinderMock.Object, _lockFinderMock.Object, default);
        }

        private void SetupFindChat(Vector2 position)
        {
            _lockFinderMock.Setup(finder => finder.FindTemplateIn(It.IsAny<IImage>()))
                           .Returns(new TemplateMatchResult(default));

            _image = new Mock<IImage>().Object;

            _chatFinderMock.Setup(finder => finder.FindTemplateIn(_image))
                           .Returns(new TemplateMatchResult(new Rectangle()));
        }
    }
}