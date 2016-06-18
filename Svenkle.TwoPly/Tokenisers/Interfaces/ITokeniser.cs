using System.Collections.Generic;

namespace Svenkle.TwoPly.Tokenisers.Interfaces
{
    public interface ITokeniser
    {
        bool CanTokenise(string value);
        IEnumerable<string> Tokenise(string value);
    }
}