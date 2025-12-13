using FIAP.CloudGames.Games.Domain.Requests.Payment;
using FluentValidation;

namespace FIAP.CloudGames.Games.Service.Validators;

public class OrderNotificationRequestValidator : AbstractValidator<OrderNotificationRequest>
{
    public OrderNotificationRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.")
            .NotNull().WithMessage("OrderId cannot be null.");

        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("PaymentId is required.")
            .NotNull().WithMessage("PaymentId cannot be null.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid payment status (Pending, Processing, Approved, Rejected).");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("PaymentDate is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
            .WithMessage("PaymentDate cannot be in the future (with 5 minutes tolerance).");
    }
}

