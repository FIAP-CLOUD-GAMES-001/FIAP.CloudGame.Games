using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Responses.Order;
public record OrderResponse(
    int Id, 
    List<GameOrderResponse> Games,
    decimal TotalAmount,
    EOrderStatus Status, 
    int UserId, 
    DateTime CreatedAt);

