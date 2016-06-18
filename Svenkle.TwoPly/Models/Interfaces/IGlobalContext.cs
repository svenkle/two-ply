using Microsoft.Build.Framework;

namespace Svenkle.TwoPly.Models.Interfaces
{
    public interface IGlobalContext
    {
        string WorkingDirectory { get; }
        string RootDirectory { get; }
        string ConfigurationFile { get; }
        string[] SourceFiles { get; }
        IBuildEngine BuildEngine { get; }
    }
}