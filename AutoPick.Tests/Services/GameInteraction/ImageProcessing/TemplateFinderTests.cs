namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Linq.Expressions;
    using System.Numerics;

    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class TemplateFinderTests
    {
        private const int MatchMargin = 2;

        private static readonly Vector2 DefaultImageSize = new Vector2(1024, 576);

        private readonly Mock<IImage> _templateMock;

        private Mock<IImage> _imageMock;

        public TemplateFinderTests()
        {
            _templateMock = new Mock<IImage>();
            _templateMock.Setup(template => template.Width)
                         .Returns(79);
            _templateMock.Setup(template => template.Height)
                         .Returns(103);
        }

        [Fact]
        public void TestResizesTemplateToMatchImageSize()
        {
            const float scale = 2.5f;
            IImage image = SetupImage(scale);

            TemplateFinder().FindTemplateIn(image);

            VerifyResize(scale);
        }

        [Fact]
        public void TestDoesntResizeWhenCorrectSize()
        {
            IImage image = SetupImage();

            TemplateFinder().FindTemplateIn(image);

            VerifyResize(Times.Never);
        }

        [Fact]
        public void TestScaleFactorRelativeToOriginal()
        {
            const float scale = 2.5f;
            IImage scaledImage = SetupImage(scale);
            IImage defaultImage = SetupImage();

            TemplateFinder templateFinder = TemplateFinder();
            templateFinder.FindTemplateIn(scaledImage);
            templateFinder.FindTemplateIn(defaultImage);

            VerifyResize(scale);
            VerifyResize(1);
        }

        [Fact]
        public void TestMatchAgainstSubImageCorrectly()
        {
            const float scale = 2.5f;
            Vector2 targetLocation = new Vector2(0.5f, 0.5f);
            IImage image = SetupFakeImage();
            var expectedResult = SetupSubImageMatches(targetLocation, scale);

            var actualResult = TemplateFinder(targetLocation).FindTemplateIn(image);

            Assert.Equal(expectedResult.IsMatch, actualResult.IsMatch);
            Assert.Equal(expectedResult.MatchArea, actualResult.MatchArea);
        }

        private TemplateFinder TemplateFinder(Vector2 targetLocation = default)
        {
            return new TemplateFinder(_templateMock.Object, targetLocation);
        }

        private IImage SetupFakeImage()
        {
            _imageMock = new Mock<IImage>();
            return _imageMock.Object;
        }

        private IImage SetupImage(float scale = 1f)
        {
            _imageMock = new Mock<IImage>();

            SetupImageSize(scale);

            SetupSubImage(TemplateMatchResult.Failed);

            return _imageMock.Object;
        }

        private TemplateMatchResult SetupSubImageMatches(Vector2 targetLocation, float scale)
        {
            SetupImageSize(scale);

            int x = (int)(_imageMock.Object.Width * targetLocation.X);
            int y = (int)(_imageMock.Object.Height * targetLocation.Y);
            int width = _templateMock.Object.Width;
            int height = _templateMock.Object.Height;

            int subRegionMargin = (int)(MatchMargin * scale);
            int xOffset = x - subRegionMargin;
            int yOffset = y - subRegionMargin;
            Rectangle matchArea = new Rectangle(xOffset,
                                                yOffset,
                                                width + (2 * subRegionMargin),
                                                height + (2 * subRegionMargin));

            TemplateMatchResult templateMatchResult = new TemplateMatchResult(new Rectangle(x, y, width, height));

            SetupSubImage(templateMatchResult, matchArea);

            return new TemplateMatchResult(new Rectangle(x + xOffset, y + yOffset, width, height));
        }

        private void SetupImageSize(float scale)
        {
            _imageMock.Setup(image => image.Width)
                      .Returns((int)(DefaultImageSize.X * scale));

            _imageMock.Setup(image => image.Height)
                      .Returns((int)(DefaultImageSize.Y * scale));
        }

        private void SetupSubImage(TemplateMatchResult templateMatchResult)
        {
            SetupSubImage(templateMatchResult, image => image.SubImage(It.IsAny<Rectangle>()));
        }

        private void SetupSubImage(TemplateMatchResult templateMatchResult, Rectangle matchArea)
        {
            SetupSubImage(templateMatchResult, image => image.SubImage(matchArea));
        }

        private void SetupSubImage(TemplateMatchResult templateMatchResult, Expression<Func<IImage, IImage>> subImageExpression)
        {
            Mock<IImage> subImageMock = new Mock<IImage>();

            subImageMock.Setup(image => image.MatchTemplate(_templateMock.Object, It.IsInRange(0.75, 0.95, Range.Inclusive)))
                        .Returns(templateMatchResult);

            _imageMock.Setup(subImageExpression)
                      .Returns(subImageMock.Object);
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
    }
}