using System.Collections.Generic;

namespace Svenkle.TwoPly.Services.Interfaces
{
    public interface IFileCopyService
    {
        bool Copy(IEnumerable<string> sourceFiles, IEnumerable<string> destinationFiles, bool skipUnchangedFiles);
        bool Copy(IEnumerable<string> sourceFiles, IEnumerable<string> destinationFiles);
    }
}