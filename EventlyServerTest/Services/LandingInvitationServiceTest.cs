using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;

namespace EventlyServerTest.Services;

public class LandingInvitationServiceTest : IDisposable
{
    private readonly LandingInvitationService _invitationService;
    private readonly IRepository<LandingInvitation> _invitationRepository;


    public LandingInvitationServiceTest(LandingInvitationService invitationService, IRepository<LandingInvitation> invitationRepository)
    {
        _invitationService = invitationService;
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
        var invitation = _invitationRepository.Items.SingleOrDefault();
        _invitationRepository.Remove(invitation.Id);
    }

    [Fact]
    public async Task GetInvitationDetails_Test()
    {
        var invitation = _invitationRepository.Items.SingleOrDefault();
        
        Assert.NotNull(invitation);

        var result = await _invitationService.GetInvitationDetails(invitation!.Id);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value!.Template);
        Assert.NotNull(result.Value!.Template.Name);
        Assert.NotNull(result.Value!.Template.Event);
    }
}