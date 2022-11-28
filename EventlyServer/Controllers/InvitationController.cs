using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Работа с приглашениями
/// </summary>
public class InvitationController : BaseApiController
{
    private readonly LandingInvitationService _landingInvitationService;
    private readonly IValidator<LandingInvitationCreatingDto> _creatingValidator;
    private readonly IValidator<LandingInvitationUpdatingDto> _updatingValidator;

    public InvitationController(LandingInvitationService landingInvitationService,
        IValidator<LandingInvitationCreatingDto> creatingValidator,
        IValidator<LandingInvitationUpdatingDto> updatingValidator)
    {
        _landingInvitationService = landingInvitationService;
        _creatingValidator = creatingValidator;
        _updatingValidator = updatingValidator;
    }

    /// <summary>
    /// Заказать приглашение
    /// </summary>
    /// <param name="invitation">Описание заказа</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация пользователя
    /// </remarks>
    /// <response code="200">Заказ принят</response>
    /// <response code="400">Данные не прошли валидацию</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpPost]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> OrderInvitation([FromBody] LandingInvitationCreatingDto invitation)
    {
        var validationResult = await _creatingValidator.ValidateAsync(invitation);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();

        if (!UserId.IsSuccess)
            return UserId.ConvertToEmptyResult().ToResponse();
        
        var data = await _landingInvitationService.AddInvitation(invitation, UserId.Value);
        return data.ToResponse();
    }

    /// <summary>
    /// Получить подробную информацию о выбранном приглашении
    /// </summary>
    /// <param name="id">ID выбранного приглашения</param>
    /// <returns>Подробную информацию о выбранном приглашении</returns>
    /// <remarks>
    /// Требуется авторизация пользователя или администратора
    /// </remarks>
    /// <response code="200">Подробная информация о выбранном приглашении</response>
    /// <response code="400">Заказ с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.USER) + "," + nameof(UserRoles.ADMIN))]
    [ProducesResponseType(typeof(LandingInvitationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LandingInvitationDto>> GetInvitationDetails([FromRoute] int id)
    {
        var data = await _landingInvitationService.GetInvitationDetails(id);
        return data.ToResponse();
    }

    /// <summary>
    /// Получить все приглашения клиента (в сокращенном виде)
    /// </summary>
    /// <returns>Список приглашений клиента</returns>
    /// <remarks>
    /// Требуется авторизация пользователя
    /// </remarks>
    /// <response code="200">Список приглашений клиента</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.USER))]
    [ProducesResponseType(typeof(List<LandingInvitationShortDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<LandingInvitationShortDto>>> GetAllUsersInvitations()
    {
        var data = await _landingInvitationService.GetInvitationsByUserId(UserId);
        return data.ToResponse();
    }

    /// <summary>
    /// Получить все приглашения выбранного клиента (в сокращенном виде)
    /// </summary>
    /// <param name="clientId">ID выбранного клиента</param>
    /// <returns>Список приглашений клиента</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Список приглашений клиента</response>
    /// <response code="400">Клиент с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpGet]
    [Route("admin/{clientId:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(typeof(List<LandingInvitationShortDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<LandingInvitationShortDto>>> GetAllUsersInvitationsById([FromRoute] int clientId)
    {
        var data = await _landingInvitationService.GetInvitationsByUserId(clientId);
        return data.ToResponse();
    }

    /// <summary>
    /// Отредактировать заказ клиента
    /// </summary>
    /// <param name="newInvitation">Информация об изменениях</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Заказ обновлен</response>
    /// <response code="400">Данные не прошли валидацию или заказ с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPut]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> EditInvitation([FromBody] LandingInvitationUpdatingDto newInvitation)
    {
        var validationResult = await _updatingValidator.ValidateAsync(newInvitation);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();
        
        var data = await _landingInvitationService.UpdateInvitation(newInvitation);
        return data.ToResponse();
    }

    /// <summary>
    /// Удалить выбранное приглашение
    /// </summary>
    /// <param name="id">ID удаляемого приглашения</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Заказ удален</response>
    /// <response code="400">Заказ с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteInvitation([FromRoute] int id)
    {
        var data = await _landingInvitationService.DeleteInvitation(id);
        return data.ToResponse();
    }
}