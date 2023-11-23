using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request;

namespace WebApp.Models.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestModel>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.Email)
            .Must(x => ValidatorHelper.ValidateEmail(x))
            .WithMessage(Messages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);
    }
}
