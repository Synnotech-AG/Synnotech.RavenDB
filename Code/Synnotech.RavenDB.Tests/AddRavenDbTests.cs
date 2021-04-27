using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents.Session;
using Synnotech.Xunit;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public class AddRavenDbTests
    {
        [SkippableFact]
        public static async Task RegisterAndUseSession()
        {
            SkipIfNecessary();

            var container = new ServiceCollection().AddSingleton(TestSettings.Configuration)
                                                   .AddRavenDb()
                                                   .BuildServiceProvider();
            using var session1 = container.GetRequiredService<IAsyncDocumentSession>();
            var entity = new SimpleEntity();
            await session1.StoreAsync(entity);
            await session1.SaveChangesAsync();

            using var session2 = container.GetRequiredService<IAsyncDocumentSession>();
            session2.Delete(entity.Id);
            await session2.SaveChangesAsync();

            session1.Should().NotBeSameAs(session2);
        }

        private static void SkipIfNecessary() =>
            Skip.IfNot(TestSettings.Configuration.GetValue<bool>("runDatabaseIntegrationTests"));
    }

    public class SimpleEntity
    {
        public string? Id { get; set; }

        public int Value { get; set; } = 42;
    }
}