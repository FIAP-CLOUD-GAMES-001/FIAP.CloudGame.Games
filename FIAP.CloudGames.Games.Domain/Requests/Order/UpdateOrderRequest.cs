using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Requests.Order;
public record UpdateOrderRequest(int Id, EOrderStatus? Status = null);

