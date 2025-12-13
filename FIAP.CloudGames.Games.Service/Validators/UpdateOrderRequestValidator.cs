using FIAP.CloudGames.Games.Domain.Requests.Order;
using FluentValidation;

namespace FIAP.CloudGames.Games.Service.Validators;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Order ID must be greater than zero.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid order status.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x)
            .Must(x => x.Status.HasValue)
            .WithMessage("Status must be provided for update.");
    }
}

