namespace AutoPick.Services.Resources
{
    using System.IO;

    public class ResourceReader : IResourceReader
    {
        public string[] ReadResourceFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}