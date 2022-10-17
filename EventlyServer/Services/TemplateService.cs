using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;

namespace EventlyServer.Services;

public class TemplateService
{
    private readonly IRepository<Template> _templateRepository;

    public TemplateService(IRepository<Template> templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<List<TemplateDto>> ShowTemplates()
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.ConvertAll(item => item.ToDto());
    }
}