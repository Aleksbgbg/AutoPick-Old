namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System.Drawing;

    public class TemplateMatchResult
    {
        public TemplateMatchResult()
        {
        }

        public TemplateMatchResult(bool isMatch, Rectangle matchArea)
        {
            IsMatch = isMatch;
            MatchArea = matchArea;
        }

        public bool IsMatch { get; }

        public Rectangle MatchArea { get; }
    }
}