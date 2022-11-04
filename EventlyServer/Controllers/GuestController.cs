using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Работа с гостями
/// </summary>
public class GuestController : BaseApiController
{
    private readonly GuestService _guestService;

    public GuestController(GuestService guestService)
    {
        _guestService = guestService;
    }

    /// <summary>
    /// Принять приглашение
    /// </summary>
    /// <param name="guest">Данные гостя</param>
    /// <returns>Код статуса</returns>
    /// <response code="200">Приглашение принято</response>
    /// <response code="400">Приглашения с таким ID не существует</response>
    /// <response code="409">Гость с таким номером телефона уже принял данное приглашение</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> TakeInvitation([FromBody] GuestFullCreatingDto guest)
    {
        var data = await _guestService.TakeInvitation(guest);
        return data.ToResponse();
    }

    /// <summary>
    /// Удалить выбранного гостя
    /// </summary>
    /// <param name="id">ID выбранного гостя</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Гость удален</response>
    /// <response code="400">Гостя с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteGuest([FromRoute] int id)
    {
        var data = await _guestService.DeleteGuest(id);
        return data.ToResponse();
    }
}