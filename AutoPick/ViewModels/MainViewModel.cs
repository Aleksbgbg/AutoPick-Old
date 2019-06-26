namespace AutoPick.ViewModels
{
    using AutoPick.ViewModels.Interfaces;

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IChampionPickViewModel championPickViewModel, IRoleDisplayViewModel roleDisplayViewModel)
        {
            ChampionPickViewModel = championPickViewModel;
            RoleDisplayViewModel = roleDisplayViewModel;
        }

        public IChampionPickViewModel ChampionPickViewModel { get; }

        public IRoleDisplayViewModel RoleDisplayViewModel { get; }
    }
}