namespace AutoPick.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    using AutoPick.Models;
    using AutoPick.Services.Resources;

    public class ResourceImageConverter : IValueConverter
    {
        private readonly IResourceResolver _resourceResolver;

        private readonly ResourceType _resourceType;

        public ResourceImageConverter(IResourceResolver resourceResolver, ResourceType resourceType)
        {
            _resourceResolver = resourceResolver;
            _resourceType = resourceType;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string championName = (string)value;
            string championSquaresDirectory = _resourceResolver.ResolveResourcePath(_resourceType);

            return Path.Combine(championSquaresDirectory, string.Concat(championName, ".png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}