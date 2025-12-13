using FIAP.CloudGames.Games.Domain.Requests.Order;
using FluentValidation;

namespace FIAP.CloudGames.Games.Service.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Games)
            .NotNull().WithMessage("Games array cannot be null.")
            .NotEmpty().WithMessage("At least one game must be selected.")
            .Must(games => games != null && games.Length > 0).WithMessage("At least one game must be selected.");

        RuleForEach(x => x.Games)
            .GreaterThan(0).WithMessage("Each GameId must be greater than zero.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than zero.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}

