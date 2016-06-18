using System.Linq;
using Svenkle.TwoPly.Tokenisers;
using Xunit;

namespace Svenkle.TwoPly.Tests.Tokenisers
{
    public class CopyTokeniserFacts
    {
        private readonly CopyTokeniser _copyTokeniser;

        public CopyTokeniserFacts()
        {
            _copyTokeniser = new CopyTokeniser();
        }

        public class TheCreateMethod : CopyTokeniserFacts
        {
            [Fact]
            public void ReturnsTokensWhenMultipleFilesAreSpecified()
            {
                // Prepare & Act
                var commands = _copyTokeniser.Tokenise("Copy Web.config Web.Secondary.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }

            [Fact]
            public void ReturnsTokensWhenMultipleFilesWithWildcardsAreSpecified()
            {
                // Prepare & Act
                var commands = _copyTokeniser.Tokenise("Copy Web.config Web.*.config");

                // Assert
                Assert.True(commands.Count() == 3);
            }
        }

        public class TheCanCreateMethod : CopyTokeniserFacts
        {
            [Fact]
            public void ReturnsTrueWhenStringStartsWithValidCommand()
            {
                // Prepare & Act
                var result = _copyTokeniser.CanTokenise("Copy");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsNull()
            {
                // Prepare & Act
                var result = _copyTokeniser.CanTokenise(null);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsEmpty()
            {
                // Prepare & Act
                var result = _copyTokeniser.CanTokenise(string.Empty);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrueRegardlessOfCommandCase()
            {
                // Prepare & Act
                var result = _copyTokeniser.CanTokenise("COpy");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalseIfCommandIsAComment()
            {
                // Prepare & Act
                var result = _copyTokeniser.CanTokenise("#Copy");

                // Assert
                Assert.False(result);
            }
        }
    }
}
