using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class XmlTransform : IXmlTransform
    {
        public XmlTransform(string source, string transform)
        {
            Source = source;
            Transform = transform;
        }

        public string Source { get; }
        public string Transform { get; }
    }
}
