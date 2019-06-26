namespace AutoPick.Tests.ViewModels
{
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels;

    using Moq;

    using Xunit;

    public class GameTrackViewModelTests
    {
        private readonly Mock<IGamePollService> _gamePollServiceMock;

        private readonly Mock<IStatusMessageService> _statusMessageServiceMock;

        private readonly GameTrackViewModel _gameTrackViewModel;

        public GameTrackViewModelTests()
        {
            _gamePollServiceMock = new Mock<IGamePollService>();

            _statusMessageServiceMock = new Mock<IStatusMessageService>();

            _gameTrackViewModel = new GameTrackViewModel(_gamePollServiceMock.Object, _statusMessageServiceMock.Object);
        }

        [Fact]
        public void TestStoresCorrectStatusMessage()
        {
            const GameStatus status = GameStatus.InLobby;
            const string statusText = "In the lobby";
            SetupStatusText(status, statusText);

            RaiseGameUpdatedEvent(status);

            Assert.Equal(statusText, _gameTrackViewModel.Status);
        }

        [Fact]
        public void TestStoresImage()
        {
            ImageSource image = new BitmapImage();

            RaiseGameUpdatedEvent(image);

            Assert.Equal(image, _gameTrackViewModel.Image);
        }

        private void SetupStatusText(GameStatus status, string text)
        {
            _statusMessageServiceMock.Setup(service => service.ConvertToStatusMessage(status))
                                     .Returns(text);
        }

        private void RaiseGameUpdatedEvent(GameStatus status)
        {
            _gamePollServiceMock.Raise(service => service.GameUpdated += null, (object)new GameStatusUpdate(status, gameImage: null));
        }

        private void RaiseGameUpdatedEvent(ImageSource image)
        {
            _gamePollServiceMock.Raise(service => service.GameUpdated += null, (object)new GameStatusUpdate(gameStatus: 0, image));
        }
    }
}