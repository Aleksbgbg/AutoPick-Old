namespace AutoPick.Services.GameInteraction.ImageProcessing.ImageHandlers
{
    public interface IImageHandlerFactory
    {
        IImageHandler[] LoadImageHandlers();
    }
}