using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using FluentValidation;

namespace EventlyServer.Controllers.Validators;

public class GuestValidator : AbstractValidator<GuestFullCreatingDto>
{
    public GuestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IdInvitation).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PhoneNumber).NotEmpty().Must(x => x.ValidateAsPhoneNumber());
    }
}