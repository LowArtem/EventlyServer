using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EventlyServer.Services.Security;

/// <summary>
/// Служебный класс для описания настроек генерации JWT-токена
/// </summary>
public static class AuthOptions
{
    public const string ISSUER = "inHolidayServer";
    public const string AUDIENCE = "inHolidayClient";
    public const int LIFETIME = 60;

    private static readonly string Key = Environment.GetEnvironmentVariable("AUTH_SECRET_KEY") ??
                                   throw new InvalidOperationException("Cannot get auth key from the environment");
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}