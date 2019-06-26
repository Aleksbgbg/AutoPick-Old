namespace AutoPick.Services
{
    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class LaneLoader : ILaneLoader
    {
        public Lane[] LoadAllLanes()
        {
            return new Lane[]
            {
                new Lane("Top"),
                new Lane("Jungle"),
                new Lane("Middle"),
                new Lane("Bottom"),
                new Lane("Support")
            };
        }
    }
}