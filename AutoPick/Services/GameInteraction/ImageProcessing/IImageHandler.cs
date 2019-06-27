namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    public interface IImageHandler
    {
        ImageProcessingResult ProcessImage(IImage image);
    }
}