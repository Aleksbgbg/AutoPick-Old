namespace AutoPick.Services.GameInteraction
{
    public interface IGameStatusRetriever
    {
        GameStatusUpdate GetCurrentStatus();
    }
}