namespace AutoPick.ViewModels.Interfaces
{
    public interface IShellViewModel : IViewModelBase
    {
        IMainViewModel MainViewModel { get; }
    }
}