using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request.Accounts;

namespace WebApp.Models.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestModel>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.UserName)
            .Length(3, 50)
            .WithMessage(Messages.InvalidLengthRange);

        RuleFor(x => x.UserName)
            .Must(x => x.All(c => char.IsLetterOrDigit(c)))
            .WithMessage(Messages.InvalidUserName)
            .When(x => x.UserName is not null);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.Email)
            .Must(x => ValidatorHelper.ValidateEmail(x))
            .WithMessage(Messages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage(Messages.InvalidLengthRange);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.ConfirmPassword)
            .Length(6, 20)
            .WithMessage(Messages.InvalidLengthRange);

        RuleFor(x => x)
            .Must(x => x.Password.Equals(x.ConfirmPassword)).WithName("Password")
            .When(x => x.Password is not null && x.ConfirmPassword is not null)
            .WithMessage(Messages.PasswordMismatch);
    }
}
