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
    public class MoveTaskFactory : ITaskFactory
    {
        private readonly IExecutionContext _executionContext;
        private readonly IFileSystem _fileSystem;

        public MoveTaskFactory(IExecutionContext executionContext, IFileSystem fileSystem)
        {
            _executionContext = executionContext;
            _fileSystem = fileSystem;
        }

        public bool CanCreate(IReadOnlyList<string> tokens)
        {
            if (tokens == null)
                return false;

            if (tokens.Count < 3)
                return false;

            var command = tokens.FirstOrDefault();

            return command != null &&
                command.StartsWith("Move", StringComparison.InvariantCultureIgnoreCase);
        }

        public ITask Create(IReadOnlyList<string> tokens)
        {
            var source = RootPath(_executionContext.WorkingDirectory, tokens.ElementAt(1));

            return new Move
            {
                BuildEngine = _executionContext.BuildEngine,
                SourceFiles = new[] { new TaskItem(source) as ITaskItem },
                DestinationFolder = new TaskItem(tokens.ElementAt(2)),
                OverwriteReadOnlyFiles = true
            };
        }

        private string RootPath(string root, string path)
        {
            return !_fileSystem.Path.IsPathRooted(path) ?
                _fileSystem.Path.Combine(root, path) : path;
        }

        private bool IsDirectory(string path)
        {
            var fileInfo = _fileSystem.FileInfo.FromFileName(path);
            return fileInfo.Attributes.HasFlag(FileAttributes.Directory);
        }
    }
}
