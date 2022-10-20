using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories;
using EventlyServer.Services.Security;

namespace EventlyServer.Services;

public class LandingInvitationService
{
    private readonly LandingInvitationRepository _landingInvitationRepository;
    private readonly TokenService _tokenService;

    public LandingInvitationService(LandingInvitationRepository landingInvitationRepository, TokenService tokenService)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _tokenService = tokenService;
    }

    public async Task<List<LandingInvitationShortDto>> GetInvitationsByUser(string token)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }
    
    public async Task<LandingInvitationDto> GetInvitationDetails(string token, int id)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        var invitation = user.LandingInvitations.FirstOrDefault(i => i.Id == id);
        if (invitation == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        }

        return invitation.ToDto();
    }

    public async Task<List<LandingInvitationShortDto>> AddInvitation(string token, LandingInvitationCreatingDto dto)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        await _landingInvitationRepository.AddAsync(dto.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }
    
    public async Task<List<LandingInvitationShortDto>> UpdateInvitation(string token, LandingInvitationUpdatingDto dto)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        await _landingInvitationRepository.UpdateAsync(dto.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }

    public async Task DeleteInvitation(int id)
    {
        await _landingInvitationRepository.RemoveAsync(id);
    }
}