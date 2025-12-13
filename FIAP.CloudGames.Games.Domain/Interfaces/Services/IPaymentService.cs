using FIAP.CloudGames.Games.Domain.Requests.Payment;

namespace FIAP.CloudGames.Games.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task SendPaymentRequestAsync(PaymentRequest paymentRequest);
}


