namespace FIAP.CloudGames.Games.Domain.Entities;

public class OrderGameEntity
{
    public int OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
    public int GameId { get; set; }
    public GameEntity Game { get; set; } = null!;

    private OrderGameEntity() { }

    public OrderGameEntity(int orderId, int gameId)
    {
        OrderId = orderId;
        GameId = gameId;
    }
}

