
using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Api.Filters;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Models;
using FIAP.CloudGames.Games.Domain.Requests.Game;
using FIAP.CloudGames.Games.Domain.Responses.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Games.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [TypeFilter(typeof(ValidationFilter<CreateGameRequest>))]
    [ProducesResponseType(typeof(ApiResponse<GameResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest request)
    {
        var game = await gameService.CreateAsync(request);
        return this.ApiOk(game, "Game created successfully.", HttpStatusCode.Created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var games = await gameService.ListAsync();
        return this.ApiOk(games, "Games retrieved successfully.");
    }

    [HttpGet("{id}/recommendations")]
    [ProducesResponseType(typeof(ApiResponse<List<GameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Recommendations(int id)
    {
        var games = await gameService.RecommendationsAsync(id);
        return this.ApiOk(games, "Games retrieved successfully.");
    }

    [HttpGet("metrics")]
    [ProducesResponseType(typeof(ApiResponse<GameElasticMetrics>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Metrics()
    {
        var metrics = await gameService.MetricsAsync();
        return this.ApiOk(metrics, "Metrics retrieved successfully.");
    }
}