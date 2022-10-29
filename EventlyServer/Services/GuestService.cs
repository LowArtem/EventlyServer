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
    /// <exception cref="DbUpdateException">если гость с таким номером телефона уже зарегистрировался на мероприятие</exception>
    public async Task TakeInvitation(GuestFullCreatingDto guest)
    {
        var invitation = await _invitationRepository.Items.FirstOrDefaultAsync(i => i.Id == guest.IdInvitation);
        if (invitation == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        } 
        
        await _guestRepository.AddAsync(guest.ToGuest());
    }

    /// <summary>
    /// Удалить выбранного гостя
    /// </summary>
    /// <param name="guestId">ID удаляемого гостя</param>
    /// <exception cref="InvalidDataException">если гость с переданным id не существует</exception>
    public async Task DeleteGuest(int guestId)
    {
        var guest = await _guestRepository.GetAsync(guestId);
        if (guest == null)
        {
            throw new InvalidDataException("Guest with given id cannot be found");
        }
        
        await _guestRepository.RemoveAsync(guestId);
    }
}