using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace EventlyServer.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly UserService _userService;

    public TestController(IRepository<User> userRepository, UserService userService)
    {
        this._userRepository = userRepository;
        _userService = userService;
    }
    
    [HttpGet]
    [Route("test")]
    [Authorize]
    public string Test()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? "Login is not defined";
        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "Role is not defined";

        return $"Login - {login}, role - {role}";
    }

    [HttpGet]
    [Route("get")]
    [Authorize]
    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = nameof(UserRoles.ADMIN))]
    public async Task<User> AddUser([FromBody] UserDto user)
    {
        return await _userRepository.AddAsync(user.ToUser());
    }
    
    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <param name="user">информация о новом пользователе</param>
    /// <returns>полностью сформированный класс с информацией о пользователе</returns>
    /// <response code="200">Возвращает созданного пользователя</response>
    /// <response code="409">Пользователь с таким email уже существует</response>
    [HttpPost]
    [Route("register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Register([FromBody] UserDto user)
    {
        try
        {
            string token =  await _userService.Register(user);
            var created = await _userService.GetUserByEmail(TokenService.GetLoginFromToken(token));
            return created;
        }
        catch (AuthenticationException e)
        {
            return Conflict(e.Message);
        }
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<string> Login([FromBody] UserAuthDto userAuthDto)
    {
        return await _userService.Login(userAuthDto.Email, userAuthDto.Password);
    }
}