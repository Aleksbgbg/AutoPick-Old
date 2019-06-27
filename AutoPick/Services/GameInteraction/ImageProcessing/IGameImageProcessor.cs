namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    public interface IGameImageProcessor
    {
        GameStatusUpdate ProcessGameImage(IImage image);
    }
}