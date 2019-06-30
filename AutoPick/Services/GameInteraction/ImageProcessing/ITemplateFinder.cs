namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    public interface ITemplateFinder
    {
        TemplateMatchResult FindTemplateIn(IImage image);
    }
}