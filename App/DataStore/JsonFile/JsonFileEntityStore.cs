using System.Collections;
using System.Linq.Expressions;
using System.Text.Json;

namespace App.DataStore.JsonFile;
public class JsonFileEntityStore<T> : IEntityStore<T>
{
    private readonly string _filePath;
    private readonly List<T> _data;
    private readonly IQueryable<T> _queryable;


    public JsonFileEntityStore(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        _data = LoadFromFile();
        _queryable = _data.AsQueryable();
    }

    public Type ElementType => typeof(T);
    public Expression Expression => _queryable.Expression;
    public IQueryProvider Provider => _queryable.Provider;

    public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

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

    public async Task SaveChangesAsync()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, _data, options);
    }


    private List<T> LoadFromFile()
    {
        if (!File.Exists(_filePath))
            return new List<T>();
        try
        {
            var json = File.ReadAllText(_filePath);
            var list = JsonSerializer.Deserialize<List<T>>(json);
            return list ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    private sealed class AsyncEnumeratorWrapper : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public AsyncEnumeratorWrapper(IEnumerator<T> inner) => _inner = inner;
        public T Current => _inner.Current;
        public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
        public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
    }
}
