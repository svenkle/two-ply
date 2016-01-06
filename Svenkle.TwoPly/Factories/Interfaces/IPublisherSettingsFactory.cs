using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories.Interfaces
{
    public interface IPublisherSettingsFactory
    {
        IPublisherSettings Create(PublishTask publishTask);
    }
}
