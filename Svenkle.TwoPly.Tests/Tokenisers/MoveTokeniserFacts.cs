using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class MoveTokeniserFacts
    {
        private readonly MoveTokeniser _moveTokeniser;

        public MoveTokeniserFacts()
        {
            _moveTokeniser = new MoveTokeniser();
        }

        public class TheCreateMethod : MoveTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _moveTokeniser.Tokenise("Move Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }

            [Fact]
            public void ReturnsTokensWhenMultipleFilesWithWildcardsAreSpecified()
            {
                // Prepare & Act
                var commands = _moveTokeniser.Tokenise("Move Web.config Web.*.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : MoveTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _moveTokeniser.CanTokenise("Move");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _moveTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _moveTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _moveTokeniser.CanTokenise("Move");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _moveTokeniser.CanTokenise("#Move");

                // Assert
                Assert.False(result);
            }
        }
    }
}
