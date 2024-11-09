using Core.Entities;
using FluentValidation;

namespace API.Validators
{
    public class CreateProductValidator : AbstractValidator<Product>
    {
        public CreateProductValidator(){
            RuleFor(x => x.Name).NotEmpty().
                                WithMessage("Name is required");

            RuleFor(x => x.Description).NotEmpty()
                                       .WithMessage("Description is required");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required")
                                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.PictureUrl).NotEmpty()
                                     .WithMessage("PictureUrl is required");

            RuleFor(x => x.Type).NotEmpty()
                                 .WithMessage("Type is required");

            RuleFor(x => x.Brand).NotEmpty()
                                  .WithMessage("Brand is required");

            RuleFor(x => x.QuantityInStocks).NotEmpty()
                                            .WithMessage("QuantityInStocks is required")
                                           .GreaterThan(0).WithMessage("QuantityInStocks must be greater than 0");
        }
    }
}