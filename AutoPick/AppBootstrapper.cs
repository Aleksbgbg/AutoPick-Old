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
            dependencyRegistrar.Singleton<IRoleDisplayViewModel, RoleDisplayViewModel>();
            dependencyRegistrar.Singleton<IGameTrackViewModel, GameTrackViewModel>();
        }

        protected override void RegisterServices(IDependencyRegistrar dependencyRegistrar)
        {
            dependencyRegistrar.Singleton<IChampionLoader, ChampionLoader>();
            dependencyRegistrar.Singleton<IResourceResolver, ResourceResolver>();
            dependencyRegistrar.Singleton<IResourceReader, ResourceReader>();
            dependencyRegistrar.Singleton<ILocalDirectoryProvider, LocalDirectoryProvider>();
            dependencyRegistrar.Singleton<ILaneLoader, LaneLoader>();
            dependencyRegistrar.Singleton<IGamePollService, GamePollService>();
            dependencyRegistrar.Singleton<IThreadRunner, ThreadRunner>();
            dependencyRegistrar.Singleton<IGamePoller, GamePoller>();
            dependencyRegistrar.Singleton<IWin32Kit, Win32Kit>();
        }
    }
}