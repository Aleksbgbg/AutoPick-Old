namespace AutoPick.Tests.ViewModels
{
    using System.Linq;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels;

    using Moq;

    using Xunit;

    public class ChampionPickViewModelTests
    {
        private readonly Mock<IChampionLoader> _championLoaderMock;

        private ChampionPickViewModel _championPickViewModel;

        public ChampionPickViewModelTests()
        {
            _championLoaderMock = new Mock<IChampionLoader>();
        }

        [Fact]
        public void TestPopulatesChampionsFromLoader()
        {
            string[] championNames = SetupChampions();

            var viewModel = CreateViewModel();

            Assert.Equal(championNames, viewModel.Champions.Select(champion => champion.Name));
        }

        private string[] SetupChampions()
        {
            string[] championNames = { "Ahri", "Jax", "Kayn" };

            _championLoaderMock.Setup(loader => loader.LoadAllChampions())
                               .Returns(championNames.Select(name => new Champion(name)).ToArray());

            return championNames;
        }

        private ChampionPickViewModel CreateViewModel()
        {
            return _championPickViewModel = new ChampionPickViewModel(_championLoaderMock.Object);
        }
    }
}