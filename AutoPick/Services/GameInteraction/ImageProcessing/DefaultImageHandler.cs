namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using AutoPick.Models;

    public class DefaultImageHandler : ImageHandlerBase
    {
        public DefaultImageHandler(ITemplateFinder templateFinder, GameStatus gameStatus) : base(templateFinder, gameStatus)
        {
        }
    }
}