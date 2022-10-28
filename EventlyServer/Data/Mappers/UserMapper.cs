using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user) =>
        new UserDto(user.Id, user.Name, user.Email, user.Password, user.PhoneNumber, user.OtherCommunication, user.IsAdmin);

    public static User ToUser(this UserDto dto) =>
        new User(dto.Name, dto.Email, dto.Password, dto.PhoneNumber, dto.OtherCommunication, dto.IsAdmin);
}