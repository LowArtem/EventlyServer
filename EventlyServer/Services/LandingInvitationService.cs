using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Services.Security;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с приглашениями
/// </summary>
public class LandingInvitationService
{
    private readonly IRepository<LandingInvitation> _landingInvitationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly TokenService _tokenService;

    public LandingInvitationService(IRepository<LandingInvitation> landingInvitationRepository, TokenService tokenService, IRepository<User> userRepository)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// Получить список приглашений пользователя по его ID
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns>список сокращенных представлений приглашений</returns>
    /// <exception cref="EntityNotFoundException">если пользователь с таким ID не существует</exception>
    public async Task<List<LandingInvitationShortDto>> GetInvitationsByUserId(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            throw new EntityNotFoundException("User with given id cannot be found");
        }
        
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }

    /// <summary>
    /// Получить полную информацию о выбранном приглашении
    /// </summary>
    /// <param name="id">id выбранного приглашения</param>
    /// <returns>полная информация о приглашении</returns>
    /// <exception cref="EntityNotFoundException">если приглашения с данным id не существует</exception>
    public async Task<LandingInvitationDto> GetInvitationDetails(int id)
    {
        var invitation = await _landingInvitationRepository.GetAsync(id);
        if (invitation == null)
        {
            throw new EntityNotFoundException("Invitation with given id cannot be found");
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
    /// <exception cref="EntityNotFoundException">если приглашения с переданным ID не существует</exception>
    public async Task UpdateInvitation(LandingInvitationUpdatingDto invitationInfo)
    {
        var invitationOld = await _landingInvitationRepository.GetAsync(invitationInfo.Id);
        if (invitationOld == null)
        {
            throw new EntityNotFoundException("Invitation with given id cannot be found");
        }

        var invitationNew = new LandingInvitation(
            id: invitationInfo.Id,
            link: invitationInfo.Link != "" ? invitationInfo.Link : invitationOld.Link,
            name: invitationInfo.Name ?? invitationOld.Name,
            orderStatus: invitationInfo.OrderStatus ?? invitationOld.OrderStatus,
            startDate: invitationInfo.StartDate != null ? DateOnly.FromDateTime((DateTime)invitationInfo.StartDate) : invitationOld.StartDate,
            finishDate: invitationInfo.FinishDate != null ? DateOnly.FromDateTime((DateTime)invitationInfo.FinishDate) : invitationOld.FinishDate,
            idClient: invitationOld.IdClient,
            idTemplate: invitationInfo.IdTemplate ?? invitationOld.IdTemplate
        );
        
        await _landingInvitationRepository.UpdateAsync(invitationNew);
    }

    /// <summary>
    /// Удалить выбранное приглашение
    /// </summary>
    /// <param name="id">id выбранноо приглашения</param>
    /// <exception cref="EntityNotFoundException">если приглашения с переданным ID не существует</exception>
    public async Task DeleteInvitation(int id)
    {
        var invitation = await _landingInvitationRepository.GetAsync(id);
        if (invitation == null)
        {
            throw new EntityNotFoundException("Invitation with given id cannot be found");
        }
        
        await _landingInvitationRepository.RemoveAsync(id);
    }
}