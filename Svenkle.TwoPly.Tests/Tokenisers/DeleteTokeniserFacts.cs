using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class DeleteTokeniserFacts
    {
        private readonly DeleteTokeniser _deleteTokeniser;

        public DeleteTokeniserFacts()
        {
            _deleteTokeniser = new DeleteTokeniser();
        }

        public class TheCreateMethod : DeleteTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _deleteTokeniser.Tokenise("Delete Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }

            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecifiedWithAWildcard()
            {
                // Prepare & Act
                var commands = _deleteTokeniser.Tokenise("Delete Web.config Web.*.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : DeleteTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _deleteTokeniser.CanTokenise("Delete");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _deleteTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _deleteTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _deleteTokeniser.CanTokenise("DelETE");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _deleteTokeniser.CanTokenise("#Delete");

                // Assert
                Assert.False(result);
            }
        }
    }
}
