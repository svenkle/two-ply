using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Svenkle.TwoPly.Models.Interfaces;
using ITaskFactory = Svenkle.TwoPly.Factories.Interfaces.ITaskFactory;

namespace Svenkle.TwoPly.Factories
{
    public class TouchTaskFactory : ITaskFactory
    {
        private readonly IExecutionContext _executionContext;
        private readonly IFileSystem _fileSystem;

        public TouchTaskFactory(IExecutionContext executionContext, IFileSystem fileSystem)
        {
            _executionContext = executionContext;
            _fileSystem = fileSystem;
        }

        public bool CanCreate(IReadOnlyList<string> tokens)
        {
            if (tokens == null)
                return false;

            if (tokens.Count < 2)
                return false;

            var command = tokens.FirstOrDefault();

            return command != null &&
                command.StartsWith("Touch", StringComparison.InvariantCultureIgnoreCase);
        }

        public ITask Create(IReadOnlyList<string> tokens)
        {
            return new Touch
            {
                BuildEngine = _executionContext.BuildEngine,
                ForceTouch = true,
                AlwaysCreate = true,
                Files = tokens.Skip(1).Take(tokens.Count - 1)
                .Select(x => new TaskItem(RootPath(_executionContext.WorkingDirectory, x)))
                .Cast<ITaskItem>()
                .ToArray()
            };
        }

        private string RootPath(string root, string path)
        {
            return !_fileSystem.Path.IsPathRooted(path) ?
                _fileSystem.Path.Combine(root, path) : path;
        }
    }
}
