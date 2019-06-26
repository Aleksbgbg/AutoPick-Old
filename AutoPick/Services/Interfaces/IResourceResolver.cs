namespace AutoPick.Services.Interfaces
{
    using AutoPick.Models;

    public interface IResourceResolver
    {
        string ResolveResourcePath(ResourceType resourceType);
    }
}