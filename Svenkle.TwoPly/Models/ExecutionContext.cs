using Microsoft.Build.Framework;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext(string workingDirectory, IBuildEngine buildEngine, string rootDirectory)
        {
            WorkingDirectory = workingDirectory;
            BuildEngine = buildEngine;
            RootDirectory = rootDirectory;
        }

        public string WorkingDirectory { get; }
        public string RootDirectory { get; }
        public IBuildEngine BuildEngine { get; }
    }
}
