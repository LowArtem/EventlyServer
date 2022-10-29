using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;
using Microsoft.EntityFrameworkCore;

namespace EventlyServerTest.Services;

public class GuestServiceTest : IDisposable
{
    private readonly GuestService _guestService;
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<LandingInvitation> _invitationRepository;

    public GuestServiceTest(GuestService guestService, IRepository<Guest> guestRepository, IRepository<LandingInvitation> invitationRepository)
    {
        _guestService = guestService;
        _guestRepository = guestRepository;
        _invitationRepository = invitationRepository;
        
        Setup();
    }

    private void Setup()
    {
        var invitation = new LandingInvitation(
            "Invitation",
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today).AddDays(7), 1, 1
        );
        _invitationRepository.Add(invitation);
    }

    public void Dispose()
    {
        var guest = _guestRepository.Items.SingleOrDefault();
        _guestRepository.Remove(guest.Id);
        
        var invitation = _invitationRepository.Items.SingleOrDefault();
        _invitationRepository.Remove(invitation.Id);
    }
    
    [Fact]
    public async Task TakeInvitation_Test()
    {
        var idInvitation = _invitationRepository.Items.SingleOrDefault()!.Id;
        
        GuestFullCreatingDto guestCreatingDto = new GuestFullCreatingDto("Akakiy Petrov", "11111111111", idInvitation);

        await _guestService.TakeInvitation(guestCreatingDto);

        var guestCreated = _guestRepository.Items.SingleOrDefault(g => g.Name == "Akakiy Petrov");
        Assert.NotNull(guestCreated);

        var invitation = _invitationRepository
            .Items
            .SingleOrDefault();
        
        Assert.NotNull(invitation);
        Assert.NotEmpty(invitation.Guests);
        Assert.True(invitation.Guests.Exists(r => r.Id == guestCreated.Id));
    }

    [Fact]
    public async Task TakeInvitation_SameGuest()
    {
        var idInvitation = _invitationRepository.Items.SingleOrDefault()!.Id;
        
        GuestFullCreatingDto guestCreatingDto = new GuestFullCreatingDto("Akakiy Sidorov", "11111111111", idInvitation);
        await _guestService.TakeInvitation(guestCreatingDto);

        await Assert.ThrowsAsync<DbUpdateException>(() => _guestService.TakeInvitation(guestCreatingDto));
    }
}