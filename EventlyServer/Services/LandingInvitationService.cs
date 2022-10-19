using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories;
using EventlyServer.Services.Security;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

public class LandingInvitationService
{
    private readonly LandingInvitationRepository _landingInvitationRepository;
    private readonly UserRepository _userRepository;

    public LandingInvitationService(LandingInvitationRepository landingInvitationRepository, UserRepository userRepository, TokenService tokenService, UserService userService)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _userRepository = userRepository;
    }

    public async Task<List<LandingInvitationDto>> GetInvitationsByUser(string token)
    {
        string? login = TokenService.GetLoginFromToken(token);
        if (login == null)
        {
            throw new ArgumentException("Given token is invalid", nameof(token));
        }

        var user = await _userRepository.Items.FirstOrDefaultAsync(u => u.Email == login);
        if (user == null)
        {
            throw new InvalidDataException("User with given email cannot be found");
        }

        return user.LandingInvitations.ConvertAll(i => i.ToDto());
    }

    public async Task<List<LandingInvitationDto>> AddInvitation(string token, LandingInvitationCreatingDto dto)
    {
        string? login = TokenService.GetLoginFromToken(token);
        if (login == null)
        {
            throw new ArgumentException("Given token is invalid", nameof(token));
        }

        var user = await _userRepository.Items.FirstOrDefaultAsync(u => u.Email == login);
        if (user == null)
        {
            throw new InvalidDataException("User with given email cannot be found");
        }

        await _landingInvitationRepository.AddAsync(dto.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToDto());
    }
    
    public async Task<List<LandingInvitationDto>> UpdateInvitation(string token, LandingInvitationUpdatingDto dto)
    {
        string? login = TokenService.GetLoginFromToken(token);
        if (login == null)
        {
            throw new ArgumentException("Given token is invalid", nameof(token));
        }

        var user = await _userRepository.Items.FirstOrDefaultAsync(u => u.Email == login);
        if (user == null)
        {
            throw new InvalidDataException("User with given email cannot be found");
        }

        await _landingInvitationRepository.UpdateAsync(dto.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToDto());
    }

    public async Task DeleteInvitation(string token, int id)
    {
        string? login = TokenService.GetLoginFromToken(token);
        if (login == null)
        {
            throw new ArgumentException("Given token is invalid", nameof(token));
        }

        var user = await _userRepository.Items.FirstOrDefaultAsync(u => u.Email == login);
        if (user == null)
        {
            throw new InvalidDataException("User with given email cannot be found");
        }

        await _landingInvitationRepository.RemoveAsync(id);
    }
}