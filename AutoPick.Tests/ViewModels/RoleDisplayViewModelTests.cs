namespace AutoPick.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.Resources;
    using AutoPick.ViewModels;

    using Moq;

    using Xunit;

    public class RoleDisplayViewModelTests
    {
        private readonly Mock<ILaneLoader> _laneLoaderMock;

        private readonly Mock<ISelectedRoleStore> _selectedRoleStoreMock;

        private RoleDisplayViewModel _roleDisplayViewModel;

        public RoleDisplayViewModelTests()
        {
            _laneLoaderMock = new Mock<ILaneLoader>();

            _selectedRoleStoreMock = new Mock<ISelectedRoleStore>();
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

            ChangeChampion("Katarina");

            Assert.Equal("Katarina", ChampionName());
        }

        [Fact]
        public void TestStoresChampionChanges()
        {
            const string champName = "Galio";
            SetupLanes();

            ChangeChampion(champName);

            VerifyChampionStored(champName);
        }

        [Fact]
        public void TestStoresLaneChanges()
        {
            const string laneName = "Middle";
            SetupLanes();

            ChangeLane(laneName);

            VerifyLaneStored(laneName);
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
            return _roleDisplayViewModel = new RoleDisplayViewModel(_laneLoaderMock.Object, null, _selectedRoleStoreMock.Object);
        }

        private void ChangeChampion(string name)
        {
            CreateViewModel();
            _roleDisplayViewModel.ChangeChampion(new Champion(name));
        }

        private void ChangeLane(string name)
        {
            CreateViewModel();
            _roleDisplayViewModel.SelectedLane = new Lane(name);
        }

        private string ChampionName()
        {
            return _roleDisplayViewModel.SelectedChampion.Name;
        }

        private IEnumerable<string> GetLanes()
        {
            return _roleDisplayViewModel.Lanes.Select(lane => lane.Name);
        }

        private void VerifyChampionStored(string name)
        {
            _selectedRoleStoreMock.VerifySet(store => store.Champion = name, Times.Once);
        }

        private void VerifyLaneStored(string name)
        {
            _selectedRoleStoreMock.VerifySet(store => store.Lane = name, Times.Once);
        }
    }
}