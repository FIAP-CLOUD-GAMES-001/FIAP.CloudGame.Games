using FIAP.CloudGames.Games.Domain.Responses.Payment;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;

public interface IPaymentQueryService
{
    Task<IEnumerable<PaymentResponse>> ListAllAsync();
    Task<PaymentResponse?> GetByIdAsync(int id);
    Task<IEnumerable<PaymentResponse>> GetByOrderIdAsync(string orderId);
}

