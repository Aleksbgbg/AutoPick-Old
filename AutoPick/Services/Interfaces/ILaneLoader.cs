namespace AutoPick.Services.Interfaces
{
    using AutoPick.Models;

    public interface ILaneLoader
    {
        Lane[] LoadAllLanes();
    }
}