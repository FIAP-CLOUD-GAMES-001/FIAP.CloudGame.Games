using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Api.Filters;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Models;
using FIAP.CloudGames.Games.Domain.Requests.Order;
using FIAP.CloudGames.Games.Domain.Requests.Payment;
using FIAP.CloudGames.Games.Domain.Responses.Game;
using FIAP.CloudGames.Games.Domain.Responses.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Games.Api.Controllers;
/// <summary>
/// Controller para gerenciamento de pedidos (Orders)
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class OrderController(
    IOrderService orderService,
    IGameService gameService,
    IPaymentNotificationService paymentNotificationService) : ControllerBase
{
    /// <summary>
    /// Lista todos os games disponíveis para criação de pedidos
    /// </summary>
    /// <returns>Lista de games cadastrados</returns>
    [HttpGet("available-games")]
    [ProducesResponseType(typeof(ApiResponse<List<GameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableGames()
    {
        var games = await gameService.ListAsync();
        return this.ApiOk(games, "Available games retrieved successfully.");
    }

    /// <summary>
    /// Cria um novo pedido (Order) para um game específico
    /// </summary>
    /// <param name="request">Dados do pedido contendo GameId (use GET /api/Order/available-games para ver games disponíveis) e UserId</param>
    /// <returns>Pedido criado com sucesso</returns>
    [HttpPost]
    [Authorize]
    [TypeFilter(typeof(ValidationFilter<CreateOrderRequest>))]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var order = await orderService.CreateAsync(request);
        return this.ApiOk(order, "Order created successfully.", HttpStatusCode.Created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<OrderResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var orders = await orderService.ListAsync();
        return this.ApiOk(orders, "Orders retrieved successfully.");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await orderService.GetByIdAsync(id);
        if (order == null)
            return this.ApiOk( $"Order with ID {id} not found.", "Orders null");
        
        return this.ApiOk(order, "Order retrieved successfully.");
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<List<OrderResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var orders = await orderService.GetByUserIdAsync(userId);
        return this.ApiOk(orders, $"Orders for user {userId} retrieved successfully.");
    }

    [HttpPut]
    [Authorize]
    [TypeFilter(typeof(ValidationFilter<UpdateOrderRequest>))]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
    {
        await orderService.UpdateAsync(request);
        return this.ApiOk("", "Order updated successfully.");
    }

    /// <summary>
    /// Recebe notificação de atualização de status de pagamento
    /// </summary>
    /// <param name="request">Dados da notificação de pagamento</param>
    /// <returns>Notificação processada com sucesso</returns>
    [HttpPost("payment-notification")]
    [AllowAnonymous]
    [TypeFilter(typeof(ValidationFilter<OrderNotificationRequest>))]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PaymentNotification([FromBody] OrderNotificationRequest request)
    {
        await paymentNotificationService.ProcessNotificationAsync(request);
        return this.ApiOk("", "Payment notification processed successfully.");
    }
}
