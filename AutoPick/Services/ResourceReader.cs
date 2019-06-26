namespace AutoPick.Services
{
    using System.IO;

    using AutoPick.Services.Interfaces;

    public class ResourceReader : IResourceReader
    {
        public string[] ReadResourceFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}