namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using AutoPick.Models;

    public class ImageProcessingResult
    {
        public ImageProcessingResult() : this(default, default, default)
        {
        }

        public ImageProcessingResult(bool succeeded, GameStatus gameStatus, IImage resultantImage)
        {
            Succeeded = succeeded;
            GameStatus = gameStatus;
            ResultantImage = resultantImage;
        }

        public bool Succeeded { get; }

        public GameStatus GameStatus { get; }

        public IImage ResultantImage { get; }
    }
}