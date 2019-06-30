namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;
    using System.Windows.Media.Imaging;

    using Emgu.CV;
    using Emgu.CV.Structure;

    public interface IImage
    {
        int Width { get; }

        int Height { get; }

        void Draw(Rectangle rectangle);

        void Resize(double scale);

        IImage SubImage(Rectangle portion);

        TemplateMatchResult MatchTemplate(IImage template, double threshold);

        BitmapSource ToBitmapImage();

        Image<Rgb, byte> ToCvImage();
    }
}