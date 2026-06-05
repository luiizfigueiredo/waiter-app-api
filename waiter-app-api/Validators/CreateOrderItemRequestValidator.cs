using FluentValidation;
using WaiterApp.DTOs;

namespace WaiterApp.Validators;

public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        RuleFor(x => x.Product)
            .NotEmpty().WithMessage("Product id is required")
            .Must(BeValidGuid).WithMessage("Invalid product id");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }

    private static bool BeValidGuid(string id)
        => Guid.TryParse(id, out _);
}