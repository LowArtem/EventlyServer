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

/// <summary>
/// Аутентификация (регистрация, логин) клиента
/// </summary>
public class AuthController : BaseApiController
{
    private readonly UserService _userService;
    private readonly IValidator<UserRegisterDto> _registerValidator;
    private readonly IValidator<UserLoginDto> _loginValidator;

    public AuthController(UserService userService, IValidator<UserRegisterDto> registerValidator,
        IValidator<UserLoginDto> loginValidator)
    {
        _userService = userService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    /// <summary>
    /// Регистрация нового клиента
    /// </summary>
    /// <param name="user">Данные клиента</param>
    /// <returns>Базовая информация об аккаунте клиента</returns>
    /// <response code="200">Базовая информация о созданном аккаунте</response>
    /// <response code="400">Данные не прошли валидацию</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Ошибка при создании пользователя</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(UserShortDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserShortDto>> Register([FromBody] UserRegisterDto user)
    {
        var validationResult = await _registerValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();


        var token = await _userService.Register(user, false);
        if (!token.IsSuccess) return token.ConvertToEmptyResult().ToResponse();

        var email = TokenService.GetLoginFromToken(token.Value);

        var created = await _userService.GetUserByEmail(email!);

        AppendAuthCookies(token.Value);

        return created.ToResponse(u => u.ToShortDto());
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
    /// <response code="400">Данные не прошли валидацию</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPost]
    [Route("admin/register")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddNewAdmin([FromBody] UserRegisterDto user)
    {
        var validationResult = await _registerValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();


        var data = await _userService.Register(user, true);
        return data.ConvertToEmptyResult().ToResponse();
    }

    /// <summary>
    /// Вход клиента
    /// </summary>
    /// <param name="user">Данные клиента</param>
    /// <returns>Базовая информация об аккаунте клиента</returns>
    /// <response code="200">Базовая информация об аккаунте</response>
    /// <response code="400">Данные не прошли валидацию или пользователь с такими учетными данными не существует</response>
    /// <response code="500">Неизвестная ошибка сервера</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(UserShortDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserShortDto>> Login([FromBody] UserLoginDto user)
    {
        var validationResult = await _loginValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();


        var token = await _userService.Login(user.Email, user.Password);
        if (!token.IsSuccess) return token.ConvertToEmptyResult().ToResponse();

        var email = TokenService.GetLoginFromToken(token.Value);

        var logged = await _userService.GetUserByEmail(email!);

        AppendAuthCookies(token.Value);

        return logged.ToResponse(u => u.ToShortDto());
    }

    /// <summary>
    /// Получить долгоживущий токен для локальной разработки
    /// </summary>
    /// <param name="user">Данные аккаунта</param>
    /// <returns>JWT токен авторизации</returns>
    /// <response code="200">JWT токен авторизации</response>
    /// <response code="400">Данные не прошли валидацию или пользователь с такими учетными данными не существует</response>
    /// <response code="500">Неизвестная ошибка сервера</response>
    [HttpPost]
    [Route("/dev/token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> GetDevelopmentToken([FromBody] UserLoginDto user)
    {
        var validationResult = await _loginValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return validationResult.ToResult().ToResponse();

        var token = await _userService.Login(user.Email, user.Password, true);
        return token.ToResponse();
    }

    /// <summary>
    /// Выход из аккаунта
    /// </summary>
    /// <remarks>
    /// Требуется авторизация пользователя или администратора
    /// </remarks>
    /// <returns>Код статуса</returns>
    /// <response code="200">Выход успешно осуществлен</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="500">Неизвестная ошибка сервера</response>
    [HttpGet]
    [Route("logout")]
    [Authorize(Roles = nameof(UserRoles.USER) + "," + nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status401Unauthorized)]
    public ActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete(Constants.COOKIE_ID);
        return Ok();
    }

    private void AppendAuthCookies(string token)
    {
        HttpContext.Response.Cookies.Append(Constants.COOKIE_ID, token, new CookieOptions
        {
            MaxAge = TimeSpan.FromMinutes(AuthOptions.LIFETIME)
        });
    }
}