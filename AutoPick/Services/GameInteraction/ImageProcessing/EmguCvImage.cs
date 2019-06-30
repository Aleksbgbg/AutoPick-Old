namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    using Emgu.CV;
    using Emgu.CV.CvEnum;
    using Emgu.CV.Structure;

    public class EmguCvImage : IImage
    {
        private readonly Image<Rgb, byte> _originalImage;

        private Image<Rgb, byte> _currentImage;

        public EmguCvImage(Bitmap bitmap)
        {
            _originalImage = _currentImage = new Image<Rgb, byte>(bitmap);
        }

        public EmguCvImage(string filepath)
        {
            _originalImage = _currentImage = new Image<Rgb, byte>(filepath);
        }

        private EmguCvImage(Image<Rgb, byte> image)
        {
            _originalImage = _currentImage = image;
        }

        public int Width => _currentImage.Width;

        public int Height => _currentImage.Height;

        public void Draw(Rectangle rectangle)
        {
            _currentImage.Draw(rectangle, new Rgb(255, 0, 0), thickness: 2);
        }

        public void Resize(double scale)
        {
            if (Math.Abs(scale - 1) < 0.1)
            {
                _currentImage = _originalImage;
            }
            else
            {
                _currentImage = _originalImage.Resize(scale, Inter.Linear);
            }
        }

        public IImage SubImage(Rectangle portion)
        {
            Image<Rgb, byte> newImage = new Image<Rgb, byte>(portion.Width, portion.Height);

            for (int x = 0; x < portion.Width; ++x)
            {
                for (int y = 0; y < portion.Height; ++y)
                {
                    newImage[y, x] = _currentImage[y + portion.Y, x + portion.X];
                }
            }

            return new EmguCvImage(newImage);
        }

        public TemplateMatchResult MatchTemplate(IImage template, double threshold)
        {
            using (Image<Gray, float> result = _currentImage.MatchTemplate(template.ToCvImage(),
                                                                    TemplateMatchingType.CcoeffNormed))
            {
                bool x = false;

                result.MinMax(out double[] minValues,
                              out double[] maxValues,
                              out Point[] minLocations,
                              out Point[] maxLocations);

                if (x)
                {
                    template.ToCvImage().Save("E:\\Pictures\\League\\Template.png");
                    _currentImage.Save("E:\\Pictures\\League\\Actual.png");
                }

                if (maxValues[0] > threshold)
                {
                    Rectangle matchArea = new Rectangle(maxLocations[0], new Size(template.Width, template.Height));

                    return new TemplateMatchResult(matchArea);
                }
            }

            return TemplateMatchResult.Failed;
        }

        public BitmapSource ToBitmapImage()
        {
            using (Bitmap bitmap = _currentImage.Bitmap)
            {
                IntPtr hbitmap = bitmap.GetHbitmap();

                try
                {
                    BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap,
                                                                                      IntPtr.Zero,
                                                                                      System.Windows.Int32Rect.Empty,
                                                                                      BitmapSizeOptions.FromEmptyOptions());
                    bitmapSource.Freeze();

                    return bitmapSource;
                }
                finally
                {
                    DeleteObject(hbitmap);
                }
            }
        }

        public Image<Rgb, byte> ToCvImage()
        {
            return _currentImage;
        }

        [DllImport("GDI32.dll")]
        private static extern int DeleteObject(IntPtr obj);
    }
}