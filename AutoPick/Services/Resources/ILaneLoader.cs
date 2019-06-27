namespace AutoPick.Services.Resources
{
    using AutoPick.Models;

    public interface ILaneLoader
    {
        Lane[] LoadAllLanes();
    }
}