namespace AutoPick.Services.GameInteraction.ImageProcessing.ImageHandlers
{
    public interface IImageHandler
    {
        ImageProcessingResult ProcessImage(IImage image);
    }
}