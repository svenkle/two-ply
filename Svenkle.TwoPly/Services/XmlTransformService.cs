using System;
using System.IO.Abstractions;
using Microsoft.Web.XmlTransform;
using Svenkle.TwoPly.Services.Interfaces;

namespace Svenkle.TwoPly.Services
{
    public class XmlTransformService : IXmlTransformService
    {
        private readonly IFileSystem _fileSystem;
        public XmlTransformService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public bool Transform(string source, string transform, string destination)
        {
            if (string.IsNullOrEmpty(destination))
                throw new ArgumentException("destination cannot be null or empty");

            var sourceContent = _fileSystem.File.ReadAllText(source);
            var transformation = new XmlTransformation(_fileSystem.File.OpenRead(transform), null);

            var sourceDocument = new XmlTransformableDocument
            {
                PreserveWhitespace = true
            };

            sourceDocument.LoadXml(sourceContent);
            transformation.Apply(sourceDocument);
            _fileSystem.File.WriteAllText(destination, sourceDocument.OuterXml);

            return true;
        }
    }
}