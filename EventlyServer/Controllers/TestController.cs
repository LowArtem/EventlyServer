using System.Security.Claims;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services;
using EventlyServer.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost]
    [Route("register")]
    public async Task<UserDto> Register([FromBody] UserDto user)
    {
        string token =  await _userService.Register(user);
        return await _userService.GetUserByEmail(TokenService.GetLoginFromToken(token));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<string> Login([FromBody] UserAuthDto userAuthDto)
    {
        return await _userService.Login(userAuthDto.Email, userAuthDto.Password);
    }
}