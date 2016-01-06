using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories.Interfaces
{
    public interface ITransformFactory
    {
        IXmlTransform Create(string syntax);
    }
}