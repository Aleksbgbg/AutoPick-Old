namespace AutoPick.Tests.ViewModels
{
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;
    using AutoPick.ViewModels;

    using Moq;

    using Xunit;

    public class GameTrackViewModelTests
    {
        private readonly Mock<IGameMonitor> _gameMonitorMock;

        private readonly GameTrackViewModel _gameTrackViewModel;

        public GameTrackViewModelTests()
        {
            _gameMonitorMock = new Mock<IGameMonitor>();

            _gameTrackViewModel = new GameTrackViewModel(_gameMonitorMock.Object);
        }

        [Fact]
        public void TestStoresCorrectStatusMessage()
        {
            const GameStatus status = GameStatus.InLobby;

            RaiseGameUpdatedEvent(status);

            Assert.Equal(status, _gameTrackViewModel.Status);
        }

        [Fact]
        public void TestStoresImage()
        {
            ImageSource image = new BitmapImage();

            RaiseGameUpdatedEvent(image);

            Assert.Equal(image, _gameTrackViewModel.Image);
        }

        private void RaiseGameUpdatedEvent(GameStatus status)
        {
            _gameMonitorMock.Raise(monitor => monitor.GameUpdated += null, (object)new GameStatusUpdate(status, gameImage: null));
        }

        private void RaiseGameUpdatedEvent(ImageSource image)
        {
            _gameMonitorMock.Raise(monitor => monitor.GameUpdated += null, (object)new GameStatusUpdate(gameStatus: 0, image));
        }
    }
}