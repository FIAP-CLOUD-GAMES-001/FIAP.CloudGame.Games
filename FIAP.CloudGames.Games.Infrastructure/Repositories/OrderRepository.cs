using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Repositories;
public class OrderRepository(DataContext context) : IOrderRepository
{
    public async Task AddAsync(OrderEntity order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task<OrderEntity?> GetByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderGames)
                .ThenInclude(og => og.Game)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<OrderEntity>> ListAllAsync()
    {
        return await context.Orders
            .AsNoTracking()
            .Include(o => o.OrderGames)
                .ThenInclude(og => og.Game)
            .ToListAsync();
    }

    public async Task<List<OrderEntity>> GetByUserIdAsync(int userId)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(o => o.OrderGames)
                .ThenInclude(og => og.Game)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateAsync(OrderEntity order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }
}

