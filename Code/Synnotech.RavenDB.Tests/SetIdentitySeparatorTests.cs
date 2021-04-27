using System;
using FluentAssertions;
using Raven.Client.Documents.Conventions;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public static class SetIdentitySeparatorTests
    {
        [Fact]
        public static void DefaultSeparator()
        {
            var documentConventions = new DocumentConventions().SetIdentityPartsSeparator();
            documentConventions.IdentityPartsSeparator.Should().Be('-');
        }

        [Theory]
        [InlineData('/')]
        [InlineData('_')]
        public static void SetCustomSeparator(char separator)
        {
            var documentConventions = new DocumentConventions().SetIdentityPartsSeparator(separator);

            documentConventions.IdentityPartsSeparator.Should().Be(separator);
        }

        [Fact]
        public static void DocumentConventionsNull()
        {
            Action act = () => ((DocumentConventions) null!).SetIdentityPartsSeparator();

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("documentConventions");
        }
    }
}