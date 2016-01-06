using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories.Interfaces
{
    public interface IConfigurationFactory
    {
        IConfiguration Create(string configurationFile);
    }
}