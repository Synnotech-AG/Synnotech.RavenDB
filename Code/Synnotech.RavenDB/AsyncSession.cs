using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Represents an asynchronous session to a RavenDB database.
    /// Beware: you must not derive from this class and introduce other
    /// references to disposable objects. Only Session will be disposed.
    /// </summary>
    public abstract class AsyncSession : AsyncReadOnlySession, IAsyncSession
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AsyncSession"/>.
        /// </summary>
        /// <param name="session">The RavenDB document session that will be used to query the database.</param>
        /// <param name="waitForIndexesAfterSaveChanges">
        /// The value indicating whether this session will wait before all indexes are updated during a call to <see cref="SaveChangesAsync"/>.
        /// You need to set this value to true when you want to query an index after <see cref="SaveChangesAsync"/> has been
        /// called, otherwise the new information might not be part of the index.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="session"/> is null.</exception>
        protected AsyncSession(IAsyncDocumentSession session,
                               bool waitForIndexesAfterSaveChanges = true) : base(session)
        {
            if (waitForIndexesAfterSaveChanges)
                session.Advanced.WaitForIndexesAfterSaveChanges();
        }

        /// <summary>
        /// Saves all pending changes to the server.
        /// </summary>
        /// <param name="token">The token to cancel this asynchronous operation (optional).</param>
        public Task SaveChangesAsync(CancellationToken token = default) => Session.SaveChangesAsync(token);
    }
}