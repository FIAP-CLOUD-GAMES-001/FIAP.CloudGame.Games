using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Entities;
public class OrderEntity : BaseEntity
{
    public EOrderStatus Status { get; private set; }
    public int UserId { get; private set; }
    public ICollection<OrderGameEntity> OrderGames { get; private set; } = new List<OrderGameEntity>();

    private OrderEntity() { }

    public OrderEntity(int userId, IEnumerable<int> gameIds)
    {
        UserId = userId;
        Status = EOrderStatus.Created;
        
        foreach (var gameId in gameIds)
        {
            var orderGame = new OrderGameEntity(0, gameId);
            orderGame.Order = this; // Estabelecer a referência para o EF Core
            OrderGames.Add(orderGame);
        }
    }

    public void UpdateStatus(EOrderStatus newStatus)
    {
        Status = newStatus;
    }
}
