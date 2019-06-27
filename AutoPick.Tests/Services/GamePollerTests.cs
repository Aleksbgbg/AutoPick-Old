namespace AutoPick.Tests.Services
{
    using System;
    using System.Windows.Media.Imaging;

    using AutoPick.Models;
    using AutoPick.Services;
    using AutoPick.Services.Interfaces;

    using Moq;

    using Xunit;

    public class GamePollerTests
    {
        private readonly Mock<IWin32Kit> _win32KitMock;

        private readonly Mock<IGameImageProcessor> _gameImageProcessorMock;

        private readonly GamePoller _gamePoller;

        public GamePollerTests()
        {
            _win32KitMock = new Mock<IWin32Kit>(MockBehavior.Strict);

            _gameImageProcessorMock = new Mock<IGameImageProcessor>();

            _gamePoller = new GamePoller(_win32KitMock.Object, _gameImageProcessorMock.Object);
        }

        [Fact]
        public void TestGameOffline()
        {
            SetupWindowInactive();

            var status = _gamePoller.GetCurrentStatus();

            Assert.Null(status.GameImage);
            Assert.Equal(GameStatus.Offline, status.GameStatus);
        }

        [Fact]
        public void TestGameMinimised()
        {
            SetupWindowMinimised();

            var status = _gamePoller.GetCurrentStatus();

            Assert.Null(status.GameImage);
            Assert.Equal(GameStatus.Minimised, status.GameStatus);
        }

        [Fact]
        public void TestAnalysesGameImageWhenWindowMaximised()
        {
            var expectedUpdate = SetupWindowActiveAndMaximised();

            var actualUpdate = _gamePoller.GetCurrentStatus();

            Assert.Equal(expectedUpdate.GameStatus, actualUpdate.GameStatus);
            Assert.Equal(expectedUpdate.GameImage, actualUpdate.GameImage);
        }

        [Fact]
        public void TestReleasesWindowCaptureAfterProcessing()
        {
            IntPtr image = SetupReleaseCaptureInSequence();

            _gamePoller.GetCurrentStatus();

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
            _win32KitMock.Setup(kit => kit.ReleaseWindowCapture(It.IsAny<IntPtr>()));

            var image = SetupWindowCapture();
            var status = SetupGameStatusFromImage(image);

            return status;
        }

        private void SetupWindowActive(bool isActive)
        {
            _win32KitMock.Setup(kit => kit.IsWindowActive())
                         .Returns(isActive);
        }

        private void SetupWindowMinimised(bool isMinimised)
        {
            _win32KitMock.Setup(kit => kit.IsWindowMinimised())
                         .Returns(isMinimised);
        }

        private IntPtr SetupWindowCapture()
        {
            IntPtr image = IntPtr.Zero;

            _win32KitMock.Setup(kit => kit.CaptureWindow())
                         .Returns(image);

            return image;
        }

        private GameStatusUpdate SetupGameStatusFromImage(IntPtr image)
        {
            GameStatusUpdate status = new GameStatusUpdate(GameStatus.AcceptingMatch,
                                                           new BitmapImage());

            _gameImageProcessorMock.Setup(analyser => analyser.ProcessGameImage(image))
                                   .Returns(status);

            return status;
        }

        private IntPtr SetupReleaseCaptureInSequence()
        {
            SetupWindowActive(true);
            SetupWindowMinimised(false);

            var sequence = new MockSequence();

            IntPtr image = IntPtr.Zero;

            _win32KitMock.InSequence(sequence)
                         .Setup(kit => kit.CaptureWindow())
                         .Returns(image);

            _gameImageProcessorMock.InSequence(sequence)
                                   .Setup(processor => processor.ProcessGameImage(image));

            _win32KitMock.InSequence(sequence)
                         .Setup(kit => kit.ReleaseWindowCapture(image));

            return image;
        }

        private void VerifyReleaseCapture(IntPtr image)
        {
            _win32KitMock.Verify(kit => kit.ReleaseWindowCapture(image), Times.Once);
        }
    }
}