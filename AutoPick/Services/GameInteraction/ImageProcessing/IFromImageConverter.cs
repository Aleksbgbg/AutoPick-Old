namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Windows.Media;

    public interface IFromImageConverter
    {
        ImageSource ImageSourceFrom(IImage image);
    }
}