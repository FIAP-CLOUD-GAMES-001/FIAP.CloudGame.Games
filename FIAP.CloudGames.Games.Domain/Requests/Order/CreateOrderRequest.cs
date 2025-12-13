using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Requests.Order;
/// <summary>
/// Request para criação de um novo pedido (Order)
/// </summary>
/// <param name="Games">Array de IDs dos games que serão comprados (consulte GET /api/Order/available-games para ver games disponíveis)</param>
/// <param name="UserId">ID do usuário que está realizando o pedido</param>
/// <param name="PaymentMethod">Método de pagamento escolhido</param>
public record CreateOrderRequest(
    /// <summary>
    /// Array de IDs dos games cadastrados. Use o endpoint GET /api/Order/available-games para obter a lista de games disponíveis.
    /// </summary>
    int[] Games, 
    /// <summary>
    /// ID do usuário que está realizando o pedido
    /// </summary>
    int UserId,
    /// <summary>
    /// Método de pagamento escolhido (CreditCard, DebitCard, Pix, Boleto, GiftCard)
    /// </summary>
    EPaymentMethod PaymentMethod);

