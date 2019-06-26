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

        [Fact]
        public void TestResolveChampionNames()
        {
            SetupCurrentDirectory();

            string championNamesFile = _resourceResolver.ResolveResourcePath(ResourceType.ChampionNames);

            Assert.Equal(@"CurrentDirectory\Resources\Champions.txt", championNamesFile);
        }

        private void SetupCurrentDirectory()
        {
            _localDirectoryProviderMock.Setup(provider => provider.CurrentDirectory)
                                       .Returns(CurrentDirectory);
        }
    }
}