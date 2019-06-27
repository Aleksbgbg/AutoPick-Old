namespace AutoPick.Tests.Converters
{
    using AutoPick.Converters;
    using AutoPick.Models;
    using AutoPick.Services.Resources;

    using Moq;

    using Xunit;

    public class ResourceImageConverterTests
    {
        private readonly Mock<IResourceResolver> _resourceResolverMock;

        private ResourceImageConverter _resourceImageConverter;

        public ResourceImageConverterTests()
        {
            _resourceResolverMock = new Mock<IResourceResolver>();
        }

        [Fact]
        public void TestConvertChampionImage()
        {
            const string championName = "Katarina";
            string resourcePath = SetupResource(ResourceType.ChampionSquares);

            string championImage = Convert(championName);

            Assert.Equal($@"{resourcePath}\{championName}.png", championImage);
        }

        [Fact]
        public void TestConvertLaneImage()
        {
            const string laneName = "Top";
            string resourcePath = SetupResource(ResourceType.LaneImages);

            string laneImage = Convert(laneName);

            Assert.Equal($@"{resourcePath}\{laneName}.png", laneImage);
        }

        private string SetupResource(ResourceType resourceType)
        {
            string resource = @"CurrentDirectory\SomeResource";

            _resourceImageConverter = new ResourceImageConverter(_resourceResolverMock.Object, resourceType);

            _resourceResolverMock.Setup(resolver => resolver.ResolveResourcePath(resourceType))
                                 .Returns(resource);

            return resource;
        }

        private string Convert(string championName)
        {
            return (string)_resourceImageConverter.Convert(championName, null, null, null);
        }
    }
}