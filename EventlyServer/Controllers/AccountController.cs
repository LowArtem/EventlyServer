using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserService _userService;
    private readonly IValidator<UserUpdateDto> _validator;

    public AccountController(UserService userService, IValidator<UserUpdateDto> validator)
    {
        _userService = userService;
        _validator = validator;
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
    [Route("all")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var data = await _userService.GetAllUsers();
        return data.ToResponse();
    }

    /// <summary>
    /// Получить информацию о текущем пользователе
    /// </summary>
    /// <returns>Информация о текущем пользователе</returns>
    /// <remarks>
    /// Требуется авторизация пользователя или администратора
    /// </remarks>
    /// <response code="200">Информация о текущем пользователе</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.USER) + "," + nameof(UserRoles.ADMIN))]
    [ProducesResponseType(typeof(UserSecretDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserSecretDto>> GetUser()
    {
        if (!UserId.IsSuccess)
            return UserId.ConvertToEmptyResult().ToResponse();

        var user = await _userService.GetUserById(UserId.Value);
        return user.ToResponse(u => u.ToSecretDto());
    }

    /// <summary>
    /// Изменить аккаунт выбранного пользователя
    /// </summary>
    /// <param name="user">Обновленная информация</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
    /// Требуется авторизация пользователя
    /// </remarks>
    /// <response code="200">Аккаунт успешно изменен</response>
    /// <response code="400">Данные не прошли валидацию или пользователя с таким ID не существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPut]
    [Route("")]
    [Authorize(Roles = nameof(UserRoles.USER) + "," + nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> EditUserAccount([FromBody] UserUpdateDto user)
    {
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();

        if (!UserId.IsSuccess)
            return UserId.ConvertToEmptyResult().ToResponse();
        
        var data = await _userService.UpdateUser(user, UserId.Value);
        return data.ToResponse();
    }

    /// <summary>
    /// Удалить выбранный аккаунт
    /// </summary>
    /// <param name="id">ID удаляемого пользователя</param>
    /// <returns>Код статуса</returns>
    /// <remarks>
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
        var data = await _userService.DeleteUser(id);
        return data.ToResponse();
    }
}