using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Extensions;
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
    /// <exception cref="EntityNotFoundException">если мероприятия с переданным id не существует</exception>
    /// <exception cref="EntityExistsException">если гость с таким номером телефона уже зарегистрировался на мероприятие</exception>
    public async Task<Result> TakeInvitation(GuestFullCreatingDto guest)
    {
        try
        {
            var invitation = await _invitationRepository.Items.FirstOrDefaultAsync(i => i.Id == guest.IdInvitation);
            if (invitation == null)
            {
                return new EntityNotFoundException(nameof(invitation), guest.IdInvitation);
            }
            
            await _guestRepository.AddAsync(guest.ToGuest());
            return Result.Success();
        }
        catch (DbUpdateException e)
        {
            return new EntityExistsException(nameof(LandingInvitation), guest.PhoneNumber);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Удалить выбранного гостя
    /// </summary>
    /// <param name="guestId">ID удаляемого гостя</param>
    /// <exception cref="EntityNotFoundException">если гость с переданным id не существует</exception>
    public async Task<Result> DeleteGuest(int guestId)
    {
        try
        {
            var guest = await _guestRepository.GetAsync(guestId);
            if (guest == null)
            {
                return new EntityNotFoundException(nameof(guest), guestId);
            }
        
            await _guestRepository.RemoveAsync(guestId);
            return Result.Success();
        }
        catch (Exception e)
        {
            return e;
        }
    }
}