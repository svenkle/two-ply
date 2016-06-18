using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Svenkle.TwoPly.Tokenisers.Interfaces;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class ConfigurationTokeniserFacts
    {
        private readonly IConfigurationTokeniser _configurationTokeniser;

        public ConfigurationTokeniserFacts()
        {
            var tokenisers = new ITokeniser[]
            {
                new TransformTokeniser(),
                new CopyTokeniser(),
                new DeleteTokeniser(),
                new MoveTokeniser(),
                new TouchTokeniser(),
                new DeployTokeniser()
            };

            _configurationTokeniser = new ConfigurationTokeniser(tokenisers);
        }

        public class TheCreateMethod : ConfigurationTokeniserFacts
        {
            [Fact]
            public void IgnoresLinesStartingWithTheCommentCharacter()
            {
                // Prepare
                var configurationData = new[] { "#Comment", "#Deploy" };

                // Act
                var configuration = _configurationTokeniser.Tokenise(configurationData);

                // Assert
                Assert.False(configuration.Any());
            }

            [Fact]
            public void IgnoresWhiteSpaceAndEmptyLines()
            {
                // Prepare
                var configurationData = new[]
                {
                    " ",
                    "TRANSFORM Web.config Web.Debug.config",
                    "DEPLOY C:\\Temp"
                };

                // Act
                var configuration = _configurationTokeniser.Tokenise(configurationData);

                // Assert
                Assert.True(configuration.Count == 1);
                Assert.True(configuration.First().Value.Count == 2);
            }


            [Fact]
            public void ReadsMultipleTargetsFromConfiguration()
            {
                // Prepare
                var configurationData = new[]
                {
                    "TRANSFORM Web.config Web.Debug.config",
                    "DEPLOY C:\\Temp\\Debug",
                    "TRANSFORM Web.config Web.Release.config",
                    "DEPLOY C:\\Temp\\Release"
                };

                // Act
                var configuration = _configurationTokeniser.Tokenise(configurationData);

                // Assert
                Assert.True(configuration.Count == 2);
                Assert.NotEqual(configuration.First(), configuration.Last());
                Assert.True(configuration.First().Value.Count == 2);
                Assert.True(configuration.Last().Value.Count == 2);
            }
        }
    }
}
