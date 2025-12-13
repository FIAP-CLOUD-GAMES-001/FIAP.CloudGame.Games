using FIAP.CloudGames.Games.Domain.Requests.Order;
using FIAP.CloudGames.Games.Domain.Responses.Order;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;
public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);
    Task<IEnumerable<OrderResponse>> ListAsync();
    Task<OrderResponse?> GetByIdAsync(int id);
    Task<IEnumerable<OrderResponse>> GetByUserIdAsync(int userId);
    Task UpdateAsync(UpdateOrderRequest request);
}
