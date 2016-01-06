using Svenkle.TwoPly.Factories.Interfaces;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class PublisherSettingsFactory : IPublisherSettingsFactory
    {
        public IPublisherSettings Create(PublishTask publishTask)
        {
            return new PublisherSettings(publishTask.SkipUnchangedFiles);
        }
    }
}