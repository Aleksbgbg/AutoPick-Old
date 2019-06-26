namespace AutoPick.ViewModels
{
    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels.Interfaces;

    using Caliburn.Micro;

    public class RoleDisplayViewModel : ViewModelBase, IRoleDisplayViewModel
    {
        public RoleDisplayViewModel(ILaneLoader laneLoader)
        {
            Lanes = new BindableCollection<Lane>(laneLoader.LoadAllLanes());
            _selectedLane = Lanes[0];
        }

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
        }
    }
}