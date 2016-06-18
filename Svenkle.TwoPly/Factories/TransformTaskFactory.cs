using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Build.Framework;
using Svenkle.TwoPly.Models.Interfaces;
using Svenkle.TwoPly.Services.Interfaces;
using Svenkle.TwoPly.Tasks;
using ITaskFactory = Svenkle.TwoPly.Factories.Interfaces.ITaskFactory;

namespace Svenkle.TwoPly.Factories
{
    public class TransformTaskFactory : ITaskFactory
    {
        private readonly IExecutionContext _executionContext;
        private readonly IXmlTransformService _xmlTransformService;
        private readonly IFileSystem _fileSystem;

        public TransformTaskFactory(IExecutionContext executionContext, IXmlTransformService xmlTransformService, IFileSystem fileSystem)
        {
            _executionContext = executionContext;
            _xmlTransformService = xmlTransformService;
            _fileSystem = fileSystem;
        }

        public bool CanCreate(IReadOnlyList<string> tokens)
        {
            if (tokens == null)
                return false;

            if (tokens.Count < 3 && tokens.Count > 4)
                return false;

            var command = tokens.FirstOrDefault();

            return command != null && command.StartsWith("Transform", StringComparison.InvariantCultureIgnoreCase);
        }

        public ITask Create(IReadOnlyList<string> tokens)
        {
            var source = tokens.ElementAt(1);
            var destination = tokens.ElementAtOrDefault(3);
            if (string.IsNullOrWhiteSpace(destination))
                destination = RootPath(_executionContext.WorkingDirectory, _fileSystem.Path.GetFileName(source));

            return new Transform(_xmlTransformService)
            {
                BuildEngine = _executionContext.BuildEngine,
                SourceFile = source,
                TransformFile = tokens.ElementAt(2),
                DestinationFile = destination
            };
        }

        private string RootPath(string root, string path)
        {
            return !_fileSystem.Path.IsPathRooted(path) ?
                _fileSystem.Path.Combine(root, path) : path;
        }
    }
}
