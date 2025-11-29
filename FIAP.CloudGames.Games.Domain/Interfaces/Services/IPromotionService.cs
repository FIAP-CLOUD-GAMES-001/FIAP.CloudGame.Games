using FIAP.CloudGames.Games.Domain.Requests.Game;
using FIAP.CloudGames.Games.Domain.Responses.Game;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;
public interface IPromotionService
{
    Task<PromotionResponse> CreateAsync(CreatePromotionRequest request);
    Task<IEnumerable<PromotionResponse>> ListAsync();
}




