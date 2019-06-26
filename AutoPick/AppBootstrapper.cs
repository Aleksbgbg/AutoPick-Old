namespace AutoPick
{
    using AutoPick.Services;
    using AutoPick.Services.Interfaces;
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

        protected override void RegisterServices(IDependencyRegistrar dependencyRegistrar)
        {
            dependencyRegistrar.Singleton<IChampionLoader, ChampionLoader>();
            dependencyRegistrar.Singleton<IResourceResolver, ResourceResolver>();
            dependencyRegistrar.Singleton<IResourceReader, ResourceReader>();
        }
    }
}