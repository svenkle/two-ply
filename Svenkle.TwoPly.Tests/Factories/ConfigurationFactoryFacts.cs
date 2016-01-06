using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Svenkle.TwoPly.Factories;
using Svenkle.TwoPly.Factories.Interfaces;
using Xunit;

namespace Svenkle.TwoPly.Tests.Factories
{
    public class ConfigurationFactoryFacts
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfigurationFactory _configurationFactory;

        public ConfigurationFactoryFacts()
        {
            ITransformFactory transformFactory = new TransformFactory();
            ITargetPathFactory targetPathFactory = new TargetPathFactory();
            _fileSystem = new MockFileSystem();
            _configurationFactory = new ConfigurationFactory(_fileSystem, transformFactory, targetPathFactory);
        }

        public class TheCreateMethod : ConfigurationFactoryFacts
        {
            [Fact]
            public void ThrowsAnArgumentExceptionWhenTheConfigurationFileHasTransformsWithoutAPath()
            {
                // Prepare
                var configurationFile = _fileSystem.Path.GetRandomFileName();
                var rawConfiguration = new[]
                {
                    "Web.config => XmlTransform.config"
                };

                _fileSystem.File.WriteAllLines(configurationFile, rawConfiguration);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => _configurationFactory.Create(configurationFile));
            }
            
            [Fact]
            public void ThrowsAnArgumentExceptionWhenThereAreDuplicatePathValues()
            {
                // Prepare
                var configurationFile = _fileSystem.Path.GetRandomFileName();
                var rawConfiguration = new[]
                {
                    "Path = C:\\Temp",
                    "Path = C:\\Temp"
                };

                _fileSystem.File.WriteAllLines(configurationFile, rawConfiguration);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => _configurationFactory.Create(configurationFile));
            }


            [Fact]
            public void ThrowsAnArgumentExceptionWhenTheConfigurationFileParameterIsNull()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _configurationFactory.Create(null));
            }

            [Fact]
            public void ThrowsAFileNotFoundExceptionWhenTheConfigurationFileIsNotFound()
            {
                // Prepare
                var configurationFile = _fileSystem.Path.GetRandomFileName();

                // Act & Assert
                Assert.Throws<FileNotFoundException>(() => _configurationFactory.Create(configurationFile));
            }

            [Fact]
            public void IgnoresLinesStartingWithTheCommentCharacter()
            {
                // Prepare
                var configurationFile = _fileSystem.Path.GetRandomFileName();
                var rawConfiguration = new[]
                {
                    "#Path = C:\\Sample",
                    "Path = C:\\Temp"
                };

                _fileSystem.File.WriteAllLines(configurationFile, rawConfiguration);

                // Act
                var configuration = _configurationFactory.Create(configurationFile);

                // Assert
                Assert.NotNull(configuration);
                Assert.NotEmpty(configuration.Targets);
                Assert.True(configuration.Targets.Count() == 1);
            }

            [Fact]
            public void IgnoresWhiteSpaceAndEmptyLines()
            {
                // Prepare
                var configurationFile = _fileSystem.Path.GetRandomFileName();
                var rawConfiguration = new[]
                {
                    "",
                    " ",
                    "Path = C:\\Temp"
                };

                _fileSystem.File.WriteAllLines(configurationFile, rawConfiguration);

                // Act
                var configuration = _configurationFactory.Create(configurationFile);

                // Assert
                Assert.NotNull(configuration);
                Assert.NotEmpty(configuration.Targets);
                Assert.True(configuration.Targets.Count() == 1);
            }
        }
    }
}
