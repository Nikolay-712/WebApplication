using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request;

namespace WebApp.Models.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.Email)
            .Must(x => ValidatorHelper.ValidateEmail(x))
            .WithMessage(Messages.InvalidEmail);
    }
}
