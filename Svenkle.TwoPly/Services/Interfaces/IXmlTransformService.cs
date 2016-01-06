namespace Svenkle.TwoPly.Services.Interfaces
{
    public interface IXmlTransformService
    {
        bool Transform(string source, string transform, string destination);
    }
}