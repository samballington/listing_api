using FluentValidation;
using VehicleListing.Api.DTOs;

namespace VehicleListing.Api.Validators;

public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleRequest>
{
    public UpdateVehicleValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue);

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Mileage.HasValue);

        RuleFor(x => x.Status)
            .Must(s => s == "Available" || s == "Sold" || s == "Pending")
            .WithMessage("Status must be one of: Available, Sold, Pending")
            .When(x => x.Status is not null);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);
    }
}
