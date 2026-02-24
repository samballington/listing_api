using FluentValidation;
using VehicleListing.Api.DTOs;

namespace VehicleListing.Api.Validators;

public class CreateVehicleValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DealerId)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => s == "Available" || s == "Sold" || s == "Pending")
            .WithMessage("Status must be one of: Available, Sold, Pending");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);
    }
}
