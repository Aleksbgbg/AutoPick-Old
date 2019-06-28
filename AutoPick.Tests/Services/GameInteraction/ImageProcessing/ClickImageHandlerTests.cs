namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class ClickImageHandlerTests
    {
        private readonly Mock<IImage> _imageMock;

        private readonly Mock<IGameWindowClicker> _gameWindowClickerMock;

        private readonly ClickImageHandler _clickImageHandler;

        public ClickImageHandlerTests()
        {
            _gameWindowClickerMock = new Mock<IGameWindowClicker>();

            _imageMock = new Mock<IImage>();

            _clickImageHandler = new ClickImageHandler(_gameWindowClickerMock.Object,
                                                       _imageMock.Object,
                                                       default);
        }

        [Fact]
        public void TestClicksCenter()
        {
            const int x = 500;
            const int y = 1200;
            const int width = 100;
            const int height = 10;
            SetupImageMatch(x, y, width, height);

            ProcessImage();

            VerifyClick(x + (width / 2), y + (height / 2));
        }

        private void SetupImageMatch(int x, int y, int width, int height)
        {
            _imageMock.Setup(image => image.MatchTemplate(It.IsAny<IImage>(), It.IsAny<double>()))
                      .Returns(new TemplateMatchResult(new Rectangle(x, y, width, height)));
        }

        private void ProcessImage()
        {
            _clickImageHandler.ProcessImage(_imageMock.Object);
        }

        private void VerifyClick(int x, int y)
        {
            _gameWindowClickerMock.Verify(clicker => clicker.Click(x, y), Times.Once);
        }
    }
}