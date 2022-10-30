using System.Security.Authentication;
using System.Security.Claims;
using EventlyServer.Data.Dto;
using EventlyServer.Exceptions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Работа с приглашениями
/// </summary>
[ApiController]
[Route("api/invitation")]
[Produces("application/json")]
public class InvitationController : ControllerBase
{
    private readonly LandingInvitationService _landingInvitationService;

    public InvitationController(LandingInvitationService landingInvitationService)
    {
        _landingInvitationService = landingInvitationService;
    }

    /// <summary>
    /// Заказать приглашение
    /// </summary>
    /// <param name="invitation">Описание заказа</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация пользователя
    /// </remarks>
    /// <response code="201">Заказ принят</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpPost]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.USER))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> OrderInvitation([FromBody] LandingInvitationCreatingDto invitation)
    {
        try
        {
            await _landingInvitationService.AddInvitation(invitation);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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
    /// <response code="404">Заказ с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.USER) + "," + nameof(UserRoles.ADMIN))]
    [ProducesResponseType(typeof(LandingInvitationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LandingInvitationDto>> GetInvitationDetails([FromRoute] int id)
    {
        try
        {
            return await _landingInvitationService.GetInvitationDetails(id);
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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
        try
        {
            // Получение логина зарегистрированного пользователя
            var login = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (login == null)
            {
                throw new AuthenticationException("Login received from token is null, incorrect token");
            }

            return await _landingInvitationService.GetInvitationsByUser(login);
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
        }
        catch (AuthenticationException e)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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
    /// <response code="404">Клиент с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpGet]
    [Route("admin/{clientId:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(typeof(List<LandingInvitationShortDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<LandingInvitationShortDto>>> GetAllUsersInvitationsById([FromRoute] int clientId)
    {
        try
        {
            return await _landingInvitationService.GetInvitationsByUserId(clientId);
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Отредактировать заказ клиента
    /// </summary>
    /// <param name="newInvitation">Информация об изменениях</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="202">Заказ обновлен</response>
    /// <response code="404">Заказ с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPut]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> EditInvitation([FromBody] LandingInvitationUpdatingDto newInvitation)
    {
        try
        {
            await _landingInvitationService.UpdateInvitation(newInvitation);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}