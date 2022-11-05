using System.ComponentModel.DataAnnotations;
using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using EventlyServer.Data.Repositories.Abstracts;
using EventlyServer.Exceptions;
using EventlyServer.Extensions;
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
    /// <exception cref="EntityNotFoundException">если пользователь с такими учетными данными не обнаружен</exception>
    public async Task<Result<string>> Login(string email, string password)
    {
        return await _tokenService.GenerateTokenAsync(email, password);
    }

    /// <summary>
    /// Добавляет нового пользователя
    /// </summary>
    /// <param name="user">информация о новом пользователе</param>
    /// <param name="isAdmin">является ли пользователь администратором</param>
    /// <returns>JWT-токен для данного пользователя</returns>
    /// <exception cref="EntityExistsException">если пользователь с таким имейлом уже существует</exception>
    /// <exception cref="ValidationException">если email или телефон имеют неверный формат</exception>
    public async Task<Result<string>> Register(UserRegisterDto user, bool isAdmin)
    {
        var testUser = await _userRepository.Items.FirstOrDefaultAsync(item => item.Email == user.Email);
        if (testUser != null)
        {
            return new EntityExistsException("User with this email already exists");
        }

        var registeredUser = await _userRepository.AddAsync(user.ToUser(isAdmin));
        return await _tokenService.GenerateTokenAsync(registeredUser.Email, registeredUser.Password);
    }

    /// <summary>
    /// Получает пользователя по имейлу
    /// </summary>
    /// <param name="email">имейл пользователя</param>
    /// <returns>информацию о данном пользователе</returns>
    /// <exception cref="EntityNotFoundException">если пользователь с таким email не существует</exception>
    public async Task<Result<User>> GetUserByEmail(string email)
    {
        return await _tokenService.GetUserFromLoginOrThrow(email);
    }

    /// <summary>
    /// Получает пользователя по ID
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <returns>информацию о данном пользователе</returns>
    /// <exception cref="EntityNotFoundException">если пользователь с таким ID не существует</exception>
    public async Task<Result<User>> GetUserById(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null)
            return new EntityNotFoundException($"User with given id ({id}) cannot be found");
        
        return user;
    }

    /// <summary>
    /// Получить список всех пользователей
    /// </summary>
    /// <returns>список всех пользователей (может быть пустой)</returns>
    public async Task<Result<List<UserDto>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return users.ConvertAll(u => u.ToDto());
    }
    
    /// <summary>
    /// Получить список всех клиентов
    /// </summary>
    /// <returns>список всех клиентов (может быть пустой)</returns>
    public async Task<Result<List<UserDto>>> GetAllClients()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Where(u => !u.IsAdmin).ToList().ConvertAll(u => u.ToDto());
    }
    
    /// <summary>
    /// Получить список всех админов
    /// </summary>
    /// <returns>список всех админов (может быть пустой)</returns>
    public async Task<Result<List<UserDto>>> GetAllAdmins()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Where(u => u.IsAdmin).ToList().ConvertAll(u => u.ToDto());
    }

    /// <summary>
    /// Редактировать информаци о пользователе
    /// </summary>
    /// <param name="newUser">обновленная информация о пользователе</param>
    /// <exception cref="EntityNotFoundException">если пользователь с переданным ID не существует</exception>
    public async Task<Result> UpdateUser(UserUpdateDto newUser)
    {
        var userOld = await _userRepository.GetAsync(newUser.Id);
        if (userOld == null)
        {
            return new EntityNotFoundException("User with given id cannot be found");
        }

        User updating = new User(
            id: newUser.Id,
            name: newUser.Name ?? userOld.Name,
            email: userOld.Email,
            password: newUser.Password ?? userOld.Password,
            phoneNumber: newUser.PhoneNumber ?? userOld.PhoneNumber,
            otherCommunication: newUser.OtherCommunication != "" ? newUser.OtherCommunication : userOld.OtherCommunication,
            isAdmin: userOld.IsAdmin 
        );
        
        await _userRepository.UpdateAsync(updating);
        return Result.Success();
    }

    /// <summary>
    /// Удалить выбранного пользователя
    /// </summary>
    /// <param name="id">ID удаляемого пользователя</param>
    /// <exception cref="EntityNotFoundException">если пользователя с переданным ID не существует</exception>
    public async Task<Result> DeleteUser(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return new EntityNotFoundException("User with given id cannot be found");
        }

        await _userRepository.RemoveAsync(id);
        return Result.Success();
    }
}