using FluentValidation;
using ProjectIndependence.API.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Application.Products.Commands.UpdateProductPrice
{
    public class UpdateProductPriceCommandValidator : AbstractValidator<UpdateProductPriceCommand>
    {
        public UpdateProductPriceCommandValidator()
        {
            RuleFor(c => c.RetailPrice)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductRetailPrice);
            RuleFor(c => c.CostPrice)
                .GreaterThan(0)
                .WithMessage(ValidationErrors.ProductCostPrice)
                .LessThanOrEqualTo(c => c.RetailPrice)
                .WithMessage(ValidationErrors.ProductCostPriceLesserThan);
            RuleFor(c => c.ReasonForPriceChange)
                .NotEmpty()
                .WithMessage(ValidationErrors.ProductPriceChangeReason)
                .MaximumLength(500)
                .WithMessage(ValidationErrors.ProductPriceChangeReasonLength);
        }
    }
}