namespace AutoPick.ViewModels
{
    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels.Interfaces;

    using Caliburn.Micro;

    public class ChampionPickViewModel : ViewModelBase, IChampionPickViewModel
    {
        public ChampionPickViewModel(IChampionLoader championLoader)
        {
            Champions = new BindableCollection<Champion>(championLoader.LoadAllChampions());
        }

        public IObservableCollection<Champion> Champions { get; }
    }
}