using System.Security.Authentication;
using EventlyServer.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Extensions;

public static class ControllerExtensions
{
    /// <summary>
    /// Возращает готовый к отправке HTTP-Response
    /// </summary>
    /// <param name="controller">Экземпляр контроллера, откуда данный метод расширения вызывается</param>
    /// <param name="getValue">Функция получения отправляемого результата</param>
    /// <typeparam name="T">Тип возвращаемого объекта (в случае успеха)</typeparam>
    /// <returns>Готовый к отправке HTTP-Response</returns>
    public static ActionResult<T> SendResponse<T>(this ControllerBase controller, Func<T> getValue)
    {
        try
        {
            return getValue();
        }
        catch (EntityExistsException e)
        {
            return controller.Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return controller.BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return controller.Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    /// <summary>
    /// Возращает готовый к отправке HTTP-Response
    /// </summary>
    /// <param name="controller">Экземпляр контроллера, откуда данный метод расширения вызывается</param>
    /// <param name="func">Функция выполнения действия</param>
    /// <returns>Готовый к отправке HTTP-Response</returns>
    public static ActionResult SendResponse(this ControllerBase controller, Action func)
    {
        try
        {
            func();
            return controller.Ok();
        }
        catch (EntityExistsException e)
        {
            return controller.Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return controller.BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return controller.Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    /// <summary>
    /// Асинхронно возращает готовый к отправке HTTP-Response
    /// </summary>
    /// <param name="controller">Экземпляр контроллера, откуда данный метод расширения вызывается</param>
    /// <param name="getValueAsync">Функция асинхронного получения отправляемого результата</param>
    /// <typeparam name="T">Тип возвращаемого объекта (в случае успеха)</typeparam>
    /// <returns>Готовый к отправке HTTP-Response</returns>
    public static async Task<ActionResult<T>> SendResponseAsync<T>(this ControllerBase controller, Func<Task<T>> getValueAsync)
    {
        try
        {
            return await getValueAsync();
        }
        catch (EntityExistsException e)
        {
            return controller.Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return controller.BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return controller.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
        }
        catch (Exception e)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    /// <summary>
    /// Асинхронно возращает готовый к отправке HTTP-Response
    /// </summary>
    /// <param name="controller">Экземпляр контроллера, откуда данный метод расширения вызывается</param>
    /// <param name="func">Функция выполнения асинхронного действия</param>
    /// <returns>Готовый к отправке HTTP-Response</returns>
    public static async Task<ActionResult> SendResponseAsync(this ControllerBase controller, Func<Task> func)
    {
        try
        {
            await func();
            return controller.Ok();
        }
        catch (EntityExistsException e)
        {
            return controller.Conflict(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return controller.BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return controller.StatusCode(StatusCodes.Status401Unauthorized, e.Message);
        }
        catch (Exception e)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}