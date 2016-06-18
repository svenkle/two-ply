using System.Collections.Generic;
using System.IO.Abstractions;

namespace Svenkle.TwoPly.Readers
{
    public class ConfigurationReader
    {
        private readonly IFileSystem _fileSystem;

        public ConfigurationReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<string> Read(string configurationFile)
        {
            return _fileSystem.File.ReadAllLines(configurationFile);
        }
    }
}
