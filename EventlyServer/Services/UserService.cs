using System.Security.Authentication;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Services.Security;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с информацией о пользователе
/// </summary>
public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly TokenService _tokenService;


    public UserService(IRepository<User> userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Получает JWT-токен для пользователя с переданными учетными данными
    /// </summary>
    /// <param name="email">имейл пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>JWT-токен для данного пользователя</returns>
    /// <exception cref="AuthenticationException">если пользователь с такими учетными данными не обнаружен</exception>
    public async Task<string> Login(string email, string password)
    {
        return await _tokenService.GenerateTokenAsync(email, password);
    }

    /// <summary>
    /// Добавляет нового пользователя
    /// </summary>
    /// <param name="user">информация о новом пользователе</param>
    /// <returns>JWT-токен для данного пользователя</returns>
    /// <exception cref="AuthenticationException">если пользователь с таким имейлом уже существует</exception>
    public async Task<string> Register(UserDto user)
    {
        var testUser = await _userRepository.Items.FirstOrDefaultAsync(item => item.Email == user.Email);
        if (testUser != null)
        {
            throw new AuthenticationException("User with this email already exists");
        }

        var registeredUser = await _userRepository.AddAsync(user.ToUser());
        return await _tokenService.GenerateTokenAsync(registeredUser.Email, registeredUser.Password);
    }

    /// <summary>
    /// Получает пользователя по имейлу
    /// </summary>
    /// <param name="email">имейл пользователя</param>
    /// <returns>информацию о данном пользователе</returns>
    /// <exception cref="ArgumentException">если пользователь с таким email не существует</exception>
    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = await _userRepository.Items.FirstOrDefaultAsync(item => item.Email == email);
        return user?.ToDto() ?? throw new ArgumentException("User with this id cannot be found");
    }
}