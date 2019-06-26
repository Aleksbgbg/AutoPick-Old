namespace AutoPick.Services
{
    using System;

    using AutoPick.Services.Interfaces;

    public class LocalDirectoryProvider : ILocalDirectoryProvider
    {
        public string CurrentDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }
}