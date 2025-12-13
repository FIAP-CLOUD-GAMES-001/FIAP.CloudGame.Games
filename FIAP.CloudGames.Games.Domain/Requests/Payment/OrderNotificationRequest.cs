namespace FIAP.CloudGames.Games.Domain.Requests.Payment;

/// <summary>
/// Request para notificação de atualização de status de pagamento
/// </summary>
public record OrderNotificationRequest(
    /// <summary>
    /// ID do pedido (Order)
    /// </summary>
    string OrderId,
    /// <summary>
    /// ID do pagamento
    /// </summary>
    string PaymentId,
    /// <summary>
    /// Valor do pagamento
    /// </summary>
    decimal Amount,
    /// <summary>
    /// Status do pagamento (Autorizado, Rejeitado, etc.)
    /// </summary>
    string Status,
    /// <summary>
    /// Data do pagamento
    /// </summary>
    DateTime PaymentDate
);

