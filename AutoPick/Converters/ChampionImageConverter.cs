namespace AutoPick.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class ChampionImageConverter : IValueConverter
    {
        private readonly IResourceResolver _resourceResolver;

        public ChampionImageConverter(IResourceResolver resourceResolver)
        {
            _resourceResolver = resourceResolver;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string championName = (string)value;
            string championSquaresDirectory = _resourceResolver.ResolveResourcePath(ResourceType.ChampionSquares);

            return Path.Combine(championSquaresDirectory, string.Concat(championName, ".png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}