using System.ComponentModel.DataAnnotations;

namespace EventlyServer.Extensions;

public static class ValidationResult
{
    public static Result ToResult(this FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.IsValid
            ? Result.Success()
            : Result.Fail(new ValidationException(validationResult.ToString()));
    }
}