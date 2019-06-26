namespace AutoPick
{
    using AutoPick.ViewModels;
    using AutoPick.ViewModels.Interfaces;

    using Wingman.Bootstrapper;
    using Wingman.Container;

    public class AppBootstrapper : BootstrapperBase<IShellViewModel>
    {
        protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
        {
            dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
            dependencyRegistrar.Singleton<IMainViewModel, MainViewModel>();
            dependencyRegistrar.Singleton<IChampionPickViewModel, ChampionPickViewModel>();
        }
    }
}