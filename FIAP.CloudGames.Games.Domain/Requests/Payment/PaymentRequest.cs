using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Requests.Payment;

/// <summary>
/// Request para envio de pagamento ao projeto de pagamentos
/// </summary>
public class PaymentRequest
{
    public string OrderId { get; set; } = string.Empty;
    public decimal OrderAmount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime OrderDate { get; set; }

   
    public PaymentRequest() { }

    public PaymentRequest(string orderId, decimal orderAmount, DateTime orderDate, EPaymentMethod ePaymentMethod)
    {
        OrderId = orderId;
        OrderAmount = orderAmount;
        OrderDate = orderDate;
        PaymentMethod = ePaymentMethod.ToString();
    }
}



