namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    public class TemplateMatchResult
    {
        public TemplateMatchResult(Rectangle matchArea)
        {
            IsMatch = true;
            MatchArea = matchArea;
        }

        private TemplateMatchResult()
        {
        }

        public static TemplateMatchResult Failed { get; } = new TemplateMatchResult();

        public bool IsMatch { get; }

        public Rectangle MatchArea { get; }
    }
}