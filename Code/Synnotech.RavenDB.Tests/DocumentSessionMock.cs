using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

// ReSharper disable InconsistentNaming
#nullable disable
#pragma warning disable CS0067

namespace Synnotech.RavenDB.Tests
{
    public sealed class DocumentSessionMock : IDocumentSession
    {
        private AdvancedOperationsMock AdvancedOperations { get; } = new ();

        private int DisposeCallCount { get; set; }

        private int SaveChangesCallCount { get; set; }

        public void Dispose() => DisposeCallCount++;

        public void MustHaveBeenDisposed() =>
            DisposeCallCount.Should().BeGreaterOrEqualTo(1);

        public void WaitForIndexesMustHaveBeenCalled() => AdvancedOperations.WaitForIndexesMustHaveBeenCalled();

        public ISessionDocumentCounters CountersFor(string documentId)
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentCounters CountersFor(object entity)
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

        public void SaveChanges() => SaveChangesCallCount++;

        public void SaveChangesMustHaveBeenCalled() => SaveChangesCallCount.Should().Be(1);

        public void Store(object entity, string changeVector, string id)
        {
            throw new NotSupportedException();
        }

        public void Store(object entity)
        {
            throw new NotSupportedException();
        }

        public void Store(object entity, string id)
        {
            throw new NotSupportedException();
        }

        public ILoaderWithInclude<object> Include(string path)
        {
            throw new NotSupportedException();
        }

        public ILoaderWithInclude<T> Include<T>(Expression<Func<T, string>> path)
        {
            throw new NotSupportedException();
        }

        public ILoaderWithInclude<T> Include<T>(Expression<Func<T, IEnumerable<string>>> path)
        {
            throw new NotSupportedException();
        }

        public ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, string>> path)
        {
            throw new NotSupportedException();
        }

        public ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, IEnumerable<string>>> path)
        {
            throw new NotSupportedException();
        }

        public T Load<T>(string id)
        {
            throw new NotSupportedException();
        }

        public Dictionary<string, T> Load<T>(IEnumerable<string> ids)
        {
            throw new NotSupportedException();
        }

        public T Load<T>(string id, Action<IIncludeBuilder<T>> includes)
        {
            throw new NotSupportedException();
        }

        public Dictionary<string, T> Load<T>(IEnumerable<string> ids, Action<IIncludeBuilder<T>> includes)
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

        public ISessionDocumentTimeSeries TimeSeriesFor(string documentId, string name)
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentTimeSeries TimeSeriesFor(object entity, string name)
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentTypedTimeSeries<TValues> TimeSeriesFor<TValues>(object entity, string name = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentTypedTimeSeries<TValues> TimeSeriesFor<TValues>(string documentId, string name = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentRollupTypedTimeSeries<TValues> TimeSeriesRollupFor<TValues>(object entity, string policy, string raw = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public ISessionDocumentRollupTypedTimeSeries<TValues> TimeSeriesRollupFor<TValues>(string documentId, string policy, string raw = null) where TValues : new()
        {
            throw new NotSupportedException();
        }

        public IAdvancedSessionOperations Advanced => AdvancedOperations;

        private sealed class AdvancedOperationsMock : IAdvancedSessionOperations
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

            public void WaitForIndexesAfterSaveChanges(TimeSpan? timeout = null, bool throwOnTimeout = true, string[] indexes = null) => WaitForIndexesCallCount++;

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
            public IDocumentQuery<T> DocumentQuery<T, TIndexCreator>() where TIndexCreator : AbstractCommonApiForIndexes, new()
            {
                throw new NotSupportedException();
            }

            public IDocumentQuery<T> DocumentQuery<T>(string indexName = null, string collectionName = null, bool isMapReduce = false)
            {
                throw new NotSupportedException();
            }

            public void Refresh<T>(T entity)
            {
                throw new NotSupportedException();
            }

            public IRawDocumentQuery<T> RawQuery<T>(string query)
            {
                throw new NotSupportedException();
            }

            public IGraphQuery<T> GraphQuery<T>(string query)
            {
                throw new NotSupportedException();
            }

            public bool Exists(string id)
            {
                throw new NotSupportedException();
            }

            public T[] LoadStartingWith<T>(string idPrefix, string matches = null, int start = 0, int pageSize = 25, string exclude = null, string startAfter = null)
            {
                throw new NotSupportedException();
            }

            public void LoadStartingWithIntoStream(string idPrefix, Stream output, string matches = null, int start = 0, int pageSize = 25, string exclude = null, string startAfter = null)
            {
                throw new NotSupportedException();
            }

            public void LoadIntoStream(IEnumerable<string> ids, Stream output)
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

            public IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query, out StreamQueryStatistics streamQueryStats)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<StreamResult<T>> Stream<T>(string startsWith, string matches = null, int start = 0, int pageSize = 2147483647, string startAfter = null)
            {
                throw new NotSupportedException();
            }

            public void StreamInto<T>(IDocumentQuery<T> query, Stream output)
            {
                throw new NotSupportedException();
            }

            public void StreamInto<T>(IRawDocumentQuery<T> query, Stream output)
            {
                throw new NotSupportedException();
            }

            public IEagerSessionOperations Eagerly => throw new NotSupportedException();
            public ILazySessionOperations Lazily => throw new NotSupportedException();
            public IAttachmentsSessionOperations Attachments => throw new NotSupportedException();
            public IRevisionsSessionOperations Revisions => throw new NotSupportedException();
            public IClusterTransactionOperations ClusterTransaction => throw new NotSupportedException();
        }
    }
}