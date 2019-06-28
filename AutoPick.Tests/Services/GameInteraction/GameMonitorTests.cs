namespace AutoPick.Tests.Services.GameInteraction
{
    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;

    using Moq;

    using Xunit;

    public class GameMonitorTests
    {
        private const int StartupDelay = 2000;

        private Mock<IThreadRunner> _threadRunnerMock;

        private readonly Mock<IGameStatusRetriever> _gameStatusRetrieverMock;

        private GameMonitor _gameMonitor;

        private GameStatusUpdate _gameStatusUpdate;

        public GameMonitorTests()
        {
            _threadRunnerMock = new Mock<IThreadRunner>();

            _gameStatusRetrieverMock = new Mock<IGameStatusRetriever>();

            _gameMonitor = Create();
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
        [InlineData(GameStatus.Idle, 1000)]
        [InlineData(GameStatus.InLobby, 1000)]
        [InlineData(GameStatus.Searching, 1000)]
        [InlineData(GameStatus.AcceptingMatch, 100)]
        [InlineData(GameStatus.PickingLane, 100)]
        [InlineData(GameStatus.ChampionSelect, 1000)]
        public void TestAccurateDelay(GameStatus status, int delay)
        {
            SetupStatusUpdate(status);

            AwakeThread();

            VerifySetDelay(delay);
        }

        private GameMonitor Create()
        {
            return _gameMonitor = new GameMonitor(_threadRunnerMock.Object, _gameStatusRetrieverMock.Object);
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
            _gameMonitor.GameUpdated += update => _gameStatusUpdate = update;
        }

        private GameStatusUpdate SetupStatusUpdate(GameStatus status)
        {
            GameStatusUpdate gameStatusUpdate = new GameStatusUpdate(status, null);

            _gameStatusRetrieverMock.Setup(retriever => retriever.GetCurrentStatus())
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