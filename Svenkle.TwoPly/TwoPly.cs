using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Svenkle.TwoPly.Factories;
using Svenkle.TwoPly.Models.Interfaces;
using Svenkle.TwoPly.Readers;
using Svenkle.TwoPly.Services;
using Svenkle.TwoPly.Tokenisers;
using Svenkle.TwoPly.Tokenisers.Interfaces;

namespace Svenkle.TwoPly
{
    public class TwoPly : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            var status = true;
            
            var fileSystem = new FileSystem();
            var configurationReader = new ConfigurationReader(fileSystem);
            var globalContextFactory = new GlobalContextFactory(fileSystem);
            var executionContextFactory = new ExecutionContextFactory(fileSystem);
            var xmlTransformService = new XmlTransformService(fileSystem);
            var configurationTokeniser = new ConfigurationTokeniser(_commandTokenisers);
            
            var globalContext = globalContextFactory.Create(this);
            
            var configurationData = configurationReader.Read(globalContext.ConfigurationFile);
            var tokenisedConfigurationData = configurationTokeniser.Tokenise(configurationData);

            if (tokenisedConfigurationData.Any())
            {
                Parallel.ForEach(tokenisedConfigurationData, target =>
                {
                    try
                    {
                        var executionContext = executionContextFactory.Create(target.Key, globalContext);

                        PrepareWorkingDirectory(globalContext, fileSystem, executionContext);
                        
                        var taskFactories = new Factories.Interfaces.ITaskFactory[]
                        {
                            new TransformTaskFactory(executionContext, xmlTransformService, fileSystem),
                            new DeployTaskFactory(executionContext, fileSystem),
                            new CopyTaskFactory(executionContext, fileSystem),
                            new DeleteTaskFactory(executionContext,fileSystem),
                            new MoveTaskFactory(executionContext,fileSystem),
                            new TouchTaskFactory(executionContext, fileSystem)
                        };
                        
                        foreach (var taskCommand in target.Value)
                        {
                            foreach (var taskFactory in taskFactories)
                            {
                                if (taskFactory.CanCreate(taskCommand))
                                {
                                    var command = taskFactory.Create(taskCommand);
                                    command.Execute();
                                }
                            }
                        }
                        
                        fileSystem.Directory.Delete(executionContext.WorkingDirectory, true);
                    }
                    catch (Exception exception)
                    {
                        Log.LogErrorFromException(exception);
                        status = false;
                    }
                });
            }

            return status;
        }

        private static void PrepareWorkingDirectory(IGlobalContext globalContext, IFileSystem fileSystem, IExecutionContext executionContext)
        {
            if (fileSystem.Directory.Exists(executionContext.WorkingDirectory))
                fileSystem.Directory.Delete(executionContext.WorkingDirectory, true);

            fileSystem.Directory.CreateDirectory(executionContext.WorkingDirectory);
            
            var configurationFilePath = fileSystem.Path.GetFileName(globalContext.ConfigurationFile);

            var sourceFiles = globalContext.SourceFiles
                .Where(x => !x.Equals(configurationFilePath, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            var destinationFiles = sourceFiles.Select(x => new TaskItem(fileSystem.Path.Combine(executionContext.WorkingDirectory, x)));

            var copyCommand = new Copy
            {
                BuildEngine = executionContext.BuildEngine,
                DestinationFiles = destinationFiles.Cast<ITaskItem>().ToArray(),
                SourceFiles = sourceFiles.Select(x => new TaskItem(x)).Cast<ITaskItem>().ToArray()
            };

            copyCommand.Execute();
        }

        private readonly ITokeniser[] _commandTokenisers = {
            new TransformTokeniser(),
            new DeployTokeniser(),
            new CopyTokeniser(),
            new DeleteTokeniser(),
            new MoveTokeniser(),
            new TouchTokeniser()
        };

        [Required]
        public string WorkingDirectory { get; set; }

        [Required]
        public string RootDirectory { get; set; }

        [Required]
        public string ConfigurationFile { get; set; }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }
    }
}
