namespace AutoPick.Tests.ViewModels
{
    using System.Linq;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels;
    using AutoPick.ViewModels.Interfaces;

    using Moq;

    using Xunit;

    public class ChampionPickViewModelTests
    {
        private readonly Mock<IChampionLoader> _championLoaderMock;

        private readonly Mock<IRoleDisplayViewModel> _roleDisplayViewModelMock;

        private ChampionPickViewModel _championPickViewModel;

        public ChampionPickViewModelTests()
        {
            _championLoaderMock = new Mock<IChampionLoader>();

            _roleDisplayViewModelMock = new Mock<IRoleDisplayViewModel>();
        }

        [Fact]
        public void TestPopulatesChampionsFromLoader()
        {
            string[] championNames = SetupChampions();

            var viewModel = CreateViewModel();

            Assert.Equal(championNames, viewModel.Champions.Select(champion => champion.Name));
        }

        [Fact]
        public void TestChangeSelectedChampionReflectsInDisplay()
        {
            SetupChampions();
            Champion champion = new Champion("Zed");

            CreateViewModel().SelectedChampion = champion;

            VerifyChangeChampionCalled(champion);
        }

        [Fact]
        public void TestChampionResetsWhenNull()
        {
            var champions = SetupChampions();
            var viewModel = CreateViewModel();

            viewModel.SelectedChampion = null;

            Assert.Equal(champions[0], viewModel.SelectedChampion.Name);
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
            return _championPickViewModel = new ChampionPickViewModel(_championLoaderMock.Object, _roleDisplayViewModelMock.Object);
        }

        private void VerifyChangeChampionCalled(Champion champion)
        {
            _roleDisplayViewModelMock.Verify(roleDisplay => roleDisplay.ChangeChampion(champion), Times.Once);
        }
    }
}