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
                    return CombineWithCurrentDirectory(@"Resources\Champions.txt");

                case ResourceType.ChampionSquares:
                    return CombineWithCurrentDirectory(@"Resources\ChampionSquares");

                case ResourceType.LaneImages:
                    return CombineWithCurrentDirectory(@"Resources\Lanes");

                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, "Invalid resource type.");
            }
        }

        private string CombineWithCurrentDirectory(string resourcePath)
        {
            return Path.Combine(_localDirectoryProvider.CurrentDirectory, resourcePath);
        }
    }
}