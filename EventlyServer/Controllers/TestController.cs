using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public TestController(IRepository<User> userRepository)
    {
        this._userRepository = userRepository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    [HttpPost]
    [Route("add")]
    public async Task<User> AddUser([FromBody] UserDto user)
    {
        return await _userRepository.AddAsync(new User(user));
    }
}