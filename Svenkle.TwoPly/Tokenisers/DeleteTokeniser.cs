using System.Collections.Generic;
using System.Linq;
using Svenkle.ExtensionMethods;
using Svenkle.TwoPly.Tokenisers.Interfaces;

namespace Svenkle.TwoPly.Tokenisers
{
    public class DeleteTokeniser : ITokeniser
    {
        public bool CanTokenise(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var sanitsedValue = value
                .RemoveWhitespace()
                .ToLowerInvariant();

            return sanitsedValue.StartsWith("delete");
        }

        public IEnumerable<string> Tokenise(string value)
        {
            return value.Split(" ")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
    }
}
