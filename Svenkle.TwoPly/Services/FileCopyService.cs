using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Polly;
using Svenkle.TwoPly.Extensions;
using Svenkle.TwoPly.Services.Interfaces;

namespace Svenkle.TwoPly.Services
{
    public class FileCopyService : IFileCopyService
    {
        private readonly IFileSystem _fileSystem;

        public FileCopyService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public bool Copy(IEnumerable<string> sourceFiles, IEnumerable<string> destinationFiles)
        {
            return Copy(sourceFiles, destinationFiles, false);
        }

        public bool Copy(IEnumerable<string> sourceFiles, IEnumerable<string> destinationFiles, bool skipUnchangedFiles)
        {
            if (sourceFiles == null)
                throw new ArgumentException("sourceFiles cannot be null");

            if (destinationFiles == null)
                throw new ArgumentException("destinationFiles cannot be null");

            var sourceFilesList = sourceFiles as IList<string> ?? sourceFiles.ToList();
            var destinationFilesList = destinationFiles as IList<string> ?? destinationFiles.ToList();

            if (sourceFilesList.Count != destinationFilesList.Count)
                throw new ArgumentException("sourceFiles and destinationFiles arrays must be the same length");

            var retryPolicy = Policy
                    .Handle<IOException>()
                    .WaitAndRetry(10, x => TimeSpan.FromSeconds(1));

            var knownDirectories = new HashSet<string>();

            for (var i = 0; i < sourceFilesList.Count; i++)
            {
                var source = sourceFilesList.ElementAt(i);
                var destination = destinationFilesList.ElementAt(i);
                var targetDirectoryIsNew = false;

                var targetDirectory = _fileSystem.Path.GetDirectoryName(destination);
                if (!knownDirectories.Contains(targetDirectory))
                {
                    if (!_fileSystem.Directory.Exists(targetDirectory))
                    {
                        _fileSystem.Directory.CreateDirectory(targetDirectory);
                        targetDirectoryIsNew = true;
                    }

                    knownDirectories.Add(targetDirectory);
                }

                if (targetDirectoryIsNew || !skipUnchangedFiles || !FilesMatch(source, destination))
                    retryPolicy.Execute(() => _fileSystem.File.Copy(source, destination, true));
            }

            return true;
        }

        private bool FilesMatch(string sourceFile, string destinationFile)
        {
            var source = _fileSystem.FileInfo.FromFileName(sourceFile);
            var destination = _fileSystem.FileInfo.FromFileName(destinationFile);

            if (!destination.Exists || !source.Exists)
                return false;

            if (source.LastWriteTimeUtc != destination.LastWriteTimeUtc)
                return source.ToHash() == destination.ToHash();

            return true;
        }
    }
}
