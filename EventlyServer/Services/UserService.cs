using System.Security.Authentication;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Services;

/// <summary>
/// Класс для обработки запросов, связанных с информацией о пользователе
/// </summary>
public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Получает ID пользователя с переданными учетными данными
    /// </summary>
    /// <param name="email">имейл пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>ID данного пользователя</returns>
    /// <exception cref="AuthenticationException">если пользователь с такими учетными данными не обнаружен</exception>
    public async Task<int> Login(string email, string password)
    {
        var user = await _userRepository
            .Items
            .FirstOrDefaultAsync(item => item.Email == email && item.Password == password);

        return user?.Id ?? throw new AuthenticationException("User with these credentials cannot be found");
    }

    /// <summary>
    /// Добавляет нового пользователя
    /// </summary>
    /// <param name="user">информация о новом пользователе</param>
    /// <returns>ID данного пользователя</returns>
    /// <exception cref="AuthenticationException">если пользователь с таким имейлом уже существует</exception>
    public async Task<int> Register(UserDto user)
    {
        var testUser = await _userRepository.Items.FirstOrDefaultAsync(item => item.Email == user.Email);
        if (testUser != null)
        {
            throw new AuthenticationException("User with this email already exists");
        }

        var registeredUser = await _userRepository.AddAsync(user.ToUser());
        return registeredUser.Id;
    }

    /// <summary>
    /// Получает пользователя по переданному ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>информацию о данном пользователе</returns>
    /// <exception cref="ArgumentException">если пользователь с таким ID не существует</exception>
    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _userRepository.GetAsync(id);
        return user?.ToDto() ?? throw new ArgumentException("User with this id cannot be found");
    }
}