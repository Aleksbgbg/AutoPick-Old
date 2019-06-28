namespace AutoPick.Tests.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class GameImageProcessorTests
    {
        private const GameStatus ExpectedStatus = GameStatus.PickingLane;

        private readonly Mock<IImageHandlerFactory> _imageHandlerFactory;

        private Mock<IImageHandler>[] _imageHandlerMocks;

        private Mock<IImage> _imageMock;

        public GameImageProcessorTests()
        {
            _imageHandlerFactory = new Mock<IImageHandlerFactory>();
        }

        private GameImageProcessor GameImageProcessor()
        {
            return new GameImageProcessor(_imageHandlerFactory.Object);
        }

        [Fact]
        public void TestGoesThrowAllImageHandlers()
        {
            IImage image = SetupImageHandling();
            ImageSource imageSource = SetupImageConversion();

            var gameStatusUpdate = GameImageProcessor().ProcessGameImage(image);

            VerifyHandleImageWithAllHandlers(image);
            Assert.Equal(ExpectedStatus, gameStatusUpdate.GameStatus);
            Assert.Equal(imageSource, gameStatusUpdate.GameImage);
        }

        [Fact]
        public void TestReturnsDefaultWhenNoHandlersAvailable()
        {
            IImage image = SetupImageHandlingNoneAvailable();
            ImageSource imageSource = SetupImageConversion();

            var gameStatusUpdate = GameImageProcessor().ProcessGameImage(image);

            Assert.Equal(GameStatus.Idle, gameStatusUpdate.GameStatus);
            Assert.Equal(imageSource, gameStatusUpdate.GameImage);
        }

        private IImage SetupImageHandling()
        {
            return SetupImageHandling(image => ImageProcessingResult.Failed,
                                      image => new ImageProcessingResult(ExpectedStatus, image)
                   );
        }

        private IImage SetupImageHandlingNoneAvailable()
        {
            return SetupImageHandling(image => ImageProcessingResult.Failed,
                                      image => ImageProcessingResult.Failed);
        }

        private IImage SetupImageHandling(params Func<IImage, ImageProcessingResult>[] results)
        {
            _imageMock = new Mock<IImage>();

            IImage image = _imageMock.Object;

            _imageHandlerMocks = results.Select(result => SetupImageHandler(image, result(image))).ToArray();

            _imageHandlerFactory.Setup(factory => factory.LoadImageHandlers())
                                .Returns(_imageHandlerMocks.Select(mock => mock.Object).ToArray());

            return image;
        }

        private static Mock<IImageHandler> SetupImageHandler(IImage image, ImageProcessingResult imageProcessingResult)
        {
            Mock<IImageHandler> imageHandlerMock = new Mock<IImageHandler>();

            imageHandlerMock.Setup(handler => handler.ProcessImage(image))
                            .Returns(imageProcessingResult);

            return imageHandlerMock;
        }

        private ImageSource SetupImageConversion()
        {
            BitmapImage bitmapImage = new BitmapImage();

            _imageMock.Setup(image => image.ToBitmapImage())
                      .Returns(bitmapImage);

            return bitmapImage;
        }

        private void VerifyHandleImageWithAllHandlers(IImage image)
        {
            foreach (var handlerMock in _imageHandlerMocks)
            {
                handlerMock.Verify(handler => handler.ProcessImage(image), Times.Once);
            }
        }
    }
}