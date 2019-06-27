namespace AutoPick.Services.Resources
{
    using AutoPick.Models;

    public interface IResourceResolver
    {
        string ResolveResourcePath(ResourceType resourceType);
    }
}