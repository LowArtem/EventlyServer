using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;
using EventlyServer.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers.Abstracts;

/// <summary>
/// Базовый контроллер
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    /// <summary>
    /// ID авторизованного пользователя
    /// </summary>
    /// <remarks>
    /// Провал с ArgumentNullException если пользователь не авторизован
    /// <para></para>
    /// Провал с ValidationException если тип значения не соответствует типу ID (<c>int</c>)
    /// </remarks>
    protected Result<int> UserId
    {
        get
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            if (claim is null)
            {
                return new ArgumentNullException(nameof(claim), "User ID is null");
            }

            if (!int.TryParse(claim.Value, out var userId))
            {
                return new ValidationException($"Cannot parse userId ({claim.Value}) to int");
            }

            return userId;
        }
    }
    
    /// <summary>
    /// Логин (email) авторизованного пользователя
    /// </summary>
    /// <remarks>Провал с ArgumentNullException если пользователь не авторизован</remarks>
    protected Result<string> UserEmail
    {
        get
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (claim is null)
            {
                return new ArgumentNullException(nameof(claim), "User login is null");
            }

            return claim.Value;
        }
    }
}