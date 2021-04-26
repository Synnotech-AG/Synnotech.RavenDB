using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;
using Raven.Client.Documents.Session.Operations.Lazy;
using Raven.Client.Http;
using Raven.Client.Json.Serialization;
using Sparrow.Json;

namespace Synnotech.RavenDB.Tests
{
    public sealed class AsyncDocumentSessionMock : IAsyncDocumentSession
    {
        private AdvancedOperationsMock AdvancedOperations { get; } = new ();

        private int DisposeCallCount { get; set; }

        private int SaveChangesCallCount { get; set; }

        public void Dispose() => DisposeCallCount++;

        public void MustHaveBeenDisposed() =>
            DisposeCallCount.Should().BeGreaterOrEqualTo(1);

        public void WaitForIndexesMustHaveBeenCalled() => AdvancedOperations.WaitForIndexesMustHaveBeenCalled();

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

        public Task SaveChangesAsync(CancellationToken token = default)
        {
            SaveChangesCallCount++;
            return Task.CompletedTask;
        }

        public void SaveChangesMustHaveBeenCalled() =>
            SaveChangesCallCount.Should().Be(1);

        public Task StoreAsync(object entity, CancellationToken token = new ())
        {
            throw new NotSupportedException();
        }

        public Task StoreAsync(object entity, string changeVector, string id, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }

        public Task StoreAsync(object entity, string id, CancellationToken token = default)
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

        public Task<T> LoadAsync<T>(string id, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }

        public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }

        public Task<T> LoadAsync<T>(string id, Action<IIncludeBuilder<T>> includes, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }

