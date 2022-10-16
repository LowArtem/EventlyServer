using EventlyServer.Data.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data.Repositories.Abstracts;

/// <summary>
/// Класс для управления моделями данных
/// </summary>
/// <typeparam name="T">класс модели данных</typeparam>
public class DbRepository<T> : IRepository<T> where T : Entity, new()
{
    /// <summary>Автоматически сохранять изменения в базе данных</summary>
    public bool AutoSaveChanges { get; set; } = true;


    private readonly DbContext _context;
    private readonly DbSet<T> _Set;


    public DbRepository(DbContext context)
    {
        _context = context;
        _Set = context.Set<T>();
    }

    public DbRepository(InHolidayContext context)
    {
        _context = context;
        _Set = context.Set<T>();
    }


    /// <summary>Коллекция элементов</summary>
    public virtual IQueryable<T> Items => _Set;


    /// <summary>
    /// Добавляем данные
    /// </summary>
    /// <param name="item"></param>
    /// <returns>добавленный объект</returns>
    public T Add(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _context.Entry(item).State = EntityState.Added;
        if (AutoSaveChanges)
            _context.SaveChanges();

        return item;
    }

    /// <summary>
    /// Добавляем данные асинхронно
    /// </summary>
    /// <param name="item"></param>
    /// <param name="cancel">токен отмены</param>
    /// <returns>добавленный объект</returns>
    public async Task<T> AddAsync(T item, CancellationToken cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _context.Entry(item).State = EntityState.Added;
        if (AutoSaveChanges)
            await _context.SaveChangesAsync(cancel).ConfigureAwait(false);

        return item;
    }

    /// <summary>
    /// Получаем данные
    /// </summary>
    /// <param name="id"></param>
    /// <returns>полученный объект (или null)</returns>
    public T? Get(long id)
    {
        return Items.SingleOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Получение полного списка данных
    /// </summary>
    /// <returns>полный список данных</returns>
    public List<T> GetAll()
    {
        return Items.ToList();
    }

    /// <summary>
    /// Получаем данные асинхронно
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancel">токен отмены</param>
    /// <returns>полученный объект (или null)</returns>
    public async Task<T?> GetAsync(long id, CancellationToken cancel = default)
    {
        return await Items.SingleOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false);
    }

    /// <summary>
    /// Получение полного списка данных асинхронно
    /// </summary>
    /// <returns>полный список данных</returns>
    public async Task<List<T>> GetAllAsync()
    {
        return await Items.ToListAsync();
    }

    /// <summary>
    /// Удаляем данные
    /// </summary>
    /// <param name="id"></param>
    /// <returns>удаленный объект</returns>
    public T? Remove(long id)
    {
        var item = Get(id);
        if (item is null) return null;

        _context.Remove(item);

        if (AutoSaveChanges)
            _context.SaveChanges();

        return item;
    }

    /// <summary>
    /// Удаляем данные асинхронно
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancel">токен отмены</param>
    /// <returns>удаленный объект</returns>
    public async Task<T?> RemoveAsync(long id, CancellationToken cancel = default)
    {
        var item = await GetAsync(id);
        if (item is null) return null;

        _context.Remove(item);

        if (AutoSaveChanges)
            await _context.SaveChangesAsync(cancel).ConfigureAwait(false);

        return item;
    }

    /// <summary>
    /// Обновляем данные
    /// </summary>
    /// <param name="item"></param>
    public void Update(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _context.Entry(item).State = EntityState.Modified;
        if (AutoSaveChanges)
            _context.SaveChanges();
    }

    /// <summary>
    /// Обновляем данные асинхронно
    /// </summary>
    /// <param name="item"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    public async Task UpdateAsync(T item, CancellationToken cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        _context.Entry(item).State = EntityState.Modified;
        if (AutoSaveChanges)
            await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Сохранить изменения
    /// </summary>
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    /// <summary>
    /// Сохранить изменения асинхронно
    /// </summary>
    /// <returns></returns>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}