using System.Collections.Generic;

namespace Svenkle.TwoPly.Tokenisers.Interfaces
{
    public interface IConfigurationTokeniser
    {
        Dictionary<string, List<List<string>>> Tokenise(IEnumerable<string> configuration);
    }
}