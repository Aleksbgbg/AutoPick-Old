namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;

    public class ToImageConverter : IToImageConverter
    {
        public IImage ImageFrom(IntPtr pointer)
        {
            Bitmap bitmap = Image.FromHbitmap(pointer);

            return new EmguCvImage(bitmap);
        }
    }
}