namespace AutoPick.Services.Resources
{
    using System;

    public class LocalDirectoryProvider : ILocalDirectoryProvider
    {
        public string CurrentDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }
}