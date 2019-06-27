namespace AutoPick.Tests.Services.Resources
{
    using System.Linq;

    using AutoPick.Models;
    using AutoPick.Services.Resources;

    using Moq;

    using Xunit;

    public class ChampionLoaderTests
    {
        private readonly Mock<IResourceResolver> _resourceResolverMock;

        private readonly Mock<IResourceReader> _resourceReaderMock;

        private readonly ChampionLoader _championLoader;

        public ChampionLoaderTests()
        {
            _resourceResolverMock = new Mock<IResourceResolver>();

            _resourceReaderMock = new Mock<IResourceReader>();

            _championLoader = new ChampionLoader(_resourceResolverMock.Object, _resourceReaderMock.Object);
        }

        [Fact]
        public void TestLoadAllChampions()
        {
            string[] championNames = SetupChampions();

            Champion[] champions = _championLoader.LoadAllChampions();

            Assert.Equal(championNames, champions.Select(champion => champion.Name));
        }

        private string[] SetupChampions()
        {
            string namesFile = "SomeNamesFile";

            _resourceResolverMock.Setup(resolver => resolver.ResolveResourcePath(ResourceType.ChampionNames))
                                 .Returns(namesFile);

            string[] championNames = { "Katarina", "Diana", "Kha'Zix" };

            _resourceReaderMock.Setup(reader => reader.ReadResourceFile(namesFile))
                               .Returns(championNames);

            return championNames;
        }
    }
}