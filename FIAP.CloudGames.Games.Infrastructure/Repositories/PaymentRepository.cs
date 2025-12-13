using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Repositories;

public class PaymentRepository(DataContext context) : IPaymentRepository
{
    public async Task AddAsync(PaymentEntity payment)
    {
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }

    public async Task<PaymentEntity?> GetByIdAsync(int id)
    {
        return await context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PaymentEntity>> ListAllAsync()
    {
        return await context.Payments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PaymentEntity>> GetByOrderIdAsync(string orderId)
    {
        return await context.Payments
            .AsNoTracking()
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<PaymentEntity?> GetByOrderIdForUpdateAsync(string orderId)
    {
        return await context.Payments
            .Where(p => p.OrderId == orderId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaymentEntity?> GetByPaymentIdAsync(string paymentId)
    {
        return await context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task UpdateAsync(PaymentEntity payment)
    {
        context.Payments.Update(payment);
        await context.SaveChangesAsync();
    }
}

