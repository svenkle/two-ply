using System;
using Svenkle.TwoPly.Factories;
using Svenkle.TwoPly.Factories.Interfaces;
using Xunit;

namespace Svenkle.TwoPly.Tests.Factories
{
    public class TargetPathFactoryFacts
    {
        private readonly ITargetPathFactory _targetPathFactory;

        public TargetPathFactoryFacts()
        {
            _targetPathFactory = new TargetPathFactory();
        }

        public class TheCreateMethod : TargetPathFactoryFacts
        {
            [Fact]
            public void ThrowsAnArgumentExceptionWhenThePathIsEmpty()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _targetPathFactory.Create("Path ="));
            }

            [Fact]
            public void ThrowsAnArgumentExceptionWhenThePathSyntaxIsMissingAnEquals()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _targetPathFactory.Create("Path C:\\Temp"));
            }

            [Fact]
            public void CorrectlyParsesThePathValueWhenThePathKeyContainsLowercaseCharacters()
            {
                // Prepare
                const string path = "C:\\Temp";

                // Act
                var targetPath = _targetPathFactory.Create("path = " + path);

                // Assert
                Assert.NotNull(targetPath);
                Assert.Equal(targetPath.Path, path);
            }

            [Fact]
            public void CorrectlyParsesThePathValueWhenItMatchesThePathConfigurationSyntax()
            {
                // Prepare
                const string path = "C:\\Path=Temp";

                // Act
                var targetPath = _targetPathFactory.Create("path = " + path);

                // Assert
                Assert.NotNull(targetPath);
                Assert.Equal(targetPath.Path, path);
            }
            
            [Fact]
            public void CorrectlyParsesThePathValueWhenThePathKeyContainsWhiteSpace()
            {
                // Prepare
                const string path = "C:\\Temp";

                // Act
                var targetPath = _targetPathFactory.Create("Path = " + path);

                // Assert
                Assert.NotNull(targetPath);
                Assert.Equal(targetPath.Path, path);
            }
        }
    }
}
