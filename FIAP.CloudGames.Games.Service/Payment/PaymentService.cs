using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Requests.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FIAP.CloudGames.Games.Service.Payment;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymentService(HttpClient httpClient, ILogger<PaymentService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SendPaymentRequestAsync(PaymentRequest paymentRequest)
    {
        try
        {
            
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token JWT não encontrado no header Authorization");
                throw new UnauthorizedAccessException("Token JWT não encontrado no header Authorization");
            }

           
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring(7);
            }
            
           
            var paymentUrl = new Uri(_httpClient.BaseAddress!, "/api/payment");
            var request = new HttpRequestMessage(HttpMethod.Post, paymentUrl)
            {
                Content = JsonContent.Create(paymentRequest)
            };

            
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro ao enviar requisição de pagamento. Status: {StatusCode}, Erro: {Error}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException($"Erro ao enviar requisição de pagamento: {response.StatusCode}");
            }

            _logger.LogInformation("Requisição de pagamento enviada com sucesso para OrderId: {OrderId}", paymentRequest.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar requisição de pagamento para OrderId: {OrderId}", paymentRequest.OrderId);
            throw;
        }
    }
}


