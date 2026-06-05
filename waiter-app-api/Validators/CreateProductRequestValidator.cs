using System.Globalization;
using System.Text.Json;
using FluentValidation;
using WaiterApp.DTOs;

namespace WaiterApp.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(150).WithMessage("Name must be at most 150 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must be at most 500 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .Must(BeValidPrice).WithMessage("Invalid price format");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .Must(BeValidGuid).WithMessage("Invalid category id");

        RuleFor(x => x.Ingredients)
            .Must(BeValidIngredientsJson).WithMessage("Invalid ingredients format")
            .When(x => !string.IsNullOrWhiteSpace(x.Ingredients));
    }

    private static bool BeValidPrice(string price)
        => decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

    private static bool BeValidGuid(string id)
        => Guid.TryParse(id, out _);

    private static bool BeValidIngredientsJson(string? json)
    {
        try
        {
            JsonSerializer.Deserialize<List<IngredientDto>>(
                json!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return true;
        }
        catch
        {
            return false;
        }
    }
}