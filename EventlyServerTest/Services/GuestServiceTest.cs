using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;

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
    }

    public void Dispose()
    {
        var invitation = _invitationRepository.Items.SingleOrDefault(i => i.Responses.Count > 0);
        invitation.Responses.Clear();
        _invitationRepository.Update(invitation);

        var guest = _guestRepository.Items.SingleOrDefault();
        _guestRepository.Remove(guest.Id);
    }
    
    [Fact]
    public async Task TakeInvitation_Test()
    {
        GuestCreatingDto guestCreatingDto = new GuestCreatingDto("Akakiy Petrov", "11111111111");

        await _guestService.TakeInvitation(guestCreatingDto, 2);

        var guestCreated = _guestRepository.Items.SingleOrDefault(g => g.Name == "Akakiy Petrov");
        Assert.NotNull(guestCreated);

        var invitation = _invitationRepository
            .Items
            .SingleOrDefault();
        
        Assert.NotNull(invitation);
        Assert.NotEmpty(invitation.Responses);
        Assert.True(invitation.Responses.Exists(r => r.IdGuest == guestCreated.Id));
    }
}