using System.Threading.Tasks;
using FluentAssertions;
using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;
using Xunit;

namespace Synnotech.RavenDB.Tests
{
    public sealed class AsyncReadOnlySessionTests
    {
        public AsyncReadOnlySessionTests()
        {
            DocumentSession = new AsyncDocumentSessionMock();
            Session = new SessionMock(DocumentSession);
        }

        private SessionMock Session { get; }

        private AsyncDocumentSessionMock DocumentSession { get; }

        [Fact]
        public static void MustImplementIDisposable() =>
            typeof(AsyncReadOnlySession).Should().Implement<IAsyncReadOnlySession>();

        [Fact]
        public void SessionMustBeRetrievable() =>
            Session.VerifyDocumentSessionAccess(DocumentSession);

        [Fact]
        public void MustDisposeDocumentSessionOnDispose()
        {
            Session.Dispose();
            DocumentSession.MustHaveBeenDisposed();
        }

        [Fact]
        public async Task MustDisposeDocumentSessionOnDisposeAsync()
        {
            await Session.DisposeAsync();
            DocumentSession.MustHaveBeenDisposed();
        }

        private sealed class SessionMock : AsyncReadOnlySession
        {
            public SessionMock(IAsyncDocumentSession session) : base(session) { }

            public void VerifyDocumentSessionAccess(AsyncDocumentSessionMock expectedDocumentSession) =>
                Session.Should().BeSameAs(expectedDocumentSession);
        }
    }
}