﻿using EventlyServer.Controllers.Abstracts;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Extensions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Аутентификация (регистрация, логин) клиента
/// </summary>
public class AuthController : BaseApiController
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Регистрация нового клиента
    /// </summary>
    /// <param name="user">Данные клиента</param>
    /// <returns>Базовая информация об аккаунте клиента</returns>
    /// <response code="200">Базовая информация о созданном аккаунте</response>
    /// <response code="400">Неверный формат данных (email или телефон)</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Ошибка при создании пользователя</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(UserShortDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserShortDto>> Register([FromBody] UserRegisterDto user)
    {
        var token = await _userService.Register(user, false);
        if (!token.IsSuccess) return BadRequest(token.Exception.Message);
            
        var email = TokenService.GetLoginFromToken(token.Value);

        var created = await _userService.GetUserByEmail(email!);
        return created.ToResponse(u => u.ToShortDto(token.Value));
    }

    /// <summary>
    /// Добавление нового администратора
    /// </summary>
    /// <param name="user">Данные нового администратора</param>
    /// <returns>Статус код</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="200">Администратор успешно добавлен</response>
    /// <response code="400">Неверный формат данных (email или телефон)</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPost]
    [Route("admin/register")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddNewAdmin([FromBody] UserRegisterDto user)
    {
        var data = await _userService.Register(user, true);
        return data.ConvertToEmptyResult().ToResponse();
    }

    /// <summary>
    /// Вход клиента
    /// </summary>
    /// <param name="user">Данные клиента</param>
    /// <returns>Базовая информация об аккаунте клиента</returns>
    /// <response code="200">Базовая информация об аккаунте</response>
    /// <response code="400">Пользователь с такими учетными данными не существует</response>
    /// <response code="500">Ошибка при получении пользователя</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(UserShortDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserShortDto>> Login([FromBody] UserLoginDto user)
    {
        var token = await _userService.Login(user.Email, user.Password);
        if (!token.IsSuccess) return BadRequest(token.Exception.Message);

        var email = TokenService.GetLoginFromToken(token.Value);

        var logged = await _userService.GetUserByEmail(email!);
        return logged.ToResponse(u => u.ToShortDto(token.Value));
    }
}