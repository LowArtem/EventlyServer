using EventlyServer.Data.Dto;
using EventlyServer.Extensions;
using FluentValidation;

namespace EventlyServer.Controllers.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
    }
}

public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber)
            .Must(x => x!.ValidateAsPhoneNumber())
            .When(x => x.PhoneNumber != null);
        RuleFor(x => x.OtherCommunication)
            .NotEmpty()
            .MaximumLength(300)
            .When(x => !string.IsNullOrEmpty(x.OtherCommunication));
    }
}

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .Must(x => x!.ValidateAsPhoneNumber())
            .When(x => x.PhoneNumber != null);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name));
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Password));
        RuleFor(x => x.OtherCommunication)
            .NotEmpty()
            .MaximumLength(300)
            .When(x => !string.IsNullOrEmpty(x.OtherCommunication));
    }
}