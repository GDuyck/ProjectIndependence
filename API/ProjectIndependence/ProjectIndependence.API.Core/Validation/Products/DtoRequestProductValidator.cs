using FluentValidation;
using ProjectIndependence.API.Core.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.Validation.Products
{
    public class DtoRequestProductValidator : AbstractValidator<DtoCreateProduct>
    {
        public DtoRequestProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("A name is required");
            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage("A pruduct must be given a price");
            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("A pruduct price can not be 0");
            RuleFor(p => p.Price)
                .PrecisionScale(2, 10, true)
                .WithMessage("Price must have 2 decimal places");
            RuleFor(p => p.Tax)
                .NotEmpty()
                .WithMessage("There must be a tax");

        }
    }
}
