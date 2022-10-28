using System.ComponentModel.DataAnnotations;

namespace EventlyServer.Data.Dto;

/// <summary>
/// Пользовательский аккаунт
/// </summary>
/// <param name="Id">ID пользователя</param>
/// <param name="Name">Имя пользователя</param>
/// <param name="Email">Электронная почта</param>
/// <param name="Password">Пароль</param>
/// <param name="PhoneNumber">Номер телефона</param>
/// <param name="OtherCommunication">Иные контакты (ссылки на соцсети, например)</param>
/// <param name="IsAdmin">Является ли пользователь администратором</param>
public record UserDto([Required] int Id, [Required] string Name, [Required] string Email, [Required] string Password,
    string? PhoneNumber,
    string? OtherCommunication = null, bool IsAdmin = false);

/// <summary>
/// Пользовательский аккаунт (базовая информация)
/// </summary>
/// <param name="Id">ID пользователя</param>
/// <param name="Name">Имя пользователя</param>
/// <param name="Email">Электронная почта</param>
/// <param name="Token">JWT-токен аутентификации</param>
/// <param name="IsAdmin">Является ли пользователь администратором</param>
public record UserShortDto([Required] int Id, [Required] string Name, [Required] string Email, [Required] string Token,
    [Required] bool IsAdmin);

/// <summary>
/// Пользовательский аккаунт (информация для обновления)
/// </summary>
/// <param name="Id">ID пользователя</param>
/// <param name="Name">Имя пользователя</param>
/// <param name="Password">Пароль</param>
/// <param name="PhoneNumber">Номер телефона</param>
/// <param name="OtherCommunication">Иные контакты (ссылки на соцсети, например)</param>
public record UserUpdateDto([Required] int Id, string? Name = null, string? Password = null, string? PhoneNumber = null,
    string? OtherCommunication = "");

/// <summary>
/// Пользовательский аккаунт (информация для логина)
/// </summary>
/// <param name="Email">Электронная почта</param>
/// <param name="Password">Пароль</param>
public record UserLoginDto([Required] string Email, [Required] string Password);

/// <summary>
/// Пользовательский аккаунт (информация для регистрации)
/// </summary>
/// <param name="Name">Имя пользователя</param>
/// <param name="Email">Электронная почта</param>
/// <param name="Password">Пароль</param>
/// <param name="PhoneNumber">Номер телефона</param>
/// <param name="OtherCommunication">Иные контакты (ссылки на соцсети, например)</param>
public record UserRegisterDto([Required] string Name, [Required] string Email, [Required] string Password,
    string? PhoneNumber,
    string? OtherCommunication);