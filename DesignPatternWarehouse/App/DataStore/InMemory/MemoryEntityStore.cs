using App.DataStore;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;

public class MemoryEntityStore<T> : IEntityStore<T>, IQueryable<T>, IAsyncEnumerable<T>, IListSource
{
    private static List<T> _data;
    private static IQueryable<T> _queryable;

    public MemoryEntityStore(IEnumerable<T>? source = null)
    {
        MemoryEntityStore<T>._data ??= source?.ToList() ?? new List<T>();
        MemoryEntityStore<T> ._queryable = _data.AsQueryable();
    }

    public Type ElementType => typeof(T);
    public Expression Expression => _queryable.Expression;
    public IQueryProvider Provider => _queryable.Provider;

    public bool ContainsListCollection => false;

    public IList GetList()
    {
        return _data.ToList();
    }

    public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new AsyncEnumeratorWrapper(_queryable.GetEnumerator());

    public void Add(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _data.Add(entity);
    }

    public void Delete(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _data.Remove(entity);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    private sealed class AsyncEnumeratorWrapper : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumeratorWrapper(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
    }
}
