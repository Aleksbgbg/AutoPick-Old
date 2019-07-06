namespace AutoPick
{
    using AutoPick.Services.GameInteraction;
    using AutoPick.Services.GameInteraction.ImageProcessing;
    using AutoPick.Services.Resources;
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
            dependencyRegistrar.Singleton<IGameMonitor, GameMonitor>();
            dependencyRegistrar.Singleton<IThreadRunner, ThreadRunner>();
            dependencyRegistrar.Singleton<IGameStatusRetriever, GameStatusRetriever>();

            dependencyRegistrar.Singleton<Win32Kit, Win32Kit>();
            dependencyRegistrar.Handler<IGameWindowManager>(retriever => retriever.GetInstance<Win32Kit>());
            dependencyRegistrar.Handler<IGameWindowClicker>(retriever => retriever.GetInstance<Win32Kit>());
            dependencyRegistrar.Handler<IGameWindowTyper>(retriever => retriever.GetInstance<Win32Kit>());

            dependencyRegistrar.Singleton<IGameImageProcessor, GameImageProcessor>();
            dependencyRegistrar.Singleton<IToImageConverter, ToImageConverter>();
            dependencyRegistrar.Singleton<IImageHandlerFactory, ImageHandlerFactory>();
        }
    }
}