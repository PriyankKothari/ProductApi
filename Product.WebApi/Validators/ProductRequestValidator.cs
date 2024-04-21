using FluentValidation;
using ProductApi.WebApi.ViewModels;

namespace ProductApi.WebApi.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(pr => pr.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(pr => pr.BrandName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(pr => pr.Price)
                .NotEmpty()
                .LessThanOrEqualTo(1000.00m)
                .GreaterThanOrEqualTo(1.00m)
                .PrecisionScale(6, 2, false);
        }
    }
}