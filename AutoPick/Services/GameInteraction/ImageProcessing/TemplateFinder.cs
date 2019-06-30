namespace AutoPick.Services.GameInteraction.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Numerics;

    public class TemplateFinder : ITemplateFinder
    {
        private const int MatchMargin = 2;

        private const double MatchThreshold = 0.75;

        private static readonly Vector2 DefaultImageSize = new Vector2(1024f, 576f);

        private readonly IImage _template;

        private readonly Vector2 _normalizedTargetLocation;

        private Vector2 _lastImageSize = DefaultImageSize;

        public TemplateFinder(IImage template, Vector2 normalizedTargetLocation)
        {
            _template = template;
            _normalizedTargetLocation = normalizedTargetLocation;
        }

        public TemplateMatchResult FindTemplateIn(IImage image)
        {
            Vector2 currentImageSize = new Vector2(image.Width, image.Height);

            float lastImageLength = _lastImageSize.Length();
            float currentImageLength = currentImageSize.Length();

            float aspectRatio = currentImageLength / DefaultImageSize.Length();

            if (Math.Abs(lastImageLength - currentImageLength) > 0.01)
            {
                _template.Resize(aspectRatio);
            }

            _lastImageSize = currentImageSize;

            int subRegionMargin = (int)(MatchMargin * aspectRatio);
            int xOffset = (int)(image.Width * _normalizedTargetLocation.X) - subRegionMargin;
            int yOffset = (int)(image.Height * _normalizedTargetLocation.Y) - subRegionMargin;
            Rectangle targetSubRegion = new Rectangle(xOffset,
                                                      yOffset,
                                                      _template.Width + (2 * subRegionMargin),
                                                      _template.Height + (2 * subRegionMargin));

            IImage subImage = image.SubImage(targetSubRegion);

            TemplateMatchResult result = subImage.MatchTemplate(_template, MatchThreshold);

            if (result.IsMatch)
            {
                return new TemplateMatchResult(new Rectangle(result.MatchArea.X + xOffset,
                                                             result.MatchArea.Y + yOffset,
                                                             result.MatchArea.Width,
                                                             result.MatchArea.Height));
            }

            return result;
        }
    }
}