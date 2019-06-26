namespace AutoPick.Converters
{
    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    using Caliburn.Micro;

    public static class DefaultConverters
    {
        public static ResourceImageConverter ChampionImageConverter { get; } = new ResourceImageConverter(IoC.Get<IResourceResolver>(), ResourceType.ChampionSquares);

        public static ResourceImageConverter LaneImageConverter { get; } = new ResourceImageConverter(IoC.Get<IResourceResolver>(), ResourceType.LaneImages);
    }
}