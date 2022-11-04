using EventlyServer.Controllers.Abstracts;
using EventlyServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Controllers;

public class TestController : BaseApiController
{
    /// <summary>
    /// Получить текущее серверное время
    /// </summary>
    /// <returns>Текущее серверное время</returns>
    /// <response code="200">Текущее серверное время</response>
    /// <response code="500">Неизвестная ошибка сервера</response>
    [HttpGet]
    [Route("/time")]
    public ActionResult<string> GetCurrentDateTime()
    {
        return Ok(DateTime.UtcNow);
    }

    /// <summary>
    /// Получить email текущего авторизованного пользователя
    /// </summary>
    /// <returns>Email текущего авторизованного пользователя</returns>
    /// <remarks>
    /// Требуется авторизация пользователя или администратора
    /// </remarks>
    /// <response code="200">Email текущего авторизованного пользователя</response>
    /// <response code="500">Неизвестная ошибка сервера (вероятнее БД)</response>
    /// <response code="400">Неверный токен</response>
    /// <response code="401">Ошибка авторизации</response>
    [HttpGet]
    [Route("/whoami")]
    [Authorize]
    public ActionResult<string> GetCurrentUserEmail()
    {
        return UserEmail.ToResponse();
    }
}