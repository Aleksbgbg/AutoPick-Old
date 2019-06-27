namespace AutoPick.Services.Resources
{
    using AutoPick.Models;

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