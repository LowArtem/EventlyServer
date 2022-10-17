using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class TemplateMapper
{
    public static TemplateDto ToDto(this Template template) => 
        new TemplateDto(template.Id, template.Price, template.ChosenTypeOfEvent.ToDto(), template.FilePath, template.PreviewPath);

    public static Template ToTemplate(this TemplateDto templateDto) => 
        new Template(templateDto.Price, templateDto.Event.Id, templateDto.FilePath, templateDto.PreviewPath);
}