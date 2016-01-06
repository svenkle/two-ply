using System.Collections.Generic;

namespace Svenkle.TwoPly.Models.Interfaces
{
    public interface IPublishTarget
    {
        ITargetPath TargetPath { get; }
        IEnumerable<IXmlTransform> Transforms { get; } 
    }
}