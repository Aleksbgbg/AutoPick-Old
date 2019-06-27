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

        private readonly Mock<IFromImageConverter> _fromImageConverterMock;

        private GameImageProcessor _gameImageProcessor;

        private Mock<IImageHandler>[] _imageHandlerMocks;

        public GameImageProcessorTests()
        {
            _imageHandlerFactory = new Mock<IImageHandlerFactory>();

            _fromImageConverterMock = new Mock<IFromImageConverter>();
        }

        private GameImageProcessor GameImageProcessor()
        {
            return _gameImageProcessor = new GameImageProcessor(_fromImageConverterMock.Object, _imageHandlerFactory.Object);
        }

        [Fact]
        public void TestGoesThrowAllImageHandlers()
        {
            IImage image = SetupImageHandling();
            ImageSource imageSource = SetupImageConversion(image);

            var gameStatusUpdate = GameImageProcessor().ProcessGameImage(image);

            VerifyHandleImageWithAllHandlers(image);
            Assert.Equal(ExpectedStatus, gameStatusUpdate.GameStatus);
            Assert.Equal(imageSource, gameStatusUpdate.GameImage);
        }

        [Fact]
        public void TestReturnsDefaultWhenNoHandlersAvailable()
        {
            IImage image = SetupImageHandlingNoneAvailable();
            ImageSource imageSource = SetupImageConversion(image);

            var gameStatusUpdate = GameImageProcessor().ProcessGameImage(image);

            Assert.Equal(GameStatus.Idle, gameStatusUpdate.GameStatus);
            Assert.Equal(imageSource, gameStatusUpdate.GameImage);
        }

        private IImage SetupImageHandling()
        {
            return SetupImageHandling(image => new ImageProcessingResult(),
                                      image => new ImageProcessingResult(true, ExpectedStatus, image)
                   );
        }

        private IImage SetupImageHandlingNoneAvailable()
        {
            return SetupImageHandling(image => new ImageProcessingResult(),
                                      image => new ImageProcessingResult());
        }

        private IImage SetupImageHandling(params Func<IImage, ImageProcessingResult>[] results)
        {
            IImage image = new Mock<IImage>().Object;

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

        private ImageSource SetupImageConversion(IImage image)
        {
            ImageSource imageSource = new BitmapImage();

            _fromImageConverterMock.Setup(converter => converter.ImageSourceFrom(image))
                                   .Returns(imageSource);

            return imageSource;
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