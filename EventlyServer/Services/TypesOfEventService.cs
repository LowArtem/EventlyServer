using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с типами мероприятий
/// </summary>
public class TypesOfEventService
{
    private readonly IRepository<TypesOfEvent> _typesOfEventRepository;

    public TypesOfEventService(IRepository<TypesOfEvent> typesOfEventRepository)
    {
        _typesOfEventRepository = typesOfEventRepository;
    }

    /// <summary>
    /// Получить список всех типов мероприятий
    /// </summary>
    /// <returns>список всех типов мероприятий</returns>
    public async Task<List<TypesOfEventDto>> GetAllTypes()
    {
        var types = await _typesOfEventRepository.GetAllAsync();
        return types.ConvertAll(t => t.ToDto());
    }

    /// <summary>
    /// Добавить новый тип мероприятия
    /// </summary>
    /// <param name="name">название нового типа мероприятия</param>
    /// <exception cref="ArgumentException">если мероприятие с таким названием уже существует</exception>
    public async Task AddNewType(string name)
    {
        var testTypes = _typesOfEventRepository.Items.FirstOrDefaultAsync(t => t.Name == name);
        if (testTypes != null)
        {
            throw new ArgumentException("Type of event with this name already exists", nameof(name));
        }
        
        await _typesOfEventRepository.AddAsync(new TypesOfEvent(name));
    }

    /// <summary>
    /// Изменить выбранный тип меропрития
    /// </summary>
    /// <param name="id">id изменяемого типа</param>
    /// <param name="newName">новое название</param>
    /// <exception cref="InvalidDataException">если типа меропрития с таким id не существует</exception>
    /// <exception cref="ArgumentException">если мероприятие с таким названием уже существует</exception>
    public async Task UpdateType(int id, string newName)
    {
        var type = await _typesOfEventRepository.GetAsync(id);
        if (type == null)
        {
            throw new InvalidDataException("Type of event with given id cannot be found");
        }
        
        var testTypes = _typesOfEventRepository.Items.FirstOrDefaultAsync(t => t.Name == newName);
        if (testTypes != null)
        {
            throw new ArgumentException("Type of event with this name already exists", nameof(newName));
        }
        type.Name = newName;
        
        await _typesOfEventRepository.UpdateAsync(type);
    }

    /// <summary>
    /// Удалить выбранный тип мероприятия
    /// </summary>
    /// <param name="id">id удаляемого типа</param>
    /// <exception cref="InvalidDataException">если типа меропрития с таким id не существует</exception>
    public async Task DeleteType(int id)
    {
        var type = await _typesOfEventRepository.GetAsync(id);
        if (type == null)
        {
            throw new InvalidDataException("Type of event with given id cannot be found");
        }

        await _typesOfEventRepository.RemoveAsync(id);
    }
}