namespace AutoPick.Tests.Services
{
    using AutoPick.Models;
    using AutoPick.Services;
    using AutoPick.Services.Interfaces;

    using Moq;

    using Xunit;

    public class GamePollServiceTests
    {
        private const int StartupDelay = 2000;

        private Mock<IThreadRunner> _threadRunnerMock;

        private Mock<IGamePoller> _gamePollerMock;

        private GamePollService _gamePollService;

        private GameStatusUpdate _gameStatusUpdate;

        public GamePollServiceTests()
        {
            _threadRunnerMock = new Mock<IThreadRunner>();

            _gamePollerMock = new Mock<IGamePoller>();

            _gamePollService = Create();
        }

        [Fact]
        public void TestInitialDelay()
        {
            SetupSetDelayThenStart(StartupDelay);

            Create();

            VerifySetDelayThenStart(StartupDelay);
        }

        [Fact]
        public void TestInvokesUpdateMessages()
        {
            GameStatusUpdate expectedUpdate = SetupStatusUpdate();
            SetupCaptureGameStatusUpdate();

            AwakeThread();

            Assert.Same(expectedUpdate, _gameStatusUpdate);
        }

        [Theory]
        [InlineData(GameStatus.Offline, 2000)]
        [InlineData(GameStatus.Minimised, 1000)]
        [InlineData(GameStatus.Idle, 5000)]
        [InlineData(GameStatus.InLobby, 5000)]
        [InlineData(GameStatus.Searching, 5000)]
        [InlineData(GameStatus.AcceptingMatch, 100)]
        [InlineData(GameStatus.PickingLane, 100)]
        [InlineData(GameStatus.ChampionSelect, 5000)]
        public void TestAccurateDelay(GameStatus status, int delay)
        {
            SetupStatusUpdate(status);

            AwakeThread();

            VerifySetDelay(delay);
        }

        private GamePollService Create()
        {
            return _gamePollService = new GamePollService(_threadRunnerMock.Object, _gamePollerMock.Object);
        }

        private void SetupSetDelayThenStart(int delay)
        {
            _threadRunnerMock = new Mock<IThreadRunner>(MockBehavior.Strict);

            var sequence = new MockSequence();

            _threadRunnerMock.InSequence(sequence)
                             .Setup(runner => runner.ChangeDelay(delay));

            _threadRunnerMock.InSequence(sequence)
                             .Setup(runner => runner.Start());
        }

        private GameStatusUpdate SetupStatusUpdate()
        {
            return SetupStatusUpdate(GameStatus.InLobby);
        }

        private void SetupCaptureGameStatusUpdate()
        {
            _gamePollService.GameUpdated += update => _gameStatusUpdate = update;
        }

        private GameStatusUpdate SetupStatusUpdate(GameStatus status)
        {
            GameStatusUpdate gameStatusUpdate = new GameStatusUpdate(status, null);

            _gamePollerMock.Setup(poller => poller.GetCurrentStatus())
                           .Returns(gameStatusUpdate);

            return gameStatusUpdate;
        }

        private void AwakeThread()
        {
            _threadRunnerMock.Raise(runner => runner.ThreadAwake += null);
        }

        private void VerifySetDelayThenStart(int delay)
        {
            _threadRunnerMock.Verify(runner => runner.ChangeDelay(delay), Times.Once);
            _threadRunnerMock.Verify(runner => runner.Start(), Times.Once);
        }

        private void VerifySetDelay(int delay)
        {
            _threadRunnerMock.Verify(runner => runner.ChangeDelay(delay), delay == StartupDelay ? Times.Exactly(2) : Times.Once());
        }
    }
}