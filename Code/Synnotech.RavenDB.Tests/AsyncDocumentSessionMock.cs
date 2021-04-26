using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;

namespace Synnotech.RavenDB.Tests
{
    public sealed class AsyncDocumentSessionMock : IAsyncDocumentSession
    {
        private int DisposeCallCount { get; set; }

        public void Dispose() => DisposeCallCount++;

        public void MustHaveBeenDisposed() =>
            DisposeCallCount.Should().BeGreaterOrEqualTo(1);

        public IAsyncSessionDocumentCounters CountersFor(string documentId)
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentCounters CountersFor(object entity)
        {
            throw new NotSupportedException();
        }

        public void Delete<T>(T entity)
        {
            throw new NotSupportedException();
        }

        public void Delete(string id)
        {
            throw new NotSupportedException();
        }

        public void Delete(string id, string expectedChangeVector)
        {
            throw new NotSupportedException();
        }

        public Task SaveChangesAsync(CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task StoreAsync(object entity, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task StoreAsync(object entity, string changeVector, string id, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task StoreAsync(object entity, string id, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public IAsyncLoaderWithInclude<object> Include(string path)
        {
            throw new NotSupportedException();
        }

        public IAsyncLoaderWithInclude<T> Include<T>(Expression<Func<T, string>> path)
        {
            throw new NotSupportedException();
        }

        public IAsyncLoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, string>> path)
        {
            throw new NotSupportedException();
        }

        public IAsyncLoaderWithInclude<T> Include<T>(Expression<Func<T, IEnumerable<string>>> path)
        {
            throw new NotSupportedException();
        }

        public IAsyncLoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, IEnumerable<string>>> path)
        {
            throw new NotSupportedException();
        }

        public Task<T> LoadAsync<T>(string id, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task<T> LoadAsync<T>(string id, Action<IIncludeBuilder<T>> includes, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids, Action<IIncludeBuilder<T>> includes, CancellationToken token = new CancellationToken())
        {
            throw new NotSupportedException();
        }

        public IRavenQueryable<T> Query<T>(string indexName = null, string collectionName = null, bool isMapReduce = false)
        {
            throw new NotSupportedException();
        }

        public IRavenQueryable<T> Query<T, TIndexCreator>() where TIndexCreator : AbstractCommonApiForIndexes, new()
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentTimeSeries TimeSeriesFor(string documentId, string name)
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentTimeSeries TimeSeriesFor(object entity, string name)
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentTypedTimeSeries<TValues> TimeSeriesFor<TValues>(string documentId, string name = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentTypedTimeSeries<TValues> TimeSeriesFor<TValues>(object entity, string name = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentRollupTypedTimeSeries<TValues> TimeSeriesRollupFor<TValues>(object entity, string policy, string raw = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public IAsyncSessionDocumentRollupTypedTimeSeries<TValues> TimeSeriesRollupFor<TValues>(string documentId, string policy, string raw = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public IAsyncAdvancedSessionOperations Advanced => throw new NotSupportedException();
    }
}