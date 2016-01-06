using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class TargetPath : ITargetPath
    {
        public TargetPath(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}