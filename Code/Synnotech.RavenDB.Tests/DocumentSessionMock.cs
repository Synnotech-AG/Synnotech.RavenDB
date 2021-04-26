using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;

namespace Synnotech.RavenDB.Tests
{
    public sealed class DocumentSessionMock : IDocumentSession
    {
        private int DisposeCallCount { get; set; }

        public void Dispose() => DisposeCallCount++;

        public void MustHaveBeenDisposed() =>
            DisposeCallCount.Should().BeGreaterOrEqualTo(1);

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

        public void SaveChanges()
        {
            throw new NotSupportedException();
        }

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

        public IAdvancedSessionOperations Advanced => throw new NotSupportedException();
    }
}