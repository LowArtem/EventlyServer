using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class TypesOfEventMapper
{
    public static TypesOfEventDto ToDto(this TypesOfEvent type) => new TypesOfEventDto(type.Id, type.Name);

    public static TypesOfEvent ToTypesOfEvent(this TypesOfEventDto typeDto) => new TypesOfEvent(typeDto.Name);
}