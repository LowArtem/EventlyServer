using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/* TODO: Исправить шаблон
    Шаблон представляет собой физический файл на сервере. 
    Для работы с ним необходимо передавать клиенту и получать от него непосредственно файл.
    Эту информацию нужно изучить и дописать потом эту логику.
*/

public class TemplateService
{
    private readonly IRepository<Template> _templateRepository;

    public TemplateService(IRepository<Template> templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task AddTemplate(TemplateDto template)
    {
        var templateTest = await _templateRepository.Items.SingleOrDefaultAsync(t => t.Name == template.Name);
        if (templateTest != null)
        {
            throw new ArgumentException("Template with this name already exists", nameof(template));
        }

        await _templateRepository.AddAsync(template.ToTemplate());
    }

    public async Task UpdateTemplate(TemplateUpdateDto updated)
    {
        var templateOld = await _templateRepository.GetAsync(updated.Id);
        if (templateOld == null)
        {
            throw new InvalidDataException("Template with given id cannot be found");
        }

        Template templateNew = new Template(
            updated.Id,
            updated.Price != null ? (int)updated.Price : templateOld.Price,
            updated.Name != null ? updated.Name : templateOld.Name,
            updated.Event != null ? updated.Event.Id : templateOld.IdTypeOfEvent,
            updated.FilePath != null ? updated.FilePath : templateOld.FilePath,
            updated.PreviewPath != null ? updated.PreviewPath : templateOld.PreviewPath
        );

        await _templateRepository.UpdateAsync(templateNew);
    }

    public async Task DeleteTemplate(int id)
    {
        var template = await _templateRepository.GetAsync(id);
        if (template == null)
        {
            throw new InvalidDataException("Template with given id cannot be found");
        }
        
        await _templateRepository.RemoveAsync(id);
    }

    public async Task<List<TemplateDto>> ShowTemplates()
    {
        var templates = await _templateRepository.GetAllAsync();
        throw new NotImplementedException();
        // TODO: нужно разобраться со static files
    }
}