using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Enums;
using FIAP.CloudGames.Games.Domain.Models;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
public interface IGameElasticSearchRepository
{
    Task<GameElasticDocument?> GetByIdAsync(int id);
    Task IndexAsync(GameElasticDocument game);
    Task UpdateAsync(GameElasticDocument game);
    Task DeleteAsync(int id);
    Task<List<GameElasticDocument>> GetRecommendationsAsync(
        int gameId,
        EGameGenre genre,
        string description,
        int size = 10);
}




