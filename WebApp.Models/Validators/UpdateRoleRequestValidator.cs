﻿using FluentValidation;
using WebApp.Common.Resources;
using WebApp.Models.Request.Roles;

namespace WebApp.Models.Validators;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequestModel>
{
    public UpdateRoleRequestValidator()
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
}