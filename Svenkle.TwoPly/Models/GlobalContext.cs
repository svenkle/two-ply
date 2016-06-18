using Microsoft.Build.Framework;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class GlobalContext : IGlobalContext
    {
        public GlobalContext(string workingDirectory, string configurationFile, string rootDirectory, IBuildEngine buildEngine, string[] sourceFiles)
        {
            WorkingDirectory = workingDirectory;
            ConfigurationFile = configurationFile;
            RootDirectory = rootDirectory;
            BuildEngine = buildEngine;
            SourceFiles = sourceFiles;
        }

        public string WorkingDirectory { get; }
        public string RootDirectory { get; }
        public string ConfigurationFile { get; }
        public string[] SourceFiles { get; }
        public IBuildEngine BuildEngine { get; }
    }
}
