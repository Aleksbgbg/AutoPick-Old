namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    public interface IImageHandlerFactory
    {
        IImageHandler[] LoadImageHandlers();
    }
}