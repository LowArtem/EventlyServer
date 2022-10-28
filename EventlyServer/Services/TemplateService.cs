using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с шаблонами сайтов-приглашений
/// </summary>
public class TemplateService
{
    private readonly IRepository<Template> _templateRepository;

    public TemplateService(IRepository<Template> templateRepository)
    {
        _templateRepository = templateRepository;
    }

    /// <summary>
    /// Добавить новый шаблон
    /// </summary>
    /// <param name="template">описание шаблона</param>
    /// <exception cref="ArgumentException">если шаблон с данным именем уже существует</exception>
    public async Task AddTemplate(TemplateDto template)
    {
        var templateTest = await _templateRepository.Items.SingleOrDefaultAsync(t => t.Name == template.Name);
        if (templateTest != null)
        {
            throw new ArgumentException("Template with this name already exists", nameof(template));
        }

        await _templateRepository.AddAsync(template.ToTemplate());
    }

    /// <summary>
    /// Обновить шаблон
    /// </summary>
    /// <param name="updated">описание шаблона (обновляются только переданные методы)</param>
    /// <exception cref="InvalidDataException">если шаблона с переданным id не существует</exception>
    public async Task UpdateTemplate(TemplateUpdateDto updated)
    {
        var templateOld = await _templateRepository.GetAsync(updated.Id);
        if (templateOld == null)
        {
            throw new InvalidDataException("Template with given id cannot be found");
        }

        Template templateNew = new Template(
            id: updated.Id,
            price: updated.Price ?? templateOld.Price,
            name: updated.Name ?? templateOld.Name,
            idTypeOfEvent: updated.Event?.Id ?? templateOld.IdTypeOfEvent,
            filePath: updated.FilePath ?? templateOld.FilePath,
            previewPath: updated.PreviewPath ?? templateOld.PreviewPath
        );

        await _templateRepository.UpdateAsync(templateNew);
    }

    /// <summary>
    /// Удалить шаблон
    /// </summary>
    /// <param name="id">id удаляемого шаблона</param>
    /// <exception cref="InvalidDataException">если шаблона с переданным id не существует</exception>
    public async Task DeleteTemplate(int id)
    {
        var template = await _templateRepository.GetAsync(id);
        if (template == null)
        {
            throw new InvalidDataException("Template with given id cannot be found");
        }
        
        await _templateRepository.RemoveAsync(id);
    }

    /// <summary>
    /// Получить информацию обо всех шаблонах
    /// </summary>
    /// <remarks>В частности выдает пути к файлам шаблона и превью, которые фронтенд может у себя отобразить</remarks>
    /// <returns>список информации обо всех существующих шаблонах</returns>
    public async Task<List<TemplateDto>> GetTemplates()
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.ConvertAll(t => t.ToDto());
    }
}