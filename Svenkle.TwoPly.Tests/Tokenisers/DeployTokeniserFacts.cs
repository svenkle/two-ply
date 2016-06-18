using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class DeployTokeniserFacts
    {
        private readonly DeployTokeniser _deployTokeniser;

        public DeployTokeniserFacts()
        {
            _deployTokeniser = new DeployTokeniser();
        }

        public class TheCreateMethod : DeployTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _deployTokeniser.Tokenise("Deploy Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }

            [Fact]
            public void ReturnsTokensWhenMultipleFilesWithWildcardsAreSpecified()
            {
                // Prepare & Act
                var commands = _deployTokeniser.Tokenise("Deploy Web.config Web.*.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : DeployTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _deployTokeniser.CanTokenise("Deploy");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _deployTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _deployTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _deployTokeniser.CanTokenise("Deploy");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _deployTokeniser.CanTokenise("#Deploy");

                // Assert
                Assert.False(result);
            }
        }
    }
}
