using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class TouchTokeniserFacts
    {
        private readonly TouchTokeniser _touchTokeniser;

        public TouchTokeniserFacts()
        {
            _touchTokeniser = new TouchTokeniser();
        }

        public class TheCreateMethod : TouchTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _touchTokeniser.Tokenise("Touch Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : TouchTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _touchTokeniser.CanTokenise("Touch");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _touchTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _touchTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _touchTokeniser.CanTokenise("Touch");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _touchTokeniser.CanTokenise("#Touch");

                // Assert
                Assert.False(result);
            }
        }
    }
}
