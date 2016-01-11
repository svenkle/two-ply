using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Svenkle.TwoPly.Factories;
using Svenkle.TwoPly.Factories.Interfaces;
using Svenkle.TwoPly.Services;
using Svenkle.TwoPly.Services.Interfaces;

namespace Svenkle.TwoPly
{
    public class PublishTask : Microsoft.Build.Utilities.Task
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfigurationFactory _configurationFactory;
        private readonly IXmlTransformService _xmlTransformService;
        private readonly IFileCopyService _fileCopyService;
        private readonly IPublisherSettingsFactory _publisherSettingsFactory;

        public PublishTask()
        {
            _fileSystem = new FileSystem();
            _configurationFactory = new ConfigurationFactory(_fileSystem, new TransformFactory(), new TargetPathFactory());
            _fileCopyService = new FileCopyService(_fileSystem);
            _xmlTransformService = new XmlTransformService(_fileSystem);
            _publisherSettingsFactory = new PublisherSettingsFactory();
        }
        
        public override bool Execute()
        {
            try
            {
                var configuration = _configurationFactory.Create(ConfigurationFile);
                Parallel.ForEach(configuration.Targets, target =>
                {
                    var publisherSettings = _publisherSettingsFactory.Create(this);
                    var publisher = new Publisher(publisherSettings, _fileCopyService, _fileSystem, _xmlTransformService);
                    publisher.Publish(target.TargetPath.Path, SourceFiles.Select(x => x.ItemSpec), target.Transforms);
                });

                return true;
            }
            catch (Exception exception)
            {
                Log.LogErrorFromException(exception);
                return false;
            }
        }

        [Required]
        public bool SkipUnchangedFiles { get; set; }

        [Required]
        public string ConfigurationFile { get; set; }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }
    }
}
