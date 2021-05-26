using System;
using System.Threading.Tasks;
using Light.GuardClauses;
using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Represents an asynchronous session to a RavenDB database. This session
    /// is only used to read data (i.e. no data is inserted or updated), thus
    /// SaveChangesAsync is not available.
    /// Beware: you must not derive from this class and introduce other
    /// references to disposable objects. Only <see cref="Session" />
    /// will be disposed.
    /// </summary>
    public abstract class AsyncReadOnlySession : IAsyncReadOnlySession
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AsyncReadOnlySession" />.
        /// </summary>
        /// <param name="session">The RavenDB document session that will be used to query the database</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="session" /> is null.</exception>
        protected AsyncReadOnlySession(IAsyncDocumentSession session) =>
            Session = session.MustNotBeNull(nameof(session));

        /// <summary>
        /// Gets the RavenDB document session to load data from the database.
        /// </summary>
        protected IAsyncDocumentSession Session { get; }

        /// <summary>
        /// Disposes the RavenDB document session.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            Session.Dispose();
            return default;
        }

        /// <summary>
        /// Disposes the RavenDB document session.
        /// </summary>
        public void Dispose() => Session.Dispose();
    }
}