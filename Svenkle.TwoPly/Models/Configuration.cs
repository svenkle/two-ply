using System.Collections.Generic;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Models
{
    public class Configuration : IConfiguration
    {
        public Configuration(IEnumerable<IPublishTarget> targets)
        {
            Targets = targets;
        }

        public IEnumerable<IPublishTarget> Targets { get; }
    }
}