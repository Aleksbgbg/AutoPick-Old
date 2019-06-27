namespace AutoPick.Services.GameInteraction
{
    using System.Windows.Media;

    using AutoPick.Models;

    public class GameStatusUpdate
    {
        public GameStatusUpdate(GameStatus gameStatus, ImageSource gameImage)
        {
            GameStatus = gameStatus;
            GameImage = gameImage;
        }

        public GameStatus GameStatus { get; }

        public ImageSource GameImage { get; }
    }
}