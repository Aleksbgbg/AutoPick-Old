namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Numerics;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class ImageHandlerBaseTests
    {
        private static readonly Vector2 DefaultImageSize = new Vector2(1024, 576);

        private readonly Mock<IImage> _templateMock;

        private Mock<IImage> _imageMock;

        public ImageHandlerBaseTests()
        {
            _templateMock = new Mock<IImage>();
        }

        [Fact]
        public void TestResizesTemplateToMatchImageSize()
        {
            const double scale = 2;
            IImage image = SetupImageSize(DefaultImageSize.X * scale, DefaultImageSize.Y * scale);

            ImageHandlerBase().ProcessImage(image);

            VerifyResize(scale);
        }

        [Fact]
        public void TestDoesntResizeWhenCorrectSize()
        {
            IImage image = SetupImageSize(DefaultImageSize.X, DefaultImageSize.Y);

            ImageHandlerBase().ProcessImage(image);

            VerifyResize(Times.Never);
        }

        [Fact]
        public void TestReturnsTrueResultOnSuccess()
        {
            IImage image = SetupImageMatchesTemplate();
            const GameStatus expectedStatus = GameStatus.PickingLane;

            var processingResult = ImageHandlerBase(expectedStatus).ProcessImage(image);

            Assert.True(processingResult.Succeeded);
            Assert.Equal(expectedStatus, processingResult.GameStatus);
            Assert.Equal(image, processingResult.ResultantImage);
        }

        [Fact]
        public void TestReturnsFalseResultOnFailure()
        {
            IImage image = SetupImageDoesNotMatchTemplate();
            var defaultProcessingResult = new ImageProcessingResult();

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
            return new ImageHandler(_templateMock.Object, gameStatus);
        }

        private IImage SetupImageSize(double x, double y)
        {
            Mock<IImage> imageMock = SetupImageMock(false);

            imageMock.Setup(image => image.Width)
                     .Returns((int)x);

            imageMock.Setup(image => image.Height)
                     .Returns((int)y);

            return imageMock.Object;
        }

        private IImage SetupImageMatchesTemplate()
        {
            return SetupImageMock(true).Object;
        }

        private IImage SetupImageDoesNotMatchTemplate()
        {
            return SetupImageMock(false).Object;
        }

        private IImage SetupImageMatchesTemplateInRectangle(Rectangle rectangle)
        {
            return SetupImageMock(true, rectangle).Object;
        }

        private Mock<IImage> SetupImageMock(bool isMatch)
        {
            return SetupImageMock(isMatch, Rectangle.Empty);
        }

        private Mock<IImage> SetupImageMock(bool isMatch, Rectangle matchRectangle)
        {
            _imageMock = new Mock<IImage>();

            _imageMock.Setup(image => image.MatchTemplate(_templateMock.Object, It.IsInRange(0.75, 0.95, Range.Inclusive)))
                      .Returns(new TemplateMatchResult(isMatch, matchRectangle));

            return _imageMock;
        }

        private void VerifyResize(double scale)
        {
            VerifyResize(scale, Times.Once);
        }

        private void VerifyResize(Func<Times> times)
        {
            VerifyResize(It.IsAny<double>(), times);
        }

        private void VerifyResize(double scale, Func<Times> times)
        {
            _templateMock.Verify(template => template.Resize(scale), times);
        }

        private void VerifyDrawRectangleOnImage(Rectangle rectangle)
        {
            _imageMock.Verify(image => image.Draw(rectangle), Times.Once);
        }

        private class ImageHandler : ImageHandlerBase
        {
            public ImageHandler(IImage template, GameStatus gameStatus) : base(template, gameStatus) { }
        }
    }
}