using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Svenkle.TwoPly.Models.Interfaces;
using Svenkle.TwoPly.Services.Interfaces;

namespace Svenkle.TwoPly
{
    public class Publisher
    {
        private readonly IFileCopyService _fileCopyService;
        private readonly IPublisherSettings _publisherSettings;
        private readonly IXmlTransformService _xmlTransformService;
        private readonly IFileSystem _fileSystem;

        public Publisher(IPublisherSettings publisherSettings, IFileCopyService fileCopyService, 
            IFileSystem fileSystem, IXmlTransformService xmlTransformService)
        {
            _publisherSettings = publisherSettings;
            _fileCopyService = fileCopyService;
            _fileSystem = fileSystem;
            _xmlTransformService = xmlTransformService;
        }

        public void Publish(string destinationFolder, IEnumerable<string> packageFiles, IEnumerable<IXmlTransform> transforms)
        {
            var sourceFiles = packageFiles as string[] ?? packageFiles.ToArray();
            var destinationFiles = sourceFiles.Select(x => _fileSystem.Path.Combine(destinationFolder, x)).ToArray();

            _fileCopyService.Copy(sourceFiles, destinationFiles, _publisherSettings.SkipUnchangedFiles);

            foreach (var transform in transforms)
            {
                _xmlTransformService.Transform(transform.Source, transform.Transform,
                    _fileSystem.Path.Combine(destinationFolder, transform.Source));
            }
        }
    }
}