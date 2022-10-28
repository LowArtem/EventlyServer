using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services.Security;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с приглашениями
/// </summary>
public class LandingInvitationService
{
    private readonly IRepository<LandingInvitation> _landingInvitationRepository;
    private readonly TokenService _tokenService;

    public LandingInvitationService(IRepository<LandingInvitation> landingInvitationRepository, TokenService tokenService)
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
    /// <param name="invitationInfo">информация о приглашении</param>
    public async Task AddInvitation(LandingInvitationCreatingDto invitationInfo)
    {
        await _landingInvitationRepository.AddAsync(invitationInfo.ToLandingInvitation());
    }

    /// <summary>
    /// Обновить информацию о приглашении
    /// </summary>
    /// <param name="invitationInfo">обновленная информация о приглашении</param>
    public async Task UpdateInvitation(LandingInvitationUpdatingDto invitationInfo)
    {
        var invitationOld = await _landingInvitationRepository.GetAsync(invitationInfo.Id);
        if (invitationOld == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        }

        var invitationNew = new LandingInvitation(
            id: invitationInfo.Id,
            link: invitationInfo.Link != "" ? invitationInfo.Link : invitationOld.Link,
            name: invitationInfo.Name ?? invitationOld.Name,
            orderStatus: invitationInfo.OrderStatus ?? invitationOld.OrderStatus,
            startDate: invitationInfo.StartDate ?? invitationOld.StartDate,
            finishDate: invitationInfo.FinishDate ?? invitationOld.FinishDate,
            idClient: invitationInfo.IdClient ?? invitationOld.IdClient,
            idTemplate: invitationInfo.IdTemplate ?? invitationOld.IdTemplate
        );
        
        await _landingInvitationRepository.UpdateAsync(invitationNew);
    }

    /// <summary>
    /// Удалить выбранное приглашение
    /// </summary>
    /// <param name="id">id выбранноо приглашения</param>
    public async Task DeleteInvitation(int id)
    {
        var invitation = await _landingInvitationRepository.GetAsync(id);
        if (invitation == null)
        {
            throw new InvalidDataException("Invitation with given id cannot be found");
        }
        
        await _landingInvitationRepository.RemoveAsync(id);
    }
}