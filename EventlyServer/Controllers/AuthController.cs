using System.Security;
using System.Security.Authentication;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Services;
using EventlyServer.Services.Security;
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
            var token = await _userService.Register(user);
            var email = TokenService.GetLoginFromToken(token);

            if (email == null) throw new SecurityException("Incorrect token");

            var created = await _userService.GetUserByEmail(email);
            return created.ToShortDto(token);
        }
        catch (AuthenticationException e)
        {
            return Conflict(e.Message);
        }
        catch (SecurityException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (ArgumentException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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

            if (email == null) throw new SecurityException("Incorrect token");

            var logged = await _userService.GetUserByEmail(email);
            return logged.ToShortDto(token);
        }
        catch (AuthenticationException e)
        {
            return BadRequest(e.Message);
        }
        catch (SecurityException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (ArgumentException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}