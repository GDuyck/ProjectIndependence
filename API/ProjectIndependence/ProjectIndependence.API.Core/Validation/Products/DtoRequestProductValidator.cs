using FluentValidation;
using ProjectIndependence.API.Core.Dtos.Products;
using ProjectIndependence.API.Core.Errors;

namespace ProjectIndependence.API.Core.Validation.Products
{
    public class DtoRequestProductValidator : AbstractValidator<DtoCreateProduct>
    {
        public DtoRequestProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage(ValidationErrors.Name);
            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductPrice);
            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductPriceNotZero);
            //RuleFor(p => p.Price)
            //    .PrecisionScale(2, 10, true)
            //    .WithMessage("Price must have 2 decimal places");
            RuleFor(p => p.Tax)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductTax);
        }
    }
}