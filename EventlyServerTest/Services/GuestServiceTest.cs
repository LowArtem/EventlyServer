using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Services;

namespace EventlyServerTest.Services;

public sealed class GuestServiceTest : IDisposable
{
    private readonly GuestService _guestService;
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<LandingInvitation> _invitationRepository;

    private int _invitationId = 0;

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
            "Invitation 1",
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today).AddDays(7), 1, 1
        );
        var created = _invitationRepository.Add(invitation);
        _invitationId = created.Id;
    }

    public void Dispose()
    {
        var guest = _guestRepository.Items.SingleOrDefault();
        _guestRepository.Remove(guest.Id);
        
        _invitationRepository.Remove(_invitationId);
    }
    
    [Fact]
    public async Task TakeInvitation_Test()
    {
        GuestFullCreatingDto guestCreatingDto = new GuestFullCreatingDto("Akakiy Petrov", "11111111111", _invitationId);

        await _guestService.TakeInvitation(guestCreatingDto);

        var guestCreated = _guestRepository.Items.SingleOrDefault(g => g.Name == "Akakiy Petrov");
        Assert.NotNull(guestCreated);

        var invitation = _invitationRepository.Get(_invitationId);
        
        Assert.NotNull(invitation);
        Assert.NotEmpty(invitation.Guests);
        Assert.True(invitation.Guests.Exists(r => r.Id == guestCreated.Id));
    }

    [Fact]
    public async Task TakeInvitation_SameGuest()
    {
        GuestFullCreatingDto guestCreatingDto = new GuestFullCreatingDto("Akakiy Sidorov", "11111111111", _invitationId);
        _guestService.TakeInvitation(guestCreatingDto).Wait();

        var result = await _guestService.TakeInvitation(guestCreatingDto);
        
        Assert.False(result.IsSuccess);
        Assert.True(result.ExceptionIs<EntityExistsException>());
    }
}