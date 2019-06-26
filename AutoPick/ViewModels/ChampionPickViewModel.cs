namespace AutoPick.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels.Interfaces;

    using Caliburn.Micro;

    public class ChampionPickViewModel : ViewModelBase, IChampionPickViewModel
    {
        private readonly ICollectionView _championsCollectionView;

        public ChampionPickViewModel(IChampionLoader championLoader)
        {
            Champions = new BindableCollection<Champion>(championLoader.LoadAllChampions());

            _championsCollectionView = CollectionViewSource.GetDefaultView(Champions);
        }

        public IObservableCollection<Champion> Champions { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;

            set
            {
                if (_searchText == value) return;

                _searchText = value;
                NotifyOfPropertyChange(nameof(SearchText));

                AssignChampionFilter(_searchText);
            }
        }

        private void AssignChampionFilter(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _championsCollectionView.Filter = null;
            }
            else
            {
                _championsCollectionView.Filter = FilterChampionMatchesSearch;
            }
        }

        private bool FilterChampionMatchesSearch(object champion)
        {
            return FilterChampionMatchesSearch((Champion)champion);
        }

        private bool FilterChampionMatchesSearch(Champion champion)
        {
            return champion.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}