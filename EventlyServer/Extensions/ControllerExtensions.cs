using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using EventlyServer.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EventlyServer.Extensions;

public static class ControllerExtensions
{
    public static ActionResult<T> ToResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return result.Exception switch
        {
            EntityExistsException => new ConflictObjectResult(result.Exception.Message),
            EntityNotFoundException => new BadRequestObjectResult(result.Exception.Message),
            ArgumentNullException => new BadRequestObjectResult(result.Exception.Message),
            ValidationException => new BadRequestObjectResult(result.Exception.Message),
            AuthenticationException => new UnauthorizedObjectResult(result.Exception.Message),
            _ => new ContentResult { StatusCode = 500, Content = result.Exception.Message }
        };
    }

    public static ActionResult<TResponse> ToResponse<TData, TResponse>(this Result<TData> result,
        Func<TData, TResponse> mapper)
    {
        if (result.IsSuccess)
        {
            var response = mapper(result.Value);
            return new OkObjectResult(response);
        }

        return result.Exception switch
        {
            EntityExistsException => new ConflictObjectResult(result.Exception.Message),
            EntityNotFoundException => new BadRequestObjectResult(result.Exception.Message),
            ArgumentNullException => new BadRequestObjectResult(result.Exception.Message),
            ValidationException => new BadRequestObjectResult(result.Exception.Message),
            AuthenticationException => new UnauthorizedObjectResult(result.Exception.Message),
            _ => new ContentResult { StatusCode = 500, Content = result.Exception.Message }
        };
    }

    public static ActionResult ToResponse(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkResult();
        }

        return result.Exception switch
        {
            EntityExistsException => new ConflictObjectResult(result.Exception.Message),
            EntityNotFoundException => new BadRequestObjectResult(result.Exception.Message),
            ArgumentNullException => new BadRequestObjectResult(result.Exception.Message),
            ValidationException => new BadRequestObjectResult(result.Exception.Message),
            AuthenticationException => new UnauthorizedObjectResult(result.Exception.Message),
            _ => new ContentResult { StatusCode = 500, Content = result.Exception.Message }
        };
    }
}