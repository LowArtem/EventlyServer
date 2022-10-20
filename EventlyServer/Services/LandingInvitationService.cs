using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories;
using EventlyServer.Services.Security;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с приглашениями
/// </summary>
public class LandingInvitationService
{
    private readonly LandingInvitationRepository _landingInvitationRepository;
    private readonly TokenService _tokenService;

    public LandingInvitationService(LandingInvitationRepository landingInvitationRepository, TokenService tokenService)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Получить список приглашений пользователя
    /// </summary>
    /// <param name="token">JWT-токен</param>
    /// <returns>список сокращенных предствлений приглашений</returns>
    public async Task<List<LandingInvitationShortDto>> GetInvitationsByUser(string token)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }
    
    /// <summary>
    /// Получить полную информацию о выбранном приглашении
    /// </summary>
    /// <param name="token">JWT-токен</param>
    /// <param name="id">id выбранного приглашения</param>
    /// <returns>полная информация о приглашении</returns>
    /// <exception cref="InvalidDataException">если приглашения с данным id не существует</exception>
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

    /// <summary>
    /// Добавить приглашение
    /// </summary>
    /// <param name="token">JWT-токен</param>
    /// <param name="invitationInfo">информация о приглашении</param>
    /// <returns>список сокращенных предствлений приглашений</returns>
    public async Task<List<LandingInvitationShortDto>> AddInvitation(string token, LandingInvitationCreatingDto invitationInfo)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        await _landingInvitationRepository.AddAsync(invitationInfo.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }
    
    /// <summary>
    /// Обновить информацию о приглашении
    /// </summary>
    /// <param name="token">JWT-токен</param>
    /// <param name="invitationInfo">обновленная информация о приглашении</param>
    /// <returns>список сокращенных предствлений приглашений</returns>
    public async Task<List<LandingInvitationShortDto>> UpdateInvitation(string token, LandingInvitationUpdatingDto invitationInfo)
    {
        var user = await _tokenService.GetUserOrThrow(token);

        await _landingInvitationRepository.UpdateAsync(invitationInfo.ToLandingInvitation());
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }

    /// <summary>
    /// Удалить выбранное приглашение
    /// </summary>
    /// <param name="id">id выбранноо приглашения</param>
    public async Task DeleteInvitation(int id)
    {
        await _landingInvitationRepository.RemoveAsync(id);
    }
}