using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Repositories;
public class PromotionRepository(DataContext context) : IPromotionRepository
{
    public async Task AddAsync(PromotionEntity promotion)
    {
        await context.Promotions.AddAsync(promotion);
        await context.SaveChangesAsync();
    }

    public async Task<PromotionEntity?> GetByIdAsync(int id)
    {
        return await context.Promotions.FindAsync(id);
    }

    public async Task<List<PromotionEntity>> ListAllAsync()
    {
        return await context.Promotions.AsNoTracking().ToListAsync();
    }
}




