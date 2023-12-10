using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request.Roles;

namespace WebApp.Models.Validators;

public class RemoveUserFromRoleRequestValidator : AbstractValidator<RemoveFromRoleRequestModel>
{
    public RemoveUserFromRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);
    }
}
