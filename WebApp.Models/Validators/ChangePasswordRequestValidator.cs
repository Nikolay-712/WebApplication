using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request.Accounts;

namespace WebApp.Models.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
{
    public ChangePasswordRequestValidator()
    {
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
