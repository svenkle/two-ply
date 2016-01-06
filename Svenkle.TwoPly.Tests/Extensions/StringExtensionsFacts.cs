using Svenkle.TwoPly.Extensions;
using Xunit;

namespace Svenkle.TwoPly.Tests.Extensions
{
    public class StringExtensionsFacts
    {
        public class TheToHashMethod
        {
            [Fact]
            public void ReturnsTheSha1HashOfAString()
            {
                // Prepare
                const string stringValue = "213";
                const string expected = "19187DC98DCE52FA4C4E8E05B341A9B77A51FD26";

                // Act & Assert
                Assert.Equal(stringValue.ToHash(), expected);
            }
        }

        public class TheRemoveWhiteSpaceMethod
        {
            [Fact]
            public void ReturnsStringWithoutWhiteSpace()
            {
                // Prepare
                const string stringValue = "A B C  D E     F    ";
                const string expected = "ABCDEF";

                // Act & Assert
                Assert.Equal(stringValue.RemoveWhitespace(), expected);
            }
        }
    }
}
