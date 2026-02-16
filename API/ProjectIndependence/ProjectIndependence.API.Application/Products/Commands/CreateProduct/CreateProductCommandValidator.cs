using FluentValidation;
using ProjectIndependence.API.Core.Errors;

namespace ProjectIndependence.API.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private static readonly int[] AllowedTaxes = [0, 6, 12, 21];

        public CreateProductCommandValidator()
        {
            RuleFor(cpm => cpm.ProductCode)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductCodeEmpty)
                .Length(3, 20)
                .WithMessage(ValidationErrors.ProductCodeLength)
                .Matches("^[A-Za-z0-9_-]+$")
                .WithMessage(ValidationErrors.ProductCodeAlphanumeric);

            RuleFor(cpm => cpm.Name)
                .NotEmpty()
                .WithMessage(ValidationErrors.Name);

            RuleFor(cpm => cpm.Description)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductCodeEmpty);

            RuleFor(cpm => cpm.RetailPrice)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductPriceNotZero)
                .GreaterThanOrEqualTo(cmp => cmp.CostPrice)
                .WithMessage(ValidationErrors.ProductRetailPriceBiggerThanCostPrice);

            RuleFor(cpm => cpm.CostPrice)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductCostPrice)
                .LessThanOrEqualTo(cmp => cmp.RetailPrice)
                .WithMessage(ValidationErrors.ProductCostPriceLesserThan);

            RuleFor(cpm => cpm.Stock)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductStock);

            RuleFor(cmp => cmp.Tax)
                .Must(t => AllowedTaxes.Contains(t))
                .WithMessage($"Invalid tax rate. Allowed values: {string.Join(", ", AllowedTaxes)}");
        }
    }
}