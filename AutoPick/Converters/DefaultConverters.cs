namespace AutoPick.Converters
{
    using AutoPick.Services.Interfaces;

    using Caliburn.Micro;

    public static class DefaultConverters
    {
        public static ChampionImageConverter ChampionImageConverter { get; } = new ChampionImageConverter(IoC.Get<IResourceResolver>());
    }
}