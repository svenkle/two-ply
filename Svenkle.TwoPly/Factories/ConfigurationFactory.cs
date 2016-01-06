using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Svenkle.TwoPly.Extensions;
using Svenkle.TwoPly.Factories.Interfaces;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITransformFactory _transformFactory;
        private readonly ITargetPathFactory _targetPathFactory;

        public ConfigurationFactory(IFileSystem fileSystem, ITransformFactory transformFactory, ITargetPathFactory targetPathFactory)
        {
            _fileSystem = fileSystem;
            _transformFactory = transformFactory;
            _targetPathFactory = targetPathFactory;
        }

        public IConfiguration Create(string configurationFile)
        {
            if(configurationFile == null)
                throw new ArgumentException("configurationFile cannot be null");

            var configurationData = _fileSystem.File.ReadAllLines(configurationFile);
            var targets = new List<IPublishTarget>();
            var configuration = new Configuration(targets);
            var transformsBuffer = new List<IXmlTransform>();
            var element = 0;

            while (element < configurationData.Length)
            {
                var line = configurationData[element]?.Trim() ?? string.Empty;
                var commandLine = line.RemoveWhitespace();
                element++;

                if (string.IsNullOrEmpty(commandLine) || commandLine.StartsWith("#"))
                    continue;

                if (commandLine.StartsWith("path=", StringComparison.InvariantCultureIgnoreCase))
                {
                    targets.Add(new PublishTarget(_targetPathFactory.Create(line), transformsBuffer.ToList()));
                    transformsBuffer.Clear();
                    continue;
                }

                if (line.Contains("=>"))
                {
                    transformsBuffer.Add(_transformFactory.Create(line));
                    continue;
                }
            }
            
            if (!targets.Any() && transformsBuffer.Any())
                throw new ArgumentException("No destination path was found");

            var paths = targets.Select(x => x.TargetPath.Path).ToArray();
            if (paths.Length != paths.Distinct().Count())
                throw new ArgumentException("Duplicate destination paths found");

            return configuration;
        }
    }
}