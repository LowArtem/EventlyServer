namespace EventlyServer.Data.Dto;

public record TemplateDto(int Id, int Price, string Name, TypesOfEventDto Event, string FilePath, string PreviewPath);

public record TemplateUpdateDto(int Id, int? Price = null, string? Name = null, TypesOfEventDto? Event = null,
    string? FilePath = null, string? PreviewPath = null);