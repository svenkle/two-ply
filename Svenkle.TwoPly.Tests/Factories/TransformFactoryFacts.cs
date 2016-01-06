using System;
using Svenkle.TwoPly.Factories;
using Svenkle.TwoPly.Factories.Interfaces;
using Xunit;

namespace Svenkle.TwoPly.Tests.Factories
{
    public class TransformFactoryFacts
    {
        private readonly ITransformFactory _transformFactory;

        public TransformFactoryFacts()
        {
            _transformFactory = new TransformFactory();
        }

        public class TheCreateMethod : TransformFactoryFacts
        {
            [Fact]
            public void ThrowsAnArgumentExceptionWhenTheConfigurationTranformSyntaxIsChained()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _transformFactory.Create("Web.config => XmlTransform.config => Secondary.config"));
            }

            [Fact]
            public void ThrowsAnArgumentExceptionWhenTheConfigurationTranformSyntaxIsInvalid()
            {
                // Prepare & Act & Assert
                Assert.Throws<ArgumentException>(() => _transformFactory.Create("Web.config => "));
            }
        }
    }
}