        public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids, Action<IIncludeBuilder<T>> includes, CancellationToken token = default)
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

        public IAsyncAdvancedSessionOperations Advanced => AdvancedOperations;

        private sealed class AdvancedOperationsMock : IAsyncAdvancedSessionOperations
        {
            private int WaitForIndexesCallCount { get; set; }

            public Task<ServerNode> GetCurrentSessionNode()
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public void Defer(ICommandData command, params ICommandData[] commands)
            {
                throw new NotSupportedException();
            }

            public void Defer(ICommandData[] commands)
            {
                throw new NotSupportedException();
            }

            public void Evict<T>(T entity)
            {
                throw new NotSupportedException();
            }

            public string GetDocumentId(object entity)
            {
                throw new NotSupportedException();
            }

            public IMetadataDictionary GetMetadataFor<T>(T instance)
            {
                throw new NotSupportedException();
            }

            public string GetChangeVectorFor<T>(T instance)
            {
                throw new NotSupportedException();
            }

            public List<string> GetCountersFor<T>(T instance)
            {
                throw new NotSupportedException();
            }

            public List<string> GetTimeSeriesFor<T>(T instance)
            {
                throw new NotSupportedException();
            }

            public DateTime? GetLastModifiedFor<T>(T instance)
            {
                throw new NotSupportedException();
            }

            public bool HasChanged(object entity)
            {
                throw new NotSupportedException();
            }

            public bool IsLoaded(string id)
            {
                throw new NotSupportedException();
            }

            public void IgnoreChangesFor(object entity)
            {
                throw new NotSupportedException();
            }

            public IDictionary<string, DocumentsChanges[]> WhatChanged()
            {
                throw new NotSupportedException();
            }

            public void WaitForReplicationAfterSaveChanges(TimeSpan? timeout = null, bool throwOnTimeout = true, int replicas = 1, bool majority = false)
            {
                throw new NotSupportedException();
            }

            public void WaitForIndexesAfterSaveChanges(TimeSpan? timeout = null, bool throwOnTimeout = true, string[] indexes = null)
            {
                WaitForIndexesCallCount++;
            }

            public void WaitForIndexesMustHaveBeenCalled() => WaitForIndexesCallCount.Should().Be(1);

            public void SetTransactionMode(TransactionMode mode)
            {
                throw new NotSupportedException();
            }

            public IDocumentStore DocumentStore => throw new NotSupportedException();
            public IDictionary<string, object> ExternalState => throw new NotSupportedException();
            public RequestExecutor RequestExecutor => throw new NotSupportedException();
            public JsonOperationContext Context => throw new NotSupportedException();
            public SessionInfo SessionInfo => throw new NotSupportedException();
            public bool HasChanges => throw new NotSupportedException();
            public int MaxNumberOfRequestsPerSession { get; set; }
            public int NumberOfRequests => throw new NotSupportedException();
            public string StoreIdentifier => throw new NotSupportedException();
            public bool UseOptimisticConcurrency { get; set; }
            public ISessionBlittableJsonConverter JsonConverter => throw new NotSupportedException();
            public event EventHandler<BeforeStoreEventArgs> OnBeforeStore;
            public event EventHandler<AfterSaveChangesEventArgs> OnAfterSaveChanges;
            public event EventHandler<BeforeDeleteEventArgs> OnBeforeDelete;
            public event EventHandler<BeforeQueryEventArgs> OnBeforeQuery;
            public IAsyncDocumentQuery<T> AsyncDocumentQuery<T, TIndexCreator>() where TIndexCreator : AbstractCommonApiForIndexes, new()
            {
                throw new NotSupportedException();
            }

            public IAsyncDocumentQuery<T> AsyncDocumentQuery<T>(string indexName = null, string collectionName = null, bool isMapReduce = false)
            {
                throw new NotSupportedException();
            }

            public Task RefreshAsync<T>(T entity, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public IAsyncRawDocumentQuery<T> AsyncRawQuery<T>(string query)
            {
                throw new NotSupportedException();
            }

            public IAsyncGraphQuery<T> AsyncGraphQuery<T>(string query)
            {
                throw new NotSupportedException();
            }

            public Task<bool> ExistsAsync(string id, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IEnumerable<T>> LoadStartingWithAsync<T>(string idPrefix, string matches = null, int start = 0, int pageSize = 25, string exclude = null, string startAfter = null, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task LoadStartingWithIntoStreamAsync(string idPrefix, Stream output, string matches = null, int start = 0, int pageSize = 25, string exclude = null, string startAfter = null, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task LoadIntoStreamAsync(IEnumerable<string> ids, Stream output, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public void Increment<T, U>(T entity, Expression<Func<T, U>> path, U valToAdd)
            {
                throw new NotSupportedException();
            }

            public void Increment<T, U>(string id, Expression<Func<T, U>> path, U valToAdd)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, U>(string id, Expression<Func<T, U>> path, U value)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, U>(T entity, Expression<Func<T, U>> path, U value)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, U>(T entity, Expression<Func<T, IEnumerable<U>>> path, Expression<Func<JavaScriptArray<U>, object>> arrayAdder)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, U>(string id, Expression<Func<T, IEnumerable<U>>> path, Expression<Func<JavaScriptArray<U>, object>> arrayAdder)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, TKey, TValue>(T entity, Expression<Func<T, IDictionary<TKey, TValue>>> path, Expression<Func<JavaScriptDictionary<TKey, TValue>, object>> dictionaryAdder)
            {
                throw new NotSupportedException();
            }

            public void Patch<T, TKey, TValue>(string id, Expression<Func<T, IDictionary<TKey, TValue>>> path, Expression<Func<JavaScriptDictionary<TKey, TValue>, object>> dictionaryAdder)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IAsyncDocumentQuery<T> query, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IAsyncDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IAsyncRawDocumentQuery<T> query, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IAsyncRawDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IQueryable<T> query, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IQueryable<T> query, out StreamQueryStatistics streamQueryStats, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(string startsWith, string matches = null, int start = 0, int pageSize = 2147483647, string startAfter = null, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task StreamIntoAsync<T>(IAsyncDocumentQuery<T> query, Stream output, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public Task StreamIntoAsync<T>(IAsyncRawDocumentQuery<T> query, Stream output, CancellationToken token = default)
            {
                throw new NotSupportedException();
            }

            public IAsyncEagerSessionOperations Eagerly => throw new NotSupportedException();
            public IAsyncLazySessionOperations Lazily => throw new NotSupportedException();
            public IAttachmentsSessionOperationsAsync Attachments => throw new NotSupportedException();
            public IRevisionsSessionOperationsAsync Revisions => throw new NotSupportedException();
            public IClusterTransactionOperationsAsync ClusterTransaction => throw new NotSupportedException();
        }
    }
}