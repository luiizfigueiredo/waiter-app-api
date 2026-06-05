using FluentValidation;
using WaiterApp.DTOs;

namespace WaiterApp.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Icon is required")
            .MaximumLength(50).WithMessage("Icon must be at most 50 characters");
    }
}