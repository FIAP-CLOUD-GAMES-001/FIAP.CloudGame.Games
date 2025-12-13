using FIAP.CloudGames.Games.Domain.Enums;

namespace FIAP.CloudGames.Games.Domain.Entities;

public class PaymentEntity : BaseEntity
{
    public string OrderId { get; private set; } = string.Empty;
    public string PaymentId { get; private set; } = string.Empty;
    public decimal OrderAmount { get; private set; }
    public string PaymentMethod { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public EPaymentStatus Status { get; private set; }

    private PaymentEntity() { }

    public PaymentEntity(string orderId, decimal orderAmount, string paymentMethod, DateTime orderDate)
    {
        OrderId = orderId;
        OrderAmount = orderAmount;
        PaymentMethod = paymentMethod;
        OrderDate = orderDate;
        Status = EPaymentStatus.Pending;
        PaymentId = string.Empty;
    }

    public void UpdateStatus(EPaymentStatus newStatus)
    {
        Status = newStatus;
    }

    public void SetPaymentId(string paymentId)
    {
        PaymentId = paymentId;
    }
}

