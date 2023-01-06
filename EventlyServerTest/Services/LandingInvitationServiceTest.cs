using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;

namespace EventlyServerTest.Services;

public sealed class LandingInvitationServiceTest : IDisposable
{
    private readonly LandingInvitationService _invitationService;
    private readonly IRepository<LandingInvitation> _invitationRepository;

    private int _invitationId = 0;

    public LandingInvitationServiceTest(LandingInvitationService invitationService, IRepository<LandingInvitation> invitationRepository)
    {
        _invitationService = invitationService;
        _invitationRepository = invitationRepository;
        
        Setup();
    }

    private void Setup()
    {
        var invitation = new LandingInvitation(
            "Invitation 2",
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today).AddDays(7), 1, 1
        );
        var created = _invitationRepository.Add(invitation);
        _invitationId = created.Id;
    }
    
    public void Dispose()
    {
        _invitationRepository.Remove(_invitationId);
    }

    [Fact]
    public async Task GetInvitationDetails_Test()
    {
        var result = await _invitationService.GetInvitationDetails(_invitationId);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value!.Template);
        Assert.NotNull(result.Value!.Template.Name);
        Assert.NotNull(result.Value!.Template.Event);
    }
}