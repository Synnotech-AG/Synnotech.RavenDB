using FluentAssertions;
using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public sealed class SessionTests
    {
        private DocumentSessionMock DocumentSession { get; } = new ();

        [Fact]
        public static void MustDeriveFromReadOnlySession() =>
            typeof(Session).Should().BeDerivedFrom<ReadOnlySession>();

        [Fact]
        public static void MustImplementIAsyncSession() =>
            typeof(Session).Should().Implement<ISession>();

        [Fact]
        public void MustForwardSaveChanges()
        {
            var session = new SessionMock(DocumentSession, false);

            session.SaveChanges();

            DocumentSession.SaveChangesMustHaveBeenCalled();
        }

        [Fact]
        public void MustCallWaitForIndexes()
        {
            var _ = new SessionMock(DocumentSession, true);

            DocumentSession.WaitForIndexesMustHaveBeenCalled();
        }

        private sealed class SessionMock : Session
        {
            public SessionMock(IDocumentSession session, bool waitForIndexesAfterSaveChanges) : base(session, waitForIndexesAfterSaveChanges) { }
        }
    }
}