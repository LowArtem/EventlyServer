using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

public class GuestService
{
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<LandingInvitation> _invitationRepository;

    public GuestService(IRepository<Guest> guestRepository, IRepository<LandingInvitation> invitationRepository)
    {
        _guestRepository = guestRepository;
        _invitationRepository = invitationRepository;
    }

    public async Task TakeInvitation(GuestCreatingDto guest, int invitationId)
    {
        var guestCreated = await _guestRepository.AddAsync(guest.ToGuest());
        
        var invitation = await _invitationRepository.Items.FirstOrDefaultAsync(i => i.Id == invitationId);
        if (invitation == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        }
        
        //TODO: где-то тут применяется триггер Макса, уточнить, как именно
        
        invitation.Responses.Add(new ResponseCreatingDto(DateTime.UtcNow, guestCreated.Id, invitationId).ToResponse());
        await _invitationRepository.UpdateAsync(invitation);
    }
}