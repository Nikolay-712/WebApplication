using FluentValidation;

namespace WebApp.Models.Validators;

public class ApplicationValidator<TModel> : AbstractValidator<TModel>
{
    public Func<object, string, Task<IEnumerable<string>>> ValidateRequestAsync => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<TModel>
            .CreateWithOptions((TModel)model, x => x.IncludeProperties(propertyName)));

        return result.IsValid is true ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}
