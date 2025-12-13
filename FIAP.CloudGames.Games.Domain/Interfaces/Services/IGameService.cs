using FIAP.CloudGames.Games.Domain.Models;
using FIAP.CloudGames.Games.Domain.Requests.Game;
using FIAP.CloudGames.Games.Domain.Responses.Game;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;
public interface IGameService
{
    Task<GameResponse> CreateAsync(CreateGameRequest request);
    Task<IEnumerable<GameResponse>> ListAsync();
    Task<IEnumerable<GameResponse>> RecommendationsAsync(int gameId);
    Task<GameElasticMetrics> MetricsAsync();
}




