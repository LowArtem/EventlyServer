using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;

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

    public async Task<List<TemplateDto>> ShowTemplates()
    {
        throw new NotImplementedException();
        // var templates = await _templateRepository.GetAllAsync();
        // return templates.ConvertAll(item => item.ToDto());
    }
}