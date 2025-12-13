using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Responses.Payment;

namespace FIAP.CloudGames.Games.Service.Payment;

public class PaymentQueryService(IPaymentRepository paymentRepository) : IPaymentQueryService
{
    public async Task<IEnumerable<PaymentResponse>> ListAllAsync()
    {
        var payments = await paymentRepository.ListAllAsync();
        return payments.Select(p => new PaymentResponse(
            p.Id,
            p.OrderId,
            p.OrderAmount,
            p.PaymentMethod,
            p.OrderDate,
            p.Status,
            p.CreatedAt));
    }

    public async Task<PaymentResponse?> GetByIdAsync(int id)
    {
        var payment = await paymentRepository.GetByIdAsync(id);
        if (payment == null) return null;

        return new PaymentResponse(
            payment.Id,
            payment.OrderId,
            payment.OrderAmount,
            payment.PaymentMethod,
            payment.OrderDate,
            payment.Status,
            payment.CreatedAt);
    }

    public async Task<IEnumerable<PaymentResponse>> GetByOrderIdAsync(string orderId)
    {
        var payments = await paymentRepository.GetByOrderIdAsync(orderId);
        return payments.Select(p => new PaymentResponse(
            p.Id,
            p.OrderId,
            p.OrderAmount,
            p.PaymentMethod,
            p.OrderDate,
            p.Status,
            p.CreatedAt));
    }
}

