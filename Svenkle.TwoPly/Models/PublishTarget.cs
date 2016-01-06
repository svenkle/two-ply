using System.Collections.Generic;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class PublishTarget : IPublishTarget
    {
        public PublishTarget(ITargetPath targetPath, IEnumerable<IXmlTransform> transforms)
        {
            TargetPath = targetPath;
            Transforms = transforms;
        }

        public ITargetPath TargetPath { get; }
        public IEnumerable<IXmlTransform> Transforms { get; }
    }
}
