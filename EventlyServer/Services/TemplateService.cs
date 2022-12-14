using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Extensions;
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
    /// <exception cref="EntityExistsException">если шаблон с данным именем уже существует</exception>
    public async Task<Result> AddTemplate(TemplateCreatingDto template)
    {
        var templateTest = await _templateRepository.Items.SingleOrDefaultAsync(t => t.Name == template.Name);
        if (templateTest != null)
        {
            return new EntityExistsException(nameof(template), template.Name);
        }

        await _templateRepository.AddAsync(template.ToTemplate());
        return Result.Success();
    }

    /// <summary>
    /// Обновить шаблон
    /// </summary>
    /// <param name="updated">описание шаблона (обновляются только переданные методы)</param>
    /// <exception cref="EntityNotFoundException">если шаблона с переданным id не существует</exception>
    public async Task<Result> UpdateTemplate(TemplateUpdateDto updated)
    {
        var templateOld = await _templateRepository.GetUntrackedAsync(updated.Id);
        if (templateOld == null)
        {
            return new EntityNotFoundException(nameof(templateOld), updated.Id);
        }

        Template templateNew = new Template(
            id: updated.Id,
            price: updated.Price ?? templateOld.Price,
            name: updated.Name ?? templateOld.Name,
            idTypeOfEvent: updated.IdEvent ?? templateOld.IdTypeOfEvent,
            filePath: updated.FilePath ?? templateOld.FilePath,
            previewPath: updated.PreviewPath ?? templateOld.PreviewPath
        );

        await _templateRepository.UpdateAsync(templateNew);
        return Result.Success();
    }

    /// <summary>
    /// Удалить шаблон
    /// </summary>
    /// <param name="id">id удаляемого шаблона</param>
    /// <exception cref="EntityNotFoundException">если шаблона с переданным id не существует</exception>
    public async Task<Result> DeleteTemplate(int id)
    {
        var template = await _templateRepository.GetAsync(id);
        if (template == null)
        {
            return new EntityNotFoundException(nameof(template), id);
        }
        
        await _templateRepository.RemoveAsync(id);
        return Result.Success();
    }

    /// <summary>
    /// Получить информацию обо всех шаблонах
    /// </summary>
    /// <remarks>В частности выдает пути к файлам шаблона и превью, которые фронтенд может у себя отобразить</remarks>
    /// <returns>список информации обо всех существующих шаблонах</returns>
    public async Task<Result<List<TemplateDto>>> GetTemplates()
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.ConvertAll(t => t.ToDto());
    }

    /// <summary>
    /// Получить информацию о данном шаблоне
    /// </summary>
    /// <param name="id">ID выбранного шаблона</param>
    /// <returns>информация о выбранном шаблоне</returns>
    /// <exception cref="EntityNotFoundException">если шаблона с переданным id не существует</exception>
    public async Task<Result<TemplateDto>> GetTemplateDetails(int id)
    {
        var template = await _templateRepository.GetAsync(id);
        if (template == null)
        {
            return new EntityNotFoundException(nameof(template), id);
        }

        return template.ToDto();
    }
}