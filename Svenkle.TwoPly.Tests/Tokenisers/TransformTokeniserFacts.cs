using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class TransformTokeniserFacts
    {
        private readonly TransformTokeniser _transformTokeniser;

        public TransformTokeniserFacts()
        {
            _transformTokeniser = new TransformTokeniser();
        }

        public class TheCreateMethod : TransformTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _transformTokeniser.Tokenise("Transform Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }

            [Fact]
            public void ReturnsTokensWhenMultipleFilesWithWildcardsAreSpecified()
            {
                // Prepare & Act
                var commands = _transformTokeniser.Tokenise("Transform Web.config Web.*.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : TransformTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _transformTokeniser.CanTokenise("Transform");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _transformTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _transformTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _transformTokeniser.CanTokenise("TranSform");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _transformTokeniser.CanTokenise("#Transform");

                // Assert
                Assert.False(result);
            }
        }
    }
}
