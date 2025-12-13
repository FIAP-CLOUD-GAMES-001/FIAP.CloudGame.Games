using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Enums;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Models;
using FIAP.CloudGames.Games.Domain.Requests.Game;
using FIAP.CloudGames.Games.Domain.Responses.Game;

namespace FIAP.CloudGames.Games.Service.Game;
public class GameService(IGameRepository gameRepository, IGameElasticSearchRepository gameElasticSearchRepository) : IGameService
{
    public async Task<GameResponse> CreateAsync(CreateGameRequest request)
    {
        var game = new GameEntity(request.Title, request.Description, request.Price, request.Genre, request.ReleaseDate);
        await gameRepository.AddAsync(game);

        var gameElastic = new GameElasticDocument
        {
            Id = game.Id.ToString(),
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            Genre = game.Genre.ToString(),
            ReleaseDate = game.ReleaseDate,
            CreatedAt = game.CreatedAt
        };
        await gameElasticSearchRepository.IndexAsync(gameElastic);
        return new GameResponse(game.Id, game.Title, game.Description, game.Price, game.Genre, game.ReleaseDate);
    }

    public async Task<IEnumerable<GameResponse>> ListAsync()
    {
        var games = await gameRepository.ListAllAsync();
        return games.Select(g => new GameResponse(g.Id, g.Title, g.Description, g.Price, g.Genre, g.ReleaseDate));
    }

    public async Task<IEnumerable<GameResponse>> RecommendationsAsync(int gameId)
    {
        var game = await gameRepository.GetByIdAsync(gameId);
        if (game is null)
            return [];

        var games = await gameElasticSearchRepository.GetRecommendationsAsync(game.Id, game.Genre, game.Description);
        return games.Select(g => new GameResponse(int.Parse(g.Id), g.Title, g.Description, g.Price, Enum.Parse<EGameGenre>(g.Genre), g.ReleaseDate));
    }

    public async Task<GameElasticMetrics> MetricsAsync()
    {
        var metrics = await gameElasticSearchRepository.GetMetricsAsync();

        return metrics;
    }
}




