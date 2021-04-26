using System.Threading.Tasks;
using FluentAssertions;
using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public sealed class AsyncSessionTests
    {
        private AsyncDocumentSessionMock DocumentSession { get; } = new ();

        [Fact]
        public static void MustDeriveFromAsyncReadOnlySession() =>
            typeof(AsyncSession).Should().BeDerivedFrom<AsyncReadOnlySession>();

        [Fact]
        public static void MustImplementIAsyncSession() =>
            typeof(AsyncSession).Should().Implement<IAsyncSession>();

        [Fact]
        public async Task MustForwardSaveChanges()
        {
            var session = new SessionMock(DocumentSession, false);

            await session.SaveChangesAsync();

            DocumentSession.SaveChangesMustHaveBeenCalled();
        }

        [Fact]
        public void MustCallWaitForIndexes()
        {
            var _ = new SessionMock(DocumentSession, true);

            DocumentSession.WaitForIndexesMustHaveBeenCalled();
        }

        private sealed class SessionMock : AsyncSession
        {
            public SessionMock(IAsyncDocumentSession session, bool waitForIndexesAfterSaveChanges) : base(session, waitForIndexesAfterSaveChanges) { }
        }
    }
}