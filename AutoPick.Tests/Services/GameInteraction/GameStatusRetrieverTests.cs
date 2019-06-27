namespace AutoPick.Tests.Services.GameInteraction
{
    using System;
    using System.Windows.Media.Imaging;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.GameInteraction.ImageProcessing;

    using Moq;

    using Xunit;

    public class GameStatusRetrieverTests
    {
        private readonly Mock<IGameWindowManager> _gameWindowManagerMock;

        private readonly Mock<IGameImageProcessor> _gameImageProcessorMock;

        private readonly Mock<IToImageConverter> _imageConverterMock;

        private readonly GameStatusRetriever _gameStatusRetriever;

        public GameStatusRetrieverTests()
        {
            _gameWindowManagerMock = new Mock<IGameWindowManager>(MockBehavior.Strict);

            _gameImageProcessorMock = new Mock<IGameImageProcessor>();

            _imageConverterMock = new Mock<IToImageConverter>();

            _gameStatusRetriever = new GameStatusRetriever(_gameWindowManagerMock.Object, _gameImageProcessorMock.Object, _imageConverterMock.Object);
        }

        [Fact]
        public void TestGameOffline()
        {
            SetupWindowInactive();

            var status = _gameStatusRetriever.GetCurrentStatus();

            Assert.Null(status.GameImage);
            Assert.Equal(GameStatus.Offline, status.GameStatus);
        }

        [Fact]
        public void TestGameMinimised()
        {
            SetupWindowMinimised();

            var status = _gameStatusRetriever.GetCurrentStatus();

            Assert.Null(status.GameImage);
            Assert.Equal(GameStatus.Minimised, status.GameStatus);
        }

        [Fact]
        public void TestAnalysesGameImageWhenWindowMaximised()
        {
            var expectedUpdate = SetupWindowActiveAndMaximised();

            var actualUpdate = _gameStatusRetriever.GetCurrentStatus();

            Assert.Equal(expectedUpdate.GameStatus, actualUpdate.GameStatus);
            Assert.Equal(expectedUpdate.GameImage, actualUpdate.GameImage);
        }

        [Fact]
        public void TestReleasesWindowCaptureAfterProcessing()
        {
            IntPtr image = SetupReleaseCaptureInSequence();

            _gameStatusRetriever.GetCurrentStatus();

            VerifyReleaseCapture(image);
        }

        private void SetupWindowInactive()
        {
            SetupWindowActive(false);
        }

        private void SetupWindowMinimised()
        {
            SetupWindowActive(true);
            SetupWindowMinimised(true);
        }

        private GameStatusUpdate SetupWindowActiveAndMaximised()
        {
            SetupWindowActive(true);
            SetupWindowMinimised(false);

            Mock<IImage> imageMock = new Mock<IImage>();

            _imageConverterMock.Setup(converter => converter.ImageFrom(It.IsAny<IntPtr>()))
                               .Returns(imageMock.Object);
            _gameWindowManagerMock.Setup(manager => manager.ReleaseWindowCapture(It.IsAny<IntPtr>()));

            var image = SetupWindowCapture();
            var status = SetupGameStatusFromImage(imageMock.Object);

            return status;
        }

        private void SetupWindowActive(bool isActive)
        {
            _gameWindowManagerMock.Setup(manager => manager.IsWindowActive())
                                  .Returns(isActive);
        }

        private void SetupWindowMinimised(bool isMinimised)
        {
            _gameWindowManagerMock.Setup(manager => manager.IsWindowMinimised())
                                  .Returns(isMinimised);
        }

        private IntPtr SetupWindowCapture()
        {
            IntPtr image = IntPtr.Zero;

            _gameWindowManagerMock.Setup(manager => manager.CaptureWindow())
                                  .Returns(image);

            return image;
        }

        private GameStatusUpdate SetupGameStatusFromImage(IImage image)
        {
            GameStatusUpdate status = new GameStatusUpdate(GameStatus.AcceptingMatch,
                                                           new BitmapImage());

            _gameImageProcessorMock.Setup(processor => processor.ProcessGameImage(image))
                                   .Returns(status);

            return status;
        }

        private IntPtr SetupReleaseCaptureInSequence()
        {
            SetupWindowActive(true);
            SetupWindowMinimised(false);

            var sequence = new MockSequence();

            IntPtr image = IntPtr.Zero;

            _gameWindowManagerMock.InSequence(sequence)
                                  .Setup(manager => manager.CaptureWindow())
                                  .Returns(image);

            Mock<IImage> imageMock = new Mock<IImage>();

            _imageConverterMock.Setup(converter => converter.ImageFrom(image))
                               .Returns(imageMock.Object);

            _gameImageProcessorMock.InSequence(sequence)
                                   .Setup(processor => processor.ProcessGameImage(imageMock.Object));

            _gameWindowManagerMock.InSequence(sequence)
                                  .Setup(manager => manager.ReleaseWindowCapture(image));

            return image;
        }

        private void VerifyReleaseCapture(IntPtr image)
        {
            _gameWindowManagerMock.Verify(manager => manager.ReleaseWindowCapture(image), Times.Once);
        }
    }
}