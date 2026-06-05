using FluentValidation;
using WaiterApp.DTOs;

namespace WaiterApp.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Table)
            .NotEmpty().WithMessage("Table is required")
            .MaximumLength(50).WithMessage("Table must be at most 50 characters");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Order must contain at least one product");

        RuleForEach(x => x.Products)
            .SetValidator(new CreateOrderItemRequestValidator());
    }
}