using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class TemplateMapper
{
    public static TemplateDto ToDto(this Template template) => 
        new TemplateDto(template.Id, template.Price, template.Name, template.ChosenTypeOfEvent.ToDto(), template.FilePath, template.PreviewPath);

    public static Template ToTemplate(this TemplateDto templateDto) => 
        new Template(templateDto.Price, templateDto.Name, templateDto.Event.Id, templateDto.FilePath, templateDto.PreviewPath);
    
    public static Template ToTemplate(this TemplateCreatingDto templateDto) =>
        new Template(templateDto.Price, templateDto.Name, templateDto.IdEvent, templateDto.FilePath, templateDto.PreviewPath);
}