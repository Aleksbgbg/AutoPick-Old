namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;
    using System.Windows.Media.Imaging;

    public interface IImage
    {
        int Width { get; }

        int Height { get; }

        void Draw(Rectangle rectangle);

        void Resize(double scale);

        TemplateMatchResult MatchTemplate(IImage template, double threshold);

        BitmapImage ToBitmapImage();
    }
}