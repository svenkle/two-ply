using System;
using System.Collections.Generic;
using System.Linq;
using Svenkle.ExtensionMethods;
using Svenkle.TwoPly.Tokenisers.Interfaces;

namespace Svenkle.TwoPly.Tokenisers
{
    public class ConfigurationTokeniser : IConfigurationTokeniser
    {
        private readonly IEnumerable<ITokeniser> _commandTokenisers;

        public ConfigurationTokeniser(IEnumerable<ITokeniser> commandTokenisers)
        {
            _commandTokenisers = commandTokenisers;
        }

        public Dictionary<string, List<List<string>>> Tokenise(IEnumerable<string> configuration)
        {
            var buffer = new List<List<string>>();
            var commandBuffer = new Dictionary<string, List<List<string>>>();

            foreach (var line in configuration)
            {
                var lastCommand = string.Empty;
                foreach (var tokeniser in _commandTokenisers)
                {
                    if (!tokeniser.CanTokenise(line))
                        continue;

                    var tokens = tokeniser.Tokenise(line).ToList();
                    lastCommand = tokens.FirstOrDefault();
                    buffer.Add(tokens);
                    break;
                }

                if (string.IsNullOrWhiteSpace(lastCommand) || !lastCommand.Equals("Deploy", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var key = string.Join("|", buffer.SelectMany(x => x)).ToHash();
                commandBuffer[key] = buffer.ToList();
                buffer.Clear();
            }

            return commandBuffer;
        }
    }
}