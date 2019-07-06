namespace AutoPick.ViewModels
{
    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.Resources;
    using AutoPick.ViewModels.Interfaces;

    using Caliburn.Micro;

    public class RoleDisplayViewModel : ViewModelBase, IRoleDisplayViewModel
    {
        private readonly ISelectedRoleStore _selectedRoleStore;

        public RoleDisplayViewModel(ILaneLoader laneLoader, IGameTrackViewModel gameTrackViewModel, ISelectedRoleStore selectedRoleStore)
        {
            _selectedRoleStore = selectedRoleStore;
            GameTrackViewModel = gameTrackViewModel;
            Lanes = new BindableCollection<Lane>(laneLoader.LoadAllLanes());
            SelectedLane = Lanes[0];
        }

        public IGameTrackViewModel GameTrackViewModel { get; }

        public IObservableCollection<Lane> Lanes { get; }

        private Lane _selectedLane;
        public Lane SelectedLane
        {
            get => _selectedLane;

            set
            {
                if (_selectedLane == value) return;

                _selectedLane = value;
                NotifyOfPropertyChange(nameof(SelectedLane));

                _selectedRoleStore.Lane = _selectedLane.Name;
            }
        }

        private Champion _selectedChampion;
        public Champion SelectedChampion
        {
            get => _selectedChampion;

            private set
            {
                if (_selectedChampion == value) return;

                _selectedChampion = value;
                NotifyOfPropertyChange(nameof(SelectedChampion));
            }
        }

        public void ChangeChampion(Champion champion)
        {
            SelectedChampion = champion;
            _selectedRoleStore.Champion = champion.Name;
        }
    }
}