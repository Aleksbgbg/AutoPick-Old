namespace AutoPick.ViewModels.Interfaces
{
    using AutoPick.Models;

    public interface IRoleDisplayViewModel : IViewModelBase
    {
        void ChangeChampion(Champion champion);
    }
}