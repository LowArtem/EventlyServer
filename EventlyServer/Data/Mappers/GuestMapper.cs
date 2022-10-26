using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class GuestMapper
{
    public static GuestDto ToDto(this Guest guest) => new GuestDto(guest.Id, guest.Name);
    
    public static Guest ToGuest(this GuestFullCreatingDto guestDto) =>
        new Guest(guestDto.Name, guestDto.PhoneNumber, guestDto.IdInvitation);
}