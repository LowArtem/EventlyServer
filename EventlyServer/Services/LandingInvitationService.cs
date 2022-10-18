using EventlyServer.Data.Repositories;

namespace EventlyServer.Services;

public class LandingInvitationService
{
    private readonly LandingInvitationRepository _landingInvitationRepository;
    private readonly UserRepository _userRepository;

    public LandingInvitationService(LandingInvitationRepository landingInvitationRepository, UserRepository userRepository)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _userRepository = userRepository;
    }
    
    
}