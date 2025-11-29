using FIAP.CloudGames.Games.Domain.Entities;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
public interface IGameRepository
{
    Task AddAsync(GameEntity game);
    Task<GameEntity?> GetByIdAsync(int id);
    Task<List<GameEntity>> ListAllAsync();
}




