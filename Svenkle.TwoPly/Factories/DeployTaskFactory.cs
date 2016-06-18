using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Svenkle.TwoPly.Models.Interfaces;
using ITaskFactory = Svenkle.TwoPly.Factories.Interfaces.ITaskFactory;

namespace Svenkle.TwoPly.Factories
{
    public class DeployTaskFactory : ITaskFactory
    {
        private readonly IExecutionContext _executionContext;
        private readonly IFileSystem _fileSystem;

        public DeployTaskFactory(IExecutionContext executionContext, IFileSystem fileSystem)
        {
            _executionContext = executionContext;
            _fileSystem = fileSystem;
        }

        public bool CanCreate(IReadOnlyList<string> tokens)
        {
            if (tokens == null)
                return false;

            if (tokens.Count != 2)
                return false;

            var command = tokens.FirstOrDefault();

            return command != null &&
                command.StartsWith("Deploy", StringComparison.InvariantCultureIgnoreCase);
        }

        public ITask Create(IReadOnlyList<string> tokens)
        {
            var destinationFolder = RootPath(_executionContext.RootDirectory, tokens.ElementAt(1));
            var sourceFiles = _fileSystem.Directory.GetFiles(_executionContext.WorkingDirectory, "*", SearchOption.AllDirectories);
            var destinationFiles = sourceFiles.Select(x => _fileSystem.Path.Combine(destinationFolder, RemovePath(_executionContext.WorkingDirectory, x)));

            return new Copy
            {
                BuildEngine = _executionContext.BuildEngine,
                DestinationFiles = destinationFiles.Select(x => new TaskItem(x)).Cast<ITaskItem>().ToArray(),
                SourceFiles = sourceFiles.Select(x => new TaskItem(x)).Cast<ITaskItem>().ToArray(),
                OverwriteReadOnlyFiles = true,
                SkipUnchangedFiles = true
            };
        }

        private string RootPath(string root, string path)
        {
            return !_fileSystem.Path.IsPathRooted(path) ?
                _fileSystem.Path.Combine(root, path) : path;
        }

        private static string RemovePath(string root, string path)
        {
            return path.Replace(root, string.Empty).Trim('\\');
        }
    }
}
