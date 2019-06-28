namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using AutoPick.Models;

    public class DefaultImageHandler : ImageHandlerBase
    {
        public DefaultImageHandler(IImage template, GameStatus gameStatus) : base(template, gameStatus)
        {
        }
    }
}