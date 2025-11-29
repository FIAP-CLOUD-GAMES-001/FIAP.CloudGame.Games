using FIAP.CloudGames.Games.Domain.Entities;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
public interface IPromotionRepository
{
    Task AddAsync(PromotionEntity promotion);
    Task<PromotionEntity?> GetByIdAsync(int id);
    Task<List<PromotionEntity>> ListAllAsync();
}




