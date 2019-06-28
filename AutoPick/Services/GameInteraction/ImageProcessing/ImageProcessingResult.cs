namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using AutoPick.Models;

    public class ImageProcessingResult
    {
        public ImageProcessingResult(GameStatus gameStatus, IImage resultantImage)
        {
            Succeeded = true;
            GameStatus = gameStatus;
            ResultantImage = resultantImage;
        }

        private ImageProcessingResult()
        {
        }

        public static ImageProcessingResult Failed { get; } = new ImageProcessingResult();

        public bool Succeeded { get; }

        public GameStatus GameStatus { get; }

        public IImage ResultantImage { get; }
    }
}