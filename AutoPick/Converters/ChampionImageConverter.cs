namespace AutoPick.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ChampionImageConverter : IValueConverter
    {
        public static ChampionImageConverter Default { get; } = new ChampionImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}