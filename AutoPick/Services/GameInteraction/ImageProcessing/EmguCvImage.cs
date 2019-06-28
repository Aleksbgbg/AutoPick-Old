namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Media.Imaging;

    using Emgu.CV;
    using Emgu.CV.CvEnum;
    using Emgu.CV.Structure;

    public class EmguCvImage : IImage
    {
        private readonly Image<Rgb, byte> _image;

        public EmguCvImage(Bitmap bitmap)
        {
            _image = new Image<Rgb, byte>(bitmap);
        }

        public EmguCvImage(string filepath)
        {
            _image = new Image<Rgb, byte>(filepath);
        }

        public int Width => _image.Width;

        public int Height => _image.Height;

        public void Draw(Rectangle rectangle)
        {
            _image.Draw(rectangle, new Rgb(255, 0, 0), thickness: 3);
        }

        public void Resize(double scale)
        {
            _image.Resize(scale, Inter.Linear);
        }

        public TemplateMatchResult MatchTemplate(IImage template, double threshold)
        {
            using (Image<Gray, float> result = _image.MatchTemplate(template.ToCvImage(),
                                                                    TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out double[] minValues,
                              out double[] maxValues,
                              out Point[] minLocations,
                              out Point[] maxLocations);

                if (maxValues[0] > threshold)
                {
                    Rectangle matchArea = new Rectangle(maxLocations[0], new Size(template.Width, template.Height));

                    return new TemplateMatchResult(true, matchArea);
                }
            }

            return new TemplateMatchResult();
        }

        public BitmapImage ToBitmapImage()
        {
            using (Stream stream = new MemoryStream())
            {
                _image.Bitmap.Save(stream, ImageFormat.Bmp);
                stream.Seek(0, SeekOrigin.Begin);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }
        }

        public Image<Rgb, byte> ToCvImage()
        {
            return _image;
        }
    }
}