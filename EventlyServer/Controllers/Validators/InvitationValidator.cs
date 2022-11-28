using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using FluentValidation;

namespace EventlyServer.Controllers.Validators;

public class InvitationCreatingValidator : AbstractValidator<LandingInvitationCreatingDto>
{
    public InvitationCreatingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IdTemplate).NotEmpty().GreaterThan(0);
        RuleFor(x => x.StartDate).NotEmpty().GreaterThan(DateTime.Today);
        RuleFor(x => x.FinishDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}

public class InvitationUpdatingValidator : AbstractValidator<LandingInvitationUpdatingDto>
{
    public InvitationUpdatingValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Link)
            .Must(x => x!.ValidateAsUrl())
            .When(x => !string.IsNullOrEmpty(x.Link));
        RuleFor(x => x.IdTemplate)
            .NotEmpty().GreaterThan(0)
            .When(x => x.IdTemplate != null);
        RuleFor(x => x.StartDate)
            .NotEmpty().GreaterThan(DateTime.Today)
            .When(x => x.StartDate != null);
        RuleFor(x => x.FinishDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.FinishDate != null);
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
}