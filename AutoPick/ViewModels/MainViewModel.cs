namespace AutoPick.ViewModels
{
    using AutoPick.ViewModels.Interfaces;

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IChampionPickViewModel championPickViewModel)
        {
            ChampionPickViewModel = championPickViewModel;
        }

        public IChampionPickViewModel ChampionPickViewModel { get; }
    }
}