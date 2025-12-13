using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Models;
using FIAP.CloudGames.Games.Domain.Responses.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.Games.Api.Controllers;

/// <summary>
/// Controller para consulta de pagamentos (Payments)
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class PaymentController(IPaymentQueryService paymentQueryService) : ControllerBase
{
    /// <summary>
    /// Lista todos os pagamentos
    /// </summary>
    /// <returns>Lista de pagamentos cadastrados</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PaymentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var payments = await paymentQueryService.ListAllAsync();
        return this.ApiOk(payments, "Payments retrieved successfully.");
    }

    /// <summary>
    /// Obtém um pagamento por ID
    /// </summary>
    /// <param name="id">ID do pagamento</param>
    /// <returns>Pagamento encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await paymentQueryService.GetByIdAsync(id);
        if (payment == null)
            return this.ApiOk($"Payment with ID {id} not found.", "Payment null");
        
        return this.ApiOk(payment, "Payment retrieved successfully.");
    }

    /// <summary>
    /// Obtém pagamentos por OrderId
    /// </summary>
    /// <param name="orderId">ID do pedido</param>
    /// <returns>Lista de pagamentos do pedido</returns>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(typeof(ApiResponse<List<PaymentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByOrderId(string orderId)
    {
        var payments = await paymentQueryService.GetByOrderIdAsync(orderId);
        return this.ApiOk(payments, $"Payments for order {orderId} retrieved successfully.");
    }
}

