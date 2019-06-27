namespace AutoPick.Services.Resources
{
    public interface IResourceReader
    {
        string[] ReadResourceFile(string path);
    }
}