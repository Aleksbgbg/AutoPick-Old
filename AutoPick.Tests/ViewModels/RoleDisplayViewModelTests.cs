namespace AutoPick.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPick.Models;
    using AutoPick.Services.Resources;
    using AutoPick.ViewModels;

    using Moq;

    using Xunit;

    public class RoleDisplayViewModelTests
    {
        private readonly Mock<ILaneLoader> _laneLoaderMock;

        private RoleDisplayViewModel _roleDisplayViewModel;

        public RoleDisplayViewModelTests()
        {
            _laneLoaderMock = new Mock<ILaneLoader>();
        }

        [Fact]
        public void TestPopulatesLanesCorrectly()
        {
            string[] expectedLanes = SetupLanes();

            CreateViewModel();

            Assert.Equal(expectedLanes, GetLanes());
        }

        [Fact]
        public void TestChangeChampion()
        {
            SetupLanes();

            ChangeChampion(new Champion("Katarina"));

            Assert.Equal("Katarina", ChampionName());
        }

        private string[] SetupLanes()
        {
            string[] lanes = { "Mid", "Jng" };

            _laneLoaderMock.Setup(loader => loader.LoadAllLanes())
                           .Returns(lanes.Select(lane => new Lane(lane)).ToArray());

            return lanes;
        }

        private RoleDisplayViewModel CreateViewModel()
        {
            return _roleDisplayViewModel = new RoleDisplayViewModel(_laneLoaderMock.Object, null);
        }

        private void ChangeChampion(Champion champion)
        {
            CreateViewModel();
            _roleDisplayViewModel.ChangeChampion(champion);
        }

        private string ChampionName()
        {
            return _roleDisplayViewModel.SelectedChampion.Name;
        }

        private IEnumerable<string> GetLanes()
        {
            return _roleDisplayViewModel.Lanes.Select(lane => lane.Name);
        }
    }
}