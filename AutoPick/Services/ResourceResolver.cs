namespace AutoPick.Services
{
    using System;
    using System.IO;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;

    public class ResourceResolver : IResourceResolver
    {
        private readonly ILocalDirectoryProvider _localDirectoryProvider;

        public ResourceResolver(ILocalDirectoryProvider localDirectoryProvider)
        {
            _localDirectoryProvider = localDirectoryProvider;
        }

        public string ResolveResourcePath(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ChampionNames:
                    return CombineWithResourcesDirectory("Champions.txt");

                case ResourceType.ChampionSquares:
                    return CombineWithResourcesDirectory("ChampionSquares");

                case ResourceType.LaneImages:
                    return CombineWithResourcesDirectory("Lanes");

                case ResourceType.DetectionImages:
                    return CombineWithResourcesDirectory("Detection");

                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, "Invalid resource type.");
            }
        }

        private string CombineWithResourcesDirectory(string resourcePath)
        {
            return Path.Combine(_localDirectoryProvider.CurrentDirectory, "Resources", resourcePath);
        }
    }
}