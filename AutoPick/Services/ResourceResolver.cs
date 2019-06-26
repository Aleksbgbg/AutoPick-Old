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
                    return Path.Combine(_localDirectoryProvider.CurrentDirectory, @"Resources\Champions.txt");

                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, "Invalid resource type.");
            }
        }
    }
}