namespace AutoPick.Models
{
    using System.Windows.Media;

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