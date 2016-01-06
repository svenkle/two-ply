using System;
using System.Linq;
using Svenkle.TwoPly.Extensions;
using Svenkle.TwoPly.Factories.Interfaces;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class TransformFactory : ITransformFactory
    {
        public IXmlTransform Create(string syntax)
        {
            var elements = syntax.Split("=>")
                                .Select(x => x.Trim())
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToArray();

            if (elements.Length > 2)
                throw new ArgumentException("You cannot specify multiple transforms in a single line");

            if (elements.Length != 2)
                throw new ArgumentException("XmlTransform syntax is malformed or invalid");

            return new XmlTransform(elements[1], elements[0]);
        }
    }
}