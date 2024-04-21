using FluentValidation.TestHelper;
using Moq;
using ProductApi.WebApi.Validators;
using ProductApi.WebApi.ViewModels;

namespace ProductApi.Tests.Validators
{
    [TestClass]
    public class ProductRequestValidatorTests
    {
        private readonly ProductRequestValidator _validator;

        private readonly string _moreThan100Characters =
            "pneumonoultramicroscopicsilicovolcanoconiosisparastratiosphecomyiastratiosphecomyioidesfloccinaucinition"; // 104 characters

        public ProductRequestValidatorTests()
        {
            this._validator = new ProductRequestValidator();
        }

        [TestMethod]
        public void ValidateProductName_Empty_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = It.IsAny<decimal>() })
                .ShouldHaveValidationErrorFor(pr => pr.Name);
        }

        [TestMethod]
        public void ValidateProductName_MoreThanMaxLengthAllowed_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = _moreThan100Characters, BrandName = It.IsAny<string>(), Price = It.IsAny<decimal>() })
                .ShouldHaveValidationErrorFor(pr => pr.Name);
        }

        [TestMethod]
        public void ValidateProductName_NotEmpty_NotMoreThanMaxLengthAllowed_ShouldNotHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = "Product Name", BrandName = It.IsAny<string>(), Price = It.IsAny<decimal>() })
                .ShouldNotHaveValidationErrorFor(pr => pr.Name);
        }

        [TestMethod]
        public void ValidateBrandName_Empty_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = It.IsAny<decimal>()})
                .ShouldHaveValidationErrorFor(pr => pr.BrandName);
        }

        [TestMethod]
        public void ValidateBrandName_MoreThanMaxLengthAllowed_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = _moreThan100Characters, Price = It.IsAny<decimal>() })
                .ShouldHaveValidationErrorFor(pr => pr.BrandName);
        }

        [TestMethod]
        public void ValidateBrandName_NotEmpty_NotMoreThanMaxLengthAllowed_ShouldNotHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = "Brand Name", Price = It.IsAny<decimal>() })
                .ShouldNotHaveValidationErrorFor(pr => pr.BrandName);
        }

        [TestMethod]
        public void ValidateUnitPrice_Zero_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = It.IsAny<decimal>() })
                .ShouldHaveValidationErrorFor(pr => pr.Price);
        }

        [TestMethod]
        public void ValidateUnitPrice_MoreThanMaximumAllowed_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = 1100.00M })
                .ShouldHaveValidationErrorFor(pr => pr.Price);
        }

        [TestMethod]
        public void ValidateUnitPrice_MoreDecimalPrecisionScaleThanAllowed_ShouldHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = 1100.000M })
                .ShouldHaveValidationErrorFor(pr => pr.Price);
        }

        [TestMethod]
        public void ValidateUnitPrice_NotZero_NotMoreThanMaximumAllowed_NotMoreDecimalPrecisionScaleThanAllowed_ShouldNotHaveValidationError()
        {
            this._validator
                .TestValidate(new ProductRequest { Name = It.IsAny<string>(), BrandName = It.IsAny<string>(), Price = 100.00M })
                .ShouldNotHaveValidationErrorFor(pr => pr.Price);
        }
    }
}