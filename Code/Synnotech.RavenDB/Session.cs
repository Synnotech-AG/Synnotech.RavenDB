using Raven.Client.Documents.Session;
using Synnotech.DatabaseAbstractions;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Represents a synchronous session to a RavenDB database.
    /// Beware: you must not derive from this class and introduce other
    /// references to disposable objects. Only Session will be disposed.
    /// Please remember that database access should be asynchronous by default,
    /// so consider using <see cref="AsyncSession"/> instead.
    /// </summary>
    public abstract class Session : ReadOnlySession, ISession
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Session"/>.
        /// </summary>
        /// <param name="session">The RavenDB document session that will be used to query the database.</param>
        /// <param name="waitForIndexesAfterSaveChanges">
        /// The value indicating whether this session will wait before all indexes are updated during a call to <see cref="SaveChanges"/>.
        /// You need to set this value to true when you want to query an index after <see cref="SaveChanges"/> has been
        /// called, otherwise the new information might not be part of the index.
        /// </param>
        protected Session(IDocumentSession session,
                          bool waitForIndexesAfterSaveChanges = true) : base(session)
        {
            if (waitForIndexesAfterSaveChanges)
                session.Advanced.WaitForIndexesAfterSaveChanges();
        }

        /// <summary>
        /// Saves all pending changes to the server.
        /// </summary>
        public void SaveChanges() => Session.SaveChanges();
    }
}