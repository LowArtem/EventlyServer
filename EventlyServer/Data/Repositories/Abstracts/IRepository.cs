using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Repositories.Abstracts;

public interface IRepository<T> where T : Entity, new()
{
    bool AutoSaveChanges { get; set; }

    IQueryable<T> Items { get; }

    T? Get(long id);
    T? GetUntracked(long id);
    List<T> GetAll();
    List<T> GetAllUntracked();
    Task<T?> GetAsync(long id, CancellationToken cancel = default);
    Task<T?> GetUntrackedAsync(long id, CancellationToken cancel = default);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllUntrackedAsync();


    T Add(T item);
    Task<T> AddAsync(T item, CancellationToken cancel = default);

    void Update(T item);
    Task UpdateAsync(T item, CancellationToken cancel = default);

    T? Remove(long id);
    Task<T?> RemoveAsync(long id, CancellationToken cancel = default);

    void SaveChanges();

    Task SaveChangesAsync();
}