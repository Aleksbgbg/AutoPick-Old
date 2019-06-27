namespace AutoPick.Tests.Services
{
    using AutoPick.Models;
    using AutoPick.Services;
    using AutoPick.Services.Interfaces;

    using Moq;

    using Xunit;

    public class ResourceResolverTests
    {
        private const string CurrentDirectory = "CurrentDirectory";

        private readonly Mock<ILocalDirectoryProvider> _localDirectoryProviderMock;

        private readonly ResourceResolver _resourceResolver;

        public ResourceResolverTests()
        {
            _localDirectoryProviderMock = new Mock<ILocalDirectoryProvider>();

            _resourceResolver = new ResourceResolver(_localDirectoryProviderMock.Object);
        }

        [Theory]
        [InlineData(ResourceType.ChampionNames, "Champions.txt")]
        [InlineData(ResourceType.ChampionSquares, "ChampionSquares")]
        [InlineData(ResourceType.LaneImages, "Lanes")]
        [InlineData(ResourceType.DetectionImages, "Detection")]
        public void TestResolveResource(ResourceType type, string expectedPath)
        {
            SetupCurrentDirectory();

            string filePath = Resolve(type);

            Assert.Equal(GetResourceRelativePath(expectedPath), filePath);
        }

        private void SetupCurrentDirectory()
        {
            _localDirectoryProviderMock.Setup(provider => provider.CurrentDirectory)
                                       .Returns(CurrentDirectory);
        }

        private string Resolve(ResourceType resourceType)
        {
            return _resourceResolver.ResolveResourcePath(resourceType);
        }

        private static string GetResourceRelativePath(string resourceName)
        {
            return $@"{CurrentDirectory}\Resources\{resourceName}";
        }
    }
}