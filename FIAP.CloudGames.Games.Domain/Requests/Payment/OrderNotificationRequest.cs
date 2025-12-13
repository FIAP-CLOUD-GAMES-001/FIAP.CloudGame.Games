using FIAP.CloudGames.Games.Domain.Enums;
using System.Text.Json.Serialization;

namespace FIAP.CloudGames.Games.Domain.Requests.Payment;

public class OrderNotificationRequest
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = null!;
    [JsonPropertyName("paymentId")]
    public string PaymentId { get; set; } = null!;
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("status")]
    public EPaymentStatus Status { get; set; }
    [JsonPropertyName("paymentDate")]
    public DateTime PaymentDate { get; set; }
}