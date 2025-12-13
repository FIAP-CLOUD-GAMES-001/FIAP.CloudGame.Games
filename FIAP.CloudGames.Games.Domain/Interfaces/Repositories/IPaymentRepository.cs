using FIAP.CloudGames.Games.Domain.Entities;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(PaymentEntity payment);
    Task<PaymentEntity?> GetByIdAsync(int id);
    Task<PaymentEntity?> GetByPaymentIdAsync(string paymentId);
    Task<List<PaymentEntity>> ListAllAsync();
    Task<List<PaymentEntity>> GetByOrderIdAsync(string orderId);
    Task<PaymentEntity?> GetByOrderIdForUpdateAsync(string orderId);
    Task UpdateAsync(PaymentEntity payment);
}

