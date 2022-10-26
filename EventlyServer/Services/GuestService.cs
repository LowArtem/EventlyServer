using System.Data;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с гостями
/// </summary>
public class GuestService
{
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<LandingInvitation> _invitationRepository;

    public GuestService(IRepository<Guest> guestRepository, IRepository<LandingInvitation> invitationRepository)
    {
        _guestRepository = guestRepository;
        _invitationRepository = invitationRepository;
    }

    /// <summary>
    /// Зарегистрировать нового гостя в мероприятии (гость принял приглашение)
    /// </summary>
    /// <param name="guest">информаци о госте</param>
    /// <exception cref="InvalidDataException">если мероприятия с переданным id не существует</exception>
    /// <exception cref="DataException">если гость с таким номером телефона уже зарегистрировался на мероприятие</exception>
    public async Task TakeInvitation(GuestFullCreatingDto guest)
    {
        var invitation = await _invitationRepository.Items.FirstOrDefaultAsync(i => i.Id == guest.IdInvitation);
        if (invitation == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        }

        try
        {
            await _guestRepository.AddAsync(guest.ToGuest());
        }
        catch (DataException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}