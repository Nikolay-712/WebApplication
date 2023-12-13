using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request.Roles;

namespace WebApp.Models.Validators;

public class CreteRoleRequestValidator : AbstractValidator<CreateRoleRequestModel>
{
    public CreteRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.Name)
            .Length(5, 25)
            .WithMessage(Messages.InvalidLengthRange);

        RuleFor(x => x.DescriptionEn)
            .NotEmpty()
            .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.DescriptionEn)
            .Length(10, 200)
            .WithMessage(Messages.InvalidLengthRange);

        RuleFor(x => x.DescriptionBg)
           .NotEmpty()
           .WithMessage(Messages.EmptyRequiredField);

        RuleFor(x => x.DescriptionBg)
            .Length(10, 200)
            .WithMessage(Messages.InvalidLengthRange);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CreateRoleRequestModel>.CreateWithOptions((CreateRoleRequestModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
