using System.Collections.Generic;

namespace Svenkle.TwoPly.Models.Interfaces
{
    public interface IConfiguration
    {
        IEnumerable<IPublishTarget> Targets { get; }
    }
}
