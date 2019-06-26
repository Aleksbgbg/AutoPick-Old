namespace AutoPick.Tests.Converters
{
    using AutoPick.Converters;
    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    using Moq;

    using Xunit;

    public class ChampionImageConverterTests
    {
        private readonly Mock<IResourceResolver> _resourceResolverMock;

        private readonly ChampionImageConverter _championImageConverter;

        public ChampionImageConverterTests()
        {
            _resourceResolverMock = new Mock<IResourceResolver>();

            _championImageConverter = new ChampionImageConverter(_resourceResolverMock.Object);
        }

        [Fact]
        public void TestConvert()
        {
            string championName = "Katarina";
            SetupChampionSquaresResource();

            string championImage = Convert(championName);

            Assert.Equal(@"CurrentDirectory\ChampionSquares\Katarina.png", championImage);
        }

        private void SetupChampionSquaresResource()
        {
            _resourceResolverMock.Setup(resolver => resolver.ResolveResourcePath(ResourceType.ChampionSquares))
                                 .Returns(@"CurrentDirectory\ChampionSquares");
        }

        private string Convert(string championName)
        {
            return (string)_championImageConverter.Convert(championName, null, null, null);
        }
    }
}