using Microsoft.Build.Framework;

namespace Svenkle.TwoPly.Models.Interfaces
{
    public interface IExecutionContext
    {
        string WorkingDirectory { get; }
        string RootDirectory { get; }
        IBuildEngine BuildEngine { get; }
    }
}