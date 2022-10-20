﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventlyServer.Services.Security;

/// <summary>
/// Класс для генерации JWT-токенов для авторизации пользователей
/// </summary>
public class TokenService
{
    private readonly IRepository<User> _userRepository;

    public TokenService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Асинхронно генерирует токен
    /// </summary>
    /// <param name="login">имейл пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>сгенерированный токен</returns>
    /// <exception cref="AuthenticationException">если пользователь с такими учетными данными не обнаружен</exception>
    public async Task<string> GenerateTokenAsync(string login, string password)
    {
        var identity = await GetIdentityAsync(login, password);
        if (identity == null)
        {
            throw new AuthenticationException("User with these credentials cannot be found");
        }
 
        var now = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Получает логин (имейл) пользователя из токена
    /// </summary>
    /// <param name="token">JWT-токен</param>
    /// <returns>имейл пользователя или null, если токен некорректен</returns>
    public static string? GetLoginFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var decryptedToken = jsonToken as JwtSecurityToken;
        return decryptedToken?.Claims.First().Value;
    }

    /// <summary>
    /// Асинхронно получает идентифицирующую информацию о пользователе:
    /// логин (имейл) и роль (user/admin)
    /// </summary>
    /// <param name="email">имейл пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>идентифицирующую информацию</returns>
    private async Task<ClaimsIdentity?> GetIdentityAsync(string email, string password)
    {
        var user = await _userRepository
            .Items
            .FirstOrDefaultAsync(item => item.Email == email && item.Password == password);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, GetUserRole(user))
            };
            ClaimsIdentity? claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        return null;
    }

    private string GetUserRole(User user)
    {
        return user.IsAdmin ? nameof(UserRoles.ADMIN) : nameof(UserRoles.USER);
    }
}