using FluentValidation;
using ProjectIndependence.API.Core.Errors;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private static readonly int[] AllowedTaxes = { 0, 6, 12, 21 };
        public UpdateProductCommandValidator()
        {
            RuleFor(cpm => cpm.Name)
                .NotEmpty()
                .WithMessage(ValidationErrors.Name);

            RuleFor(cpm => cpm.Description)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductCodeEmpty);
        }
    }
}