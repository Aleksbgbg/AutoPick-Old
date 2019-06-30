namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class ImageHandlerBaseTests
    {
        private readonly Mock<ITemplateFinder> _templateFinderMock;

        private Mock<IImage> _imageMock;

        public ImageHandlerBaseTests()
        {
            _templateFinderMock = new Mock<ITemplateFinder>();

            _imageMock = new Mock<IImage>();
        }

        [Fact]
        public void TestReturnsTrueResultOnSuccess()
        {
            const GameStatus expectedStatus = GameStatus.PickingLane;
            IImage image = SetupImageMatchesTemplate();

            var processingResult = ImageHandlerBase(expectedStatus).ProcessImage(image);

            Assert.True(processingResult.Succeeded);
            Assert.Equal(expectedStatus, processingResult.GameStatus);
            Assert.Equal(image, processingResult.ResultantImage);
        }

        [Fact]
        public void TestReturnsFalseResultOnFailure()
        {
            IImage image = SetupImageDoesNotMatchTemplate();
            var defaultProcessingResult = ImageProcessingResult.Failed;

            var processingResult = ImageHandlerBase().ProcessImage(image);

            Assert.Equal(defaultProcessingResult.Succeeded, processingResult.Succeeded);
            Assert.Equal(defaultProcessingResult.GameStatus, processingResult.GameStatus);
            Assert.Equal(defaultProcessingResult.ResultantImage, processingResult.ResultantImage);
        }

        [Fact]
        public void TestDrawsMatchRectangleOnMatch()
        {
            Rectangle expectedRectangle = new Rectangle(10, 10, 200, 500);
            IImage image = SetupImageMatchesTemplateInRectangle(expectedRectangle);

            ImageHandlerBase().ProcessImage(image);

            VerifyDrawRectangleOnImage(expectedRectangle);
        }

        private ImageHandlerBase ImageHandlerBase(GameStatus gameStatus = default)
        {
            return new ImageHandler(_templateFinderMock.Object, gameStatus);
        }

        private IImage SetupImageMatchesTemplate()
        {
            return SetupImage(new TemplateMatchResult(default));
        }

        private IImage SetupImageDoesNotMatchTemplate()
        {
            return SetupImage(TemplateMatchResult.Failed);
        }

        private IImage SetupImageMatchesTemplateInRectangle(Rectangle rectangle)
        {
            return SetupImage(new TemplateMatchResult(rectangle));
        }

        private IImage SetupImage(TemplateMatchResult templateMatchResult)
        {
            _imageMock = new Mock<IImage>();

            IImage image = _imageMock.Object;

            _templateFinderMock.Setup(finder => finder.FindTemplateIn(image))
                               .Returns(templateMatchResult);

            return image;
        }

        private void VerifyDrawRectangleOnImage(Rectangle rectangle)
        {
            _imageMock.Verify(image => image.Draw(rectangle), Times.Once);
        }

        private class ImageHandler : ImageHandlerBase
        {
            public ImageHandler(ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus) { }
        }
    }
}