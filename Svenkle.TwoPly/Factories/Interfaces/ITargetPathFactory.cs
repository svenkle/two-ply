using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories.Interfaces
{
    public interface ITargetPathFactory
    {
        ITargetPath Create(string syntax);
    }
}