using System.IO.Abstractions;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class ExecutionContextFactory
    {
        private readonly IFileSystem _fileSystem;

        public ExecutionContextFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IExecutionContext Create(string id, IGlobalContext globalContext)
        {
            var workingDirectory = _fileSystem.Path.Combine(globalContext.WorkingDirectory, id);
            return new ExecutionContext(workingDirectory, globalContext.BuildEngine, globalContext.RootDirectory);
        }
    }
}