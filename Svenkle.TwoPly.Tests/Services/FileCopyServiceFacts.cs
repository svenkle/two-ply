using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Svenkle.TwoPly.Services;
using Svenkle.TwoPly.Services.Interfaces;
using Xunit;

namespace Svenkle.TwoPly.Tests.Services
{
    public class FileCopyServiceFacts
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileCopyService _fileCopyService;

        public FileCopyServiceFacts()
        {
            _fileSystem = new MockFileSystem();
            _fileCopyService = new FileCopyService(_fileSystem);
        }

        public class TheCopyMethod : FileCopyServiceFacts
        {
            [Fact]
            public void PerformsASimpleFileCopyCorrectly()
            {
                // Prepare
                var sourceFolder = Guid.NewGuid().ToString("N");
                var sourceFile = _fileSystem.Path.GetRandomFileName();
                var sourcePath = _fileSystem.Path.Combine(sourceFolder, sourceFile);
                var destinationFolder = Guid.NewGuid().ToString("N");
                var destinationPath = _fileSystem.Path.Combine(destinationFolder, sourceFile);
                _fileSystem.File.WriteAllText(sourcePath, string.Empty);

                // Act
                _fileCopyService.Copy(new[] { sourcePath }, new[] { destinationPath });

                // Assert
                Assert.True(_fileSystem.File.Exists(destinationPath));
            }

            [Fact]
            public void CreatesDestinationDirectoriesIfTheyDontExist()
            {
                // Prepare
                var sourceFolder = Guid.NewGuid().ToString("N");
                var sourceFile = _fileSystem.Path.GetRandomFileName();
                var sourcePath = _fileSystem.Path.Combine(sourceFolder, sourceFile);
                var destinationFolder = Guid.NewGuid().ToString("N");
                var destinationPath = _fileSystem.Path.Combine(destinationFolder, sourceFile);
                _fileSystem.File.WriteAllText(sourcePath, string.Empty);

                // Act
                _fileCopyService.Copy(new[] { sourcePath }, new[] { destinationPath });

                // Assert
                Assert.True(_fileSystem.Directory.Exists(destinationFolder));
            }

            [Fact]
            public void ThrowsAnArgumentExceptionIfTheFileArraysLengthsAreDifferent()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _fileCopyService.Copy(new[] { "SOURCE" }, new[] { "DESTINATION", "DESTINATION" }));
            }
        }
    }
}
