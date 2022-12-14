using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Extensions;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с приглашениями
/// </summary>
public class LandingInvitationService
{
    private readonly IRepository<LandingInvitation> _landingInvitationRepository;
    private readonly IRepository<User> _userRepository;

    public LandingInvitationService(IRepository<LandingInvitation> landingInvitationRepository, IRepository<User> userRepository)
    {
        _landingInvitationRepository = landingInvitationRepository;
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// Получить список приглашений пользователя по его ID
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns>список сокращенных представлений приглашений</returns>
    /// <exception cref="EntityNotFoundException">если пользователь с таким ID не существует</exception>
    public async Task<Result<List<LandingInvitationShortDto>>> GetInvitationsByUserId(Result<int> id)
    {
        if (!id.IsSuccess)
            return Result.Fail<List<LandingInvitationShortDto>>(id.Exception);
        
        var user = await _userRepository.GetAsync(id.Value);
        if (user == null)
        {
            return new EntityNotFoundException(nameof(user), id);
        }
        
        return user.LandingInvitations.ConvertAll(i => i.ToShortDto());
    }

    /// <summary>
    /// Получить полную информацию о выбранном приглашении
    /// </summary>
    /// <param name="id">id выбранного приглашения</param>
    /// <returns>полная информация о приглашении</returns>
    /// <exception cref="EntityNotFoundException">если приглашения с данным id не существует</exception>
    public async Task<Result<LandingInvitationDto>> GetInvitationDetails(int id)
    {
        var invitation = await _landingInvitationRepository.GetAsync(id);
        if (invitation == null)
        {
            return new EntityNotFoundException(nameof(invitation), id);
        }

        return invitation.ToDto();
    }

    /// <summary>
    /// Добавить приглашение
    /// </summary>
    /// <param name="invitationInfo">информация о приглашении</param>
    /// <param name="idClient">ID клиента, осуществляющего заказ</param>
    public async Task<Result> AddInvitation(LandingInvitationCreatingDto invitationInfo, int idClient)
    {
        await _landingInvitationRepository.AddAsync(invitationInfo.ToLandingInvitation(idClient));
        return Result.Success();
    }

    /// <summary>
    /// Обновить информацию о приглашении
    /// </summary>
    /// <param name="invitationInfo">обновленная информация о приглашении</param>
    /// <exception cref="EntityNotFoundException">если приглашения с переданным ID не существует</exception>
    public async Task<Result> UpdateInvitation(LandingInvitationUpdatingDto invitationInfo)
    {
        var invitationOld = await _landingInvitationRepository.GetUntrackedAsync(invitationInfo.Id);
        if (invitationOld == null)
        {
            return new EntityNotFoundException(nameof(invitationInfo), invitationInfo.Id);
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
        return Result.Success();
    }

    /// <summary>
    /// Удалить выбранное приглашение
    /// </summary>
    /// <param name="id">id выбранноо приглашения</param>
    /// <exception cref="EntityNotFoundException">если приглашения с переданным ID не существует</exception>
    public async Task<Result> DeleteInvitation(int id)
    {
        var invitation = await _landingInvitationRepository.GetAsync(id);
        if (invitation == null)
        {
            return new EntityNotFoundException(nameof(invitation), id);
        }
        
        await _landingInvitationRepository.RemoveAsync(id);
        return Result.Success();
    }
}