using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>Всех пользователей</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Список всех пользователей</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpGet]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        return await this.SendResponseAsync(async () => await _userService.GetAllUsers());
    }

    /// <summary>
    /// Изменить аккаунт выбранного пользователя
    /// </summary>
    /// <param name="user">Обновленная информация</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Аккаунт успешно изменен</response>
    /// <response code="400">Пользователя с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPut]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> EditUserAccount([FromBody] UserUpdateDto user)
    {
        return await this.SendResponseAsync(async () => await _userService.UpdateUser(user));
    }

    /// <summary>
    /// Удалить выбранный аккаунт
    /// </summary>
    /// <param name="id">ID удаляемого пользователя</param>
    /// <returns>Код статуса</returns>
    /// /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Аккаунт успешно удален</response>
    /// <response code="400">Пользователя с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteUserAccount([FromRoute] int id)
    {
        return await this.SendResponseAsync(async () => await _userService.DeleteUser(id));
    }
}