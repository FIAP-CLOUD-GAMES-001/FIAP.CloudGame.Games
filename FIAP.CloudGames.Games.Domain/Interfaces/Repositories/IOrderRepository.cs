using FIAP.CloudGames.Games.Domain.Entities;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
public interface IOrderRepository
{
    Task AddAsync(OrderEntity order);
    Task<OrderEntity?> GetByIdAsync(int id);
    Task<List<OrderEntity>> ListAllAsync();
    Task<List<OrderEntity>> GetByUserIdAsync(int userId);
    Task UpdateAsync(OrderEntity order);
}

