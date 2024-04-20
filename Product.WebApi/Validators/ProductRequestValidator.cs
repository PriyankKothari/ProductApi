using FluentValidation;
using ProductApi.WebApi.ViewModels;

namespace ProductApi.WebApi.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(pr => pr.Name)
                .NotEmpty().WithMessage("Please specify Product Name")
                .MaximumLength(100).WithMessage("Product name should be 100 characters or less");

            RuleFor(pr => pr.BrandName)
                .NotEmpty().WithMessage("Please specify Brand Name")
                .MaximumLength(100).WithMessage("Brand Name should be 100 characters or less");

            RuleFor(pr => pr.Price)
                .GreaterThan(0.00m)
                .WithMessage("Unit price must be greater than 0");
        }
    }
}