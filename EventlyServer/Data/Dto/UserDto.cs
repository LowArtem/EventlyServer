using System.ComponentModel.DataAnnotations;

namespace EventlyServer.Data.Dto;

/// <summary>
/// Пользовательский аккаунт
/// </summary>
public record UserDto
{
    /// <summary>
    /// Пользовательский аккаунт
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Электронная почта</param>
    /// <param name="password">Пароль</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="otherCommunication">Иные контакты (ссылки на соцсети, например)</param>
    /// <param name="isAdmin">Является ли пользователь администратором</param>
    public UserDto([Required] int id, [Required] string name, [Required] string email, [Required] string password,
        string? phoneNumber, string? otherCommunication = null, bool isAdmin = false)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        OtherCommunication = otherCommunication;
        IsAdmin = isAdmin;
    }

    /// <summary>ID пользователя</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Имя пользователя</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Электронная почта</summary>
    [Required]
    public string Email { get; init; }

    /// <summary>Пароль</summary>
    [Required]
    public string Password { get; init; }

    /// <summary>Номер телефона</summary>
    public string? PhoneNumber { get; init; }

    /// <summary>Иные контакты (ссылки на соцсети, например)</summary>
    public string? OtherCommunication { get; init; }

    /// <summary>Является ли пользователь администратором</summary>
    public bool IsAdmin { get; init; }
}

/// <summary>
/// Пользовательский аккаунт (без скрытой информации)
/// </summary>
public record UserSecretDto
{
    /// <summary>
    /// Пользовательский аккаунт
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Электронная почта</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="otherCommunication">Иные контакты (ссылки на соцсети, например)</param>
    /// <param name="isAdmin">Является ли пользователь администратором</param>
    public UserSecretDto([Required] int id, [Required] string name, [Required] string email, string? phoneNumber,
        string? otherCommunication = null, bool isAdmin = false)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        OtherCommunication = otherCommunication;
        IsAdmin = isAdmin;
    }

    /// <summary>ID пользователя</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Имя пользователя</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Электронная почта</summary>
    [Required]
    public string Email { get; init; }

    /// <summary>Номер телефона</summary>
    public string? PhoneNumber { get; init; }

    /// <summary>Иные контакты (ссылки на соцсети, например)</summary>
    public string? OtherCommunication { get; init; }

    /// <summary>Является ли пользователь администратором</summary>
    public bool IsAdmin { get; init; }
}

/// <summary>
/// Пользовательский аккаунт (базовая информация)
/// </summary>
public record UserShortDto
{
    /// <summary>
    /// Пользовательский аккаунт (базовая информация)
    /// </summary>
    /// <param name="id">ID пользователя</param>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Электронная почта</param>
    /// <param name="isAdmin">Является ли пользователь администратором</param>
    public UserShortDto([Required] int id, [Required] string name, [Required] string email, [Required] bool isAdmin)
    {
        Id = id;
        Name = name;
        Email = email;
        IsAdmin = isAdmin;
    }

    /// <summary>ID пользователя</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Имя пользователя</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Электронная почта</summary>
    [Required]
    public string Email { get; init; }

    /// <summary>Является ли пользователь администратором</summary>
    [Required]
    public bool IsAdmin { get; init; }
}

/// <summary>
/// Пользовательский аккаунт (информация для обновления)
/// </summary>
/// <remarks>
/// Все поля необязательны для заполнения - их нужно заполнять, если нужно обновить значение.
///<para></para>
/// Если нужно оставить значение без изменения - передать null (исключение: поле OtherCommunication - оставить
/// без изменений - передать пустую строку)
/// </remarks>
public record UserUpdateDto
{
    /// <summary>
    /// Пользовательский аккаунт (информация для обновления)
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="otherCommunication">Иные контакты (ссылки на соцсети, например)</param>
    public UserUpdateDto(string? name = null, string? password = null, string? phoneNumber = null,
        string? otherCommunication = "")
    {
        Name = name;
        Password = password;
        PhoneNumber = phoneNumber;
        OtherCommunication = otherCommunication;
    }

    /// <summary>Имя пользователя</summary>
    public string? Name { get; init; }

    /// <summary>Пароль</summary>
    public string? Password { get; init; }

    /// <summary>Номер телефона</summary>
    public string? PhoneNumber { get; init; }

    /// <summary>Иные контакты (ссылки на соцсети, например)</summary>
    public string? OtherCommunication { get; init; }
}

/// <summary>
/// Пользовательский аккаунт (информация для логина)
/// </summary>
public record UserLoginDto
{
    /// <summary>
    /// Пользовательский аккаунт (информация для логина)
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <param name="password">Пароль</param>
    public UserLoginDto([Required] string email, [Required] string password)
    {
        Email = email;
        Password = password;
    }
    
    /// <summary>Электронная почта</summary>
    [Required]
    public string Email { get; init; }
    
    /// <summary>Пароль</summary>
    [Required]
    public string Password { get; init; }
}

/// <summary>
/// Пользовательский аккаунт (информация для регистрации)
/// </summary>
public record UserRegisterDto
{
    /// <summary>
    /// Пользовательский аккаунт (информация для регистрации)
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Электронная почта</param>
    /// <param name="password">Пароль</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="otherCommunication">Иные контакты (ссылки на соцсети, например)</param>
    public UserRegisterDto([Required] string name, [Required] string email, [Required] string password,
        string? phoneNumber, string? otherCommunication)
    {
        Name = name;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        OtherCommunication = otherCommunication;
    }

    /// <summary>Имя пользователя</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Электронная почта</summary>
    [Required]
    public string Email { get; init; }

    /// <summary>Пароль</summary>
    [Required]
    public string Password { get; init; }

    /// <summary>Номер телефона</summary>
    public string? PhoneNumber { get; init; }

    /// <summary>Иные контакты (ссылки на соцсети, например)</summary>
    public string? OtherCommunication { get; init; }
}