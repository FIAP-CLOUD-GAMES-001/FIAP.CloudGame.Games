using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Games.Infrastructure.Repositories;
public class GameRepository(DataContext context) : IGameRepository
{
    public async Task AddAsync(GameEntity game)
    {
        await context.Games.AddAsync(game);
        await context.SaveChangesAsync();
    }

    public async Task<GameEntity?> GetByIdAsync(int id)
    {
        return await context.Games.FindAsync(id);
    }

    public async Task<List<GameEntity>> ListAllAsync()
    {
        return await context.Games.AsNoTracking().ToListAsync();
    }
}




