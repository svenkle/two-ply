using System.IO.Abstractions;
using System.Linq;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class GlobalContextFactory
    {
        private readonly IFileSystem _fileSystem;

        public GlobalContextFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        
        public IGlobalContext Create(TwoPly twoPly)
        {
            var rootDirectory = twoPly.RootDirectory;
            var workingDirectory = RootPath(rootDirectory, twoPly.WorkingDirectory);
            var configurationFile = RootPath(rootDirectory, twoPly.ConfigurationFile);
            var sourceFiles = twoPly.SourceFiles.Select(x => x.ItemSpec).ToArray();
            return new GlobalContext(workingDirectory, configurationFile, rootDirectory, twoPly.BuildEngine, sourceFiles);
        }

        private string RootPath(string root, string path)
        {
            return !_fileSystem.Path.IsPathRooted(path) ?
                _fileSystem.Path.Combine(root, path) : path;
        }
    }
}