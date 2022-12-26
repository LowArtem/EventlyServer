using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using FluentValidation;

namespace EventlyServer.Controllers.Validators;

public class TemplateValidator : AbstractValidator<TemplateCreatingDto>
{
    public TemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.FilePath).NotEmpty().Must(x => x.ValidateAsFilePath()).MaximumLength(100);
        RuleFor(x => x.PreviewPath).NotEmpty().Must(x => x.ValidateAsFilePath()).MaximumLength(300);
        RuleFor(x => x.IdEvent).NotEmpty().GreaterThan(0);
    }
}