using FIAP.CloudGames.Games.Domain.Requests.Payment;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;

public interface IPaymentNotificationService
{
    Task ProcessNotificationAsync(OrderNotificationRequest request);
}

