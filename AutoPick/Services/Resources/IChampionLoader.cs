namespace AutoPick.Services.Resources
{
    using AutoPick.Models;

    public interface IChampionLoader
    {
        Champion[] LoadAllChampions();
    }
}