using System;
using FluentAssertions;
using Raven.Client.Documents.Session;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public sealed class ReadOnlySessionTests
    {
        public ReadOnlySessionTests()
        {
            DocumentSession = new DocumentSessionMock();
            Session = new SessionMock(DocumentSession);
        }

        private SessionMock Session { get; }

        private DocumentSessionMock DocumentSession { get; }

        [Fact]
        public static void MustImplementIDisposable() =>
            typeof(ReadOnlySession).Should().Implement<IDisposable>();

        [Fact]
        public void SessionMustBeRetrievable() =>
            Session.VerifyDocumentSessionAccess(DocumentSession);

        [Fact]
        public void MustDisposeDocumentSessionOnDispose()
        {
            Session.Dispose();
            DocumentSession.MustHaveBeenDisposed();
        }

        private sealed class SessionMock : ReadOnlySession
        {
            public SessionMock(IDocumentSession session) : base(session) { }

            public void VerifyDocumentSessionAccess(DocumentSessionMock expectedDocumentSession) =>
                Session.Should().BeSameAs(expectedDocumentSession);
        }
    }
}