namespace AutoPick.Services.Resources
{
    using System.Linq;

    using AutoPick.Models;

    public class ChampionLoader : IChampionLoader
    {
        private readonly IResourceResolver _resourceResolver;

        private readonly IResourceReader _resourceReader;

        public ChampionLoader(IResourceResolver resourceResolver, IResourceReader resourceReader)
        {
            _resourceResolver = resourceResolver;
            _resourceReader = resourceReader;
        }

        public Champion[] LoadAllChampions()
        {
            string championNamesFile = _resourceResolver.ResolveResourcePath(ResourceType.ChampionNames);

            return _resourceReader.ReadResourceFile(championNamesFile)
                                  .Select(name => new Champion(name))
                                  .ToArray();
        }
    }
}