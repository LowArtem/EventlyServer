namespace EventlyServer.Data.Dto;

public record TemplateDto(int Id, int Price, TypesOfEventDto Event, string FilePath, string PreviewPath);