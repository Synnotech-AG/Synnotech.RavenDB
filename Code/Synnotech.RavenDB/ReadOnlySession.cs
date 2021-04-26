using System;
using Light.GuardClauses;
using Raven.Client.Documents.Session;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Represents a synchronous session to a RavenDB database. This session
    /// is only used to read data (i.e. no data is inserted or updated), thus
    /// SaveChanges is not available.
    /// Beware: you must not derive from this class and introduce other
    /// references to disposable objects. Only <see cref="Session"/>
    /// will be disposed.
    /// Please remember that database access should be asynchronous by default,
    /// so consider using <see cref="AsyncReadOnlySession"/> instead.
    /// </summary>
    public abstract class ReadOnlySession : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlySession"/>.
        /// </summary>
        /// <param name="session">The RavenDB document session that will be used to query the database</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="session"/> is null.</exception>
        protected ReadOnlySession(IDocumentSession session)
        {
            Session = session.MustNotBeNull(nameof(session));
        }

        /// <summary>
        /// Gets the RavenDB document session to load data from the database.
        /// </summary>
        protected IDocumentSession Session { get; }

        /// <summary>
        /// Disposes the RavenDB document session.
        /// </summary>
        public void Dispose() => Session.Dispose();
    }
}