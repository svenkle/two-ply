using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class PublisherSettings : IPublisherSettings
    {
        public PublisherSettings(bool skipUnchangedFiles)
        {
            SkipUnchangedFiles = skipUnchangedFiles;
        }

        public bool SkipUnchangedFiles { get; }
    }
}
