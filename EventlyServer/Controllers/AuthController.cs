using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Exceptions;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

/// <summary>
/// Аутентификация (регистрация, логин) клиента
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
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
    /// <response code="201">Базовая информация о созданном аккаунте</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Ошибка при создании пользователя</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(UserShortDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserShortDto>> Register([FromBody] UserRegisterDto user)
    {
        try
        {
            var token = await _userService.Register(user, false);
            var email = TokenService.GetLoginFromToken(token);

            if (email == null) throw new ArgumentException("Incorrect token generated");

            var created = await _userService.GetUserByEmail(email);
            return StatusCode(StatusCodes.Status201Created, created.ToShortDto(token));
        }
        catch (EntityExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Добавление нового администратора
    /// </summary>
    /// <param name="user">Данные нового администратора</param>
    /// <returns>Статус код</returns>
    /// <remarks>
    /// Требуется авторизация администратора
    /// </remarks>
    /// <response code="201">Администратор успешно добавлен</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="403">Нет доступа</response>
    [HttpPost]
    [Route("admin/register")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Nullable) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddNewAdmin([FromBody] UserRegisterDto user)
    {
        try
        {
            await _userService.Register(user, true);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (EntityExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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
        try
        {
            var token = await _userService.Login(user.Email, user.Password);
            var email = TokenService.GetLoginFromToken(token);

            if (email == null) throw new ArgumentException("Incorrect token generated");

            var logged = await _userService.GetUserByEmail(email);
            return logged.ToShortDto(token);
        }
        catch (EntityNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}