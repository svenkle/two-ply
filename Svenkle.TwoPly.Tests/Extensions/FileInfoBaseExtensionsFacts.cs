using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Svenkle.TwoPly.Extensions;
using Xunit;

namespace Svenkle.TwoPly.Tests.Extensions
{
    public class FileInfoBaseExtensionsFacts
    {
        private readonly IFileSystem _fileSystem;

        public FileInfoBaseExtensionsFacts()
        {
            _fileSystem = new MockFileSystem();
        }

        public class TheToHashMethod : FileInfoBaseExtensionsFacts
        {
            [Fact]
            public void ReturnsTheSha1HashOfAFile()
            {
                // Prepare
                const string expected = "05993E69C1712B1A21928277C8ABFDC2AE39C214";
                var filename = _fileSystem.Path.GetRandomFileName();
                _fileSystem.File.WriteAllText(filename, "SAMPLE");

                // Act
                var fileInfo = _fileSystem.FileInfo.FromFileName(filename);

                // Assert
                Assert.Equal(fileInfo.ToHash(), expected);
            }
        }
    }
}
