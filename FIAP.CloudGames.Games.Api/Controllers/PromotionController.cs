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
[Authorize(Roles = "Admin")]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    [HttpPost]
    [TypeFilter(typeof(ValidationFilter<CreatePromotionRequest>))]
    [ProducesResponseType(typeof(ApiResponse<PromotionResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
    {
        var promotion = await promotionService.CreateAsync(request);
        return this.ApiOk(promotion, "Promotion created successfully.", HttpStatusCode.Created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PromotionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var promotions = await promotionService.ListAsync();
        return this.ApiOk(promotions, "Promotions retrieved successfully.");
    }
}




