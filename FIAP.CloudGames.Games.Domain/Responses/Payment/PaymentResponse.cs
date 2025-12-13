using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Responses.Payment;

public record PaymentResponse(
    int Id,
    string OrderId,
    decimal OrderAmount,
    string PaymentMethod,
    DateTime OrderDate,
    EPaymentStatus Status,
    DateTime CreatedAt);

